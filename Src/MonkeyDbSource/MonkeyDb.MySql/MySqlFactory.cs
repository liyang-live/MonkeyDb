using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MonkeyDb.MySql
{
    public class MySqlFactory : SqlDbFactory
    {
        /// <summary>
        /// Creating a database connection object
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public override IDbConnection GetConnection(string connectionString)
        {
            IDbConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        /// <summary>
        ///Create an adapter object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public override IDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            IDataAdapter dataAdater = new MySqlDataAdapter((MySqlCommand)cmd);
            return dataAdater;
        }

        /// <summary>
        /// Creating a single database parameter object
        /// </summary>
        /// <param name="dbParamName"></param>
        /// <param name="dbParamValue"></param>
        /// <param name="dbPramDbType"></param>
        /// <param name="dbPramLength"></param>
        /// <returns></returns>
        public override IDbDataParameter GetDbParam(string dbParamName, object dbParamValue, object dbParamDbType = null, int dbParamLength = 0)
        {
            IDbDataParameter dbParam = null;
            if (string.IsNullOrEmpty(dbParamName))
            {
                return dbParam;
            }
            if (dbParamValue==null)
            {
                return dbParam;
            }
            dbParamName = GetDbOperator() + dbParamName;
            if (dbParamDbType == null)
            {
                dbParam = new MySqlParameter(dbParamName, dbParamValue);
                return dbParam;
            }
            if (dbParamLength <= 0)
            {
                dbParam = new MySqlParameter(dbParamName, dbParamValue);
                return dbParam;
            }
            int dbParamMaxLength = 0;
            int dbTypeValue = (int)dbParamDbType;
            switch (dbTypeValue)
            {
                case 0:
                case 15:
                case 246:
                case 253:
                case 254:
                case 600:
                case 601:
                    dbParamMaxLength = dbParamLength;
                    break;
            }
            dbParam = new MySqlParameter(dbParamName, (MySqlDbType)dbParamDbType, dbParamMaxLength);
            dbParam.Value = dbParamValue;
            return dbParam;
        }

        /// <summary>
        /// Getting the database parameterized keyword operator
        /// </summary>
        /// <returns></returns>
        public override string GetDbOperator()
        {
            return "?";
        }

        /// <summary>
        /// Get database automatic growth SQL
        /// </summary>
        /// <param name="isGetIncrementValue"></param>
        /// <returns></returns>
        public override string GetIncrementSql(bool isGetIncrementValue)
        {
            string sql = string.Empty;
            if (isGetIncrementValue)
            {
                sql = "select @@identity";
            }
            return sql;
        }
    }
}
