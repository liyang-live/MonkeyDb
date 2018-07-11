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
        /// 数据库连接对象
        /// </summary>
        private IDbConnection _dbConn = null;

        /// <summary>
        /// 数据库命令对象
        /// </summary>
        private IDbCommand _dbCmd = null;

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        private IDbTransaction _dbTransaction = null;

        #endregion

        #region public   Methods

        /// <summary>
        /// Open a transaction
        /// </summary>
        public virtual void BeginTransaction()
        {
            if (!IsStartTransaction)
            {
                CreateTransaction();
            }
        }

        /// <summary>
        /// Submission of transactions
        /// </summary>
        public void CommitTransaction()
        {
            if (IsStartTransaction)
            {
                if (_dbTransaction != null)
                {
                    _dbTransaction.Commit();
                    CloseTransaction();
                }
            }
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (IsStartTransaction)
            {
                if (_dbTransaction != null)
                {
                    _dbTransaction.Rollback();
                    CloseTransaction();
                }
            }
        }

        #endregion

        #region private Methods

        /// <summary>
        /// Perform data insertion, deletion and modification to return the number of rows affected.
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>int</returns>
        private int ExecuteNonQueryWithTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            int result = 0;
            if (IsStartTransaction)
            {
                PrepareCommand(cmdText, _dbConn, _dbCmd, dbParams, cmdType, _dbTransaction);
                ShowSqlToConsole(cmdText, dbParams);
                result = _dbCmd.ExecuteNonQuery();
                _dbCmd.Parameters.Clear();
            }
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to DataSet
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>DataSet</returns>
        private DataSet ExecuteQueryWithTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            DataSet result = null;
            if (IsStartTransaction)
            {
                PrepareCommand(cmdText, _dbConn, _dbCmd, dbParams, cmdType, _dbTransaction);
                ShowSqlToConsole(cmdText, dbParams);
                IDataAdapter da = DbFactory.GetDataAdapter(_dbCmd);
                da.Fill(result);
                _dbCmd.Parameters.Clear();
            }
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to IDataReader
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>IDataReader</returns>
        private IDataReader ExecuteReaderWithTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            IDataReader result = null;
            if (IsStartTransaction)
            {
                PrepareCommand(cmdText, _dbConn, _dbCmd, dbParams, cmdType, _dbTransaction);
                ShowSqlToConsole(cmdText, dbParams);
                result = _dbCmd.ExecuteReader(CommandBehavior.CloseConnection);
                _dbCmd.Parameters.Clear();
            }
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to object
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>object</returns>
        private object ExecuteScalarWithTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            object result = null;
            if (IsStartTransaction)
            {
                PrepareCommand(cmdText, _dbConn, _dbCmd, dbParams, cmdType, _dbTransaction);
                ShowSqlToConsole(cmdText, dbParams);
                result = _dbCmd.ExecuteScalar();
                _dbCmd.Parameters.Clear();
                if ((Object.Equals(result, null)) || (Object.Equals(result, System.DBNull.Value)))
                {
                    result = null;
                }
            }
            return result;
        }

        /// <summary>
        /// Create database transaction objects
        /// </summary>
        private void CreateTransaction()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return;
            }
            if (_dbConn == null)
            {
                _dbConn = DbFactory.GetConnection(ConnectionString);
                _dbConn.Open();
            }
            if (_dbCmd == null)
            {
                _dbCmd = _dbConn.CreateCommand();
            }
            if (_dbTransaction == null)
            {
                _dbTransaction = _dbConn.BeginTransaction();
            }
            IsStartTransaction = true;
        }

        /// <summary>
        /// Close the database transaction object
        /// </summary>
        /// </summary>
        private void CloseTransaction()
        {
            if (IsStartTransaction)
            {
                _dbCmd.Dispose();
                _dbTransaction.Dispose();
                _dbConn.Dispose();
                IsStartTransaction = false;
            }
        }

        #endregion
    }
}
