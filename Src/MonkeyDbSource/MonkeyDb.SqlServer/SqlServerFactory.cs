using MonkeyDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MonkeyDb.SqlServer
{
    public class SqlServerFactory: SqlDbFactory
    {
        /// <summary>
        ///  Creating a database connection object
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public override IDbConnection GetConnection(string connectionString)
        {
            IDbConnection conn = new SqlConnection(connectionString);
            return conn;
        }

        /// <summary>
        ///Create an adapter object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public override IDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            IDataAdapter dataAdater = new SqlDataAdapter((SqlCommand)cmd);
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
            if (dbParamValue == null)
            {
                return dbParam;
            }
            dbParamName = GetDbOperator()+ dbParamName;
            if (dbParamDbType == null)
            {
                dbParam = new SqlParameter(dbParamName, dbParamValue);
                return dbParam;
            }
            if (dbParamLength <= 0)
            {
                dbParam = new SqlParameter(dbParamName, dbParamValue);
                return dbParam;
            }
            int dbParamMaxLength = 0;
            int dbTypeValue = (int)dbParamDbType;
            switch (dbTypeValue)
            {
                case 1:
                case 3:
                case 5:
                case 10:
                case 12:
                case 21:
                case 22:
                case 32:
                case 33:
                case 34:
                    dbParamMaxLength = dbParamLength;
                    break;
            }
            dbParam = new SqlParameter(dbParamName, (SqlDbType)dbParamDbType, dbParamMaxLength);
            dbParam.Value = dbParamValue;
            return dbParam;
        }

        /// <summary>
        /// Getting the database parameterized keyword operator
        /// </summary>
        /// <returns></returns>
        public override string GetDbOperator()
        {
            return "@";
        }

        /// <summary>
        ///Get database automatic growth SQL
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
