﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using FileWinSvcWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Accessors
{
    public class IsaeDwAccessor : IDisposable
    {
        //private const string CONNECTION_STRING = @"server='{0}\ISAESQLSERVER';Initial Catalog=IsaeDw;user ID='IsaeDw';password='IsaeDw'";
        private const string CONNECTION_STRING = @"server='{0}';Initial Catalog=IsaeDw;user ID='IsaeDw';password='IsaeDw'";

        private SqlConnection connection;
        private bool disposed = false;

        private string getDimColumnsCsv(RequestData request, string prefix=null)
        {
            if (request.Dimensions == null || request.Dimensions.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(prefix))
            {
                return string.Join(",", request.Dimensions);
            }
            return string.Join(",", $"{prefix}.{request.Dimensions}" );
        }

        internal List<ResultData> queryData(RequestData request)
        {
            //string paramDimensionColumns = getDimColumnsCsv(request);
            //string paramKpiColumns = getDimColumnsCsv(request, null);

            //string sqlQuery = @"DECLARE  @LatestCollectionKey INT = (select top 1 CollectionDateKey from FactDataCollection order by CollectionDateKey desc);" +
            //                "select d.HierarchyPath ,d.KpiStatus, d.KpiStatusValue, d.KpiValue," +
            //                $"{ }" + 
            //                "k.Name as KpiName, " +
            //                "kg.Name as KpiGroupName " +
            //                "JOIN DimHierarchy h ON h.Fullpath = d.HierarchyPath " +
            //                "where d.CollectionDateKey = @LatestCollectionKey " +
            //                "AND h.CollectionDateKey = @LatestCollectionKey " +
            //                "AND h.";
            throw new NotImplementedException();
        }

        public IsaeDwAccessor(string serverName)
        {
            try
            {
                string connStr = string.Format(CONNECTION_STRING, serverName);
                connection = new SqlConnection(connStr);
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection error. " + ex.ToString());
                throw;
            }
        }

        private static bool CheckDatabaseExists(SqlConnection sqlConnection, string databaseName)
        {
            using (SqlCommand command = new SqlCommand(string.Format("SELECT db_id('{0}')", databaseName), sqlConnection))
            {
                return (command.ExecuteScalar() != DBNull.Value);
            }
        }

        public List<HierarchyBase> GetConsolidatedHierarchy()
        {
            // Dillema: do we send it out in hierarchical structure or flat structure?
            // For this function, just give the flat structure
            // Maybe make it a parameter whether to give it as flat or hierarchical
            List<HierarchyBase> retList = new List<HierarchyBase>();

            using (SqlCommand command = new SqlCommand(@"SELECT * FROM DimHierarchyConsolidated", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HierarchyBase newHier = new HierarchyBase()
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            //ParentId = reader["ParentId"] == DBNull.Value ? null : Convert.ToInt64(reader["ParentId"]),
                            ParentId = null,
                            ServerName = reader["ServerName"].ToString(),
                            NodeName = reader["NodeName"].ToString(),
                            FullPath = reader["FullPath"].ToString(),
                            UnitType = reader["UnitType"].ToString(),
                            Children = new List<HierarchyBase>()
                        };

                        if (reader["ParentId"] != DBNull.Value)
                        {
                            newHier.ParentId = Convert.ToInt64(reader["ParentId"]);
                        }

                        retList.Add(newHier);
                    }
                }
            }

            return retList;
        }

        // Will only return the direct KPI's associated to a hierarchy.
        // If there is a need to retrive all the KPI's under a folder, the Javascript client will take care of consolidating.
        public Dictionary<string, List<KpiInfo>> GetConsolidatedHierarchyKpiOrig()
        {
            Dictionary<string, List<KpiInfo>> retDict = new Dictionary<string, List<KpiInfo>>();
            using (SqlCommand command = new SqlCommand(@"select FullPath, NodeName, g.Name as KpiGroupName, k.Name as KpiName " +
                                                        "from DimHierarchyKpiConsolidated conso " +
                                                        "JOIN DimHierarchyConsolidated h ON conso.HierarchyConsoId = h.Id " +
                                                        "JOIN DimKpi k ON conso.KpiConsoId = k.Id " +
                                                        "JOIN DimKpiGroup g ON g.Id = k.KpiGroupId ",
                                                        connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string FullPath = reader["FullPath"].ToString();
                        List<KpiInfo> currKpiList = null;

                        if (!retDict.TryGetValue(FullPath, out currKpiList))
                        {
                            currKpiList = new List<KpiInfo>();
                            retDict[FullPath] = currKpiList;
                        }

                        KpiInfo newData = new KpiInfo()
                        {
                            KpiGroupName = reader["KpiGroupName"].ToString(),
                            KpiName = reader["KpiName"].ToString(),
                        };

                        // We can assume that all items from the query are unique
                        currKpiList.Add(newData);
                    }
                }
            }

            return retDict;
        }

        // Will only return the direct KPI's associated to a hierarchy.
        // If there is a need to retrive all the KPI's under a folder, the Javascript client will take care of consolidating.
        // Dict Key: Path
        // Dict Value: Dict of Kpi Group and Kpi List
        // Note: this API is meant for lookup purposes, so keep it small.
        public Dictionary<string, Dictionary<string, List<string>>> GetConsolidatedHierarchyKpi()
        {
            Dictionary<string, Dictionary<string, List<string>>> retDict = new Dictionary<string, Dictionary<string, List<string>>>();
            using (SqlCommand command = new SqlCommand(@"select FullPath, NodeName, g.Name as KpiGroupName, k.Name as KpiName " +
                                                        "from DimHierarchyKpiConsolidated conso " +
                                                        "JOIN DimHierarchyConsolidated h ON conso.HierarchyConsoId = h.Id " +
                                                        "JOIN DimKpi k ON conso.KpiConsoId = k.Id " +
                                                        "JOIN DimKpiGroup g ON g.Id = k.KpiGroupId ",
                                                        connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // [1] Find the KPI grouo, given the path
                        string FullPath = reader["FullPath"].ToString();
                        string KpiGroupName = reader["KpiGroupName"].ToString();
                        string KpiName = reader["KpiName"].ToString();

                        Dictionary<string, List<string>> kpiGroupsList = null;

                        if (!retDict.TryGetValue(FullPath, out kpiGroupsList))
                        {
                            kpiGroupsList = new Dictionary<string, List<string>>();
                            retDict[FullPath] = kpiGroupsList;
                        }

                        // [2] Find the KPI given the KPI Groups list
                        List<string> kpisList = null;
                        if (!kpiGroupsList.TryGetValue(KpiGroupName, out kpisList))
                        {
                            kpisList = new List<string>();
                            kpiGroupsList[KpiGroupName] = kpisList;
                        }

                        // [3] If KPI is not yet in the list, add it
                        if (!kpisList.Contains(KpiName))
                        {
                            kpisList.Add(KpiName);
                        }
                    }
                }
            }

            return retDict;
        }

        public Dictionary<string, List<KpiInfo>> GetKpiMaster()
        {
            throw new NotImplementedException();
        }

        public List<TableProps> GetDimensions()
        {
            List<TableProps> retList = new List<TableProps>();

            using (SqlCommand command = new SqlCommand(@"dbo.GetTableProps", connection))
            {
                command.Parameters.AddWithValue("@TableName", "DimHierarchy");
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TableProps tableProps = new TableProps()
                        {
                            ColumnName = reader["ColumnName"].ToString(),
                            Datatype = reader["Datatype"].ToString(),
                            /* Remove unnecessary props
                            MaxLength = Convert.ToUInt16(reader["MaxLength"]),
                            Precision = Convert.ToUInt16(reader["Precision"]),
                            Scale = Convert.ToUInt16(reader["Scale"]),
                            Nullable = Convert.ToBoolean(reader["Nullable"]),
                            PrimaryKey = Convert.ToBoolean(reader["PrimaryKey"]),
                            */
                        };

                        retList.Add(tableProps);
                    }
                }
            }

            return retList;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            { //Close SQLAccessor to release resources.
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }

            disposed = true;
        }
    } //class AprSqlAccessor
}
