using System;
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
        private const string CONNECTION_STRING = @"server='{0}\ISAESQLSERVER';Initial Catalog=IsaeDw;user ID='IsaeDw';password='IsaeDw'";

        private SqlConnection connection;
        private bool disposed = false;

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

        public void GetConsolidatedHierarchy()
        {
            // maybe not needed for now
            throw new NotImplementedException();
        }

        public Dictionary<string, List<KpiInfo>> GetConsolidatedHierarchyKpi()
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
