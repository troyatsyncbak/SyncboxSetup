using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Syncbak.SyncboxSetup.Data
{
    public class BaseDataMapper
    {
        private readonly SqlConnection sqlDb;

        public BaseDataMapper(string dbName)
        {
            sqlDb = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbName].ConnectionString);
        }

        protected void ExecuteDataReader(string procName, List<SqlParameter> parameters, Action<IDataReader> action)
        {
            sqlDb.Open();

            var cmd = GetStoredProcCommand(procName, parameters);

            cmd.CommandTimeout = 10;
                        
            using (cmd)
            {
                using (var reader = cmd.ExecuteReader())
                {
                    action(reader);
                }
            }
            sqlDb.Close();
        }

        protected void ExecuteNonQuery(string procName, List<SqlParameter> parameters)
        {
            sqlDb.Open();

            var cmd = GetStoredProcCommand(procName, parameters);

            using (cmd)
            {
                cmd.ExecuteNonQuery();
            }
            sqlDb.Close();
        }

        private SqlCommand GetStoredProcCommand(string procName, IEnumerable<SqlParameter> parameters)
        {
            var cmd = new SqlCommand(procName, sqlDb);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (var param in parameters.Where(param => param.Value != null && param.SqlValue != DBNull.Value))
                {
                    cmd.Parameters.Add(param);
                }
            }
            return cmd;
        }

        public T ParseValue<T>(IDataReader reader, string columnName)
        {
            var result = default(T);

            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                result = (T)reader.GetValue(reader.GetOrdinal(columnName));

            return result;
        }
    }
}