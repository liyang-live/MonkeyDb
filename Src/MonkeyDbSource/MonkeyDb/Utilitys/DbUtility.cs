using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDb
{
    public partial class DbUtility
    {
        #region private、internal、public  variables

        /// <summary>
        /// Database string connection
        /// </summary>
        private string _connectionString { get; set; }

        /// <summary>
        /// Whether or not to open a transaction
        /// </summary>
        internal bool IsStartTransaction { get; set; }

        /// <summary>
        /// Database string connection
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if (!IsStartTransaction)
                {
                    _connectionString = value;
                }
            }
        }

        /// <summary>
        /// Database factory
        /// </summary>
        public SqlDbFactory DbFactory { get; set; }

        /// <summary>
        /// Output SQL, and data parameters to the console
        /// </summary>
        public bool IsShowSqlToConsole { get; set; }

        #endregion

        #region private、internal、public   Methods

        public DbUtility()
        {
            _connectionString = string.Empty;
            DbFactory = null;
            IsShowSqlToConsole = false;
            IsStartTransaction = false;
        }

        /// <summary>
        /// Perform data insertion, deletion and modification to return the number of rows affected.
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            var result = 0;
            if (!IsStartTransaction)
            {
                result = ExecuteNonQueryNoTrans(cmdText, dbParams, cmdType);
                return result;
            }
            result = ExecuteNonQueryWithTrans(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to DataSet
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteQuery(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            DataSet result = null;
            if (!IsStartTransaction)
            {
                result = ExecuteQueryNoTrans(cmdText, dbParams, cmdType);
                return result;
            }
            result = ExecuteQueryWithTrans(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to IDataReader
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            IDataReader result = null;
            if (!IsStartTransaction)
            {
                result = ExecuteReaderNoTrans(cmdText, dbParams, cmdType);
                return result;
            }
            result = ExecuteReaderWithTrans(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to object
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            object result = null;
            if (!IsStartTransaction)
            {
                result = ExecuteScalarNoTrans(cmdText, dbParams, cmdType);
                return result;
            }
            result = ExecuteScalarWithTrans(cmdText, dbParams, cmdType);
            return result;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Execution order preparation
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbConn">Database connection object</param>
        /// <param name="dbCmd">Database execution command</param>
        /// <param name="dbParams">Database parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <param name="dbTransaction">Database transaction object</param>
        private void PrepareCommand(string cmdText, IDbConnection dbConn, IDbCommand dbCmd, List<IDbDataParameter> dbParams, CommandType cmdType, IDbTransaction dbTransaction = null)
        {
            if (dbConn.State != ConnectionState.Open)
            {
                dbConn.Open();
            }
            dbCmd.Connection = dbConn;
            dbCmd.CommandText = cmdText;
            if (dbTransaction != null)
            {
                dbCmd.Transaction = dbTransaction;
            }
            dbCmd.CommandType = cmdType;
            if (dbParams != null)
            {
                foreach (IDbDataParameter parameter in dbParams)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    dbCmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Output SQL, and data parameters to the console
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="dbParams"></param>
        private void ShowSqlToConsole(string cmdText, List<IDbDataParameter> dbParams)
        {
            if (IsShowSqlToConsole)
            {
                Console.WriteLine("Sql:" + cmdText);
                if (dbParams != null)
                {
                    foreach (IDbDataParameter dbParam in dbParams)
                    {
                        Console.WriteLine("dbParamName:" + dbParam.ParameterName + ",dbParamValue:" + dbParam.Value.ToString());
                    }
                    Console.WriteLine("\r\n");
                }
            }

        }

        #endregion
    }
}
