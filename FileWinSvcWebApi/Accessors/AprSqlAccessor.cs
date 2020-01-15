using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using FileWinSvcWebApi.Models;

namespace Accessors
{
    public class AprSqlAccessor : IDisposable
    {
        private const string CONNECTION_STRING = @"server='{0}';Initial Catalog=ISAE_APR;user ID='IsaeApr';password='IsaeApr'";

        private SqlConnection connection;
        private bool disposed = false;

        public AprSqlAccessor(string serverName)
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

        internal void SaveLayout(LayoutData layout)
        {
            using (SqlCommand command = new SqlCommand("UPDATE Layouts set LayoutJson=@LayoutJson, LastUpdateDate=@LastUpdateDate WHERE Name=@Name;" +
                "IF @@ROWCOUNT=0 INSERT INTO Layouts(Name, LayoutJson, LastUpdateDate) VALUES (@Name, @LayoutJson, @LastUpdateDate)", connection))
            {
                command.Parameters.AddWithValue("@Name", layout.Name);
                command.Parameters.AddWithValue("@LayoutJson", layout.LayoutJson);
                command.Parameters.AddWithValue("@LastUpdateDate", DateTime.Now);
                command.ExecuteNonQuery();
            }
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
