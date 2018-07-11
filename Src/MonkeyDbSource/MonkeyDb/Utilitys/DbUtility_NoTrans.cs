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
        #region private Methods

        /// <summary>
        /// Perform data insertion, deletion and modification to return the number of rows affected.
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>int</returns>
        private int ExecuteNonQueryNoTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            int result = 0;
            using (IDbConnection conn = DbFactory.GetConnection(_connectionString))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmdText, conn, cmd, dbParams, cmdType);
                    ShowSqlToConsole(cmdText, dbParams);
                    result = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
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
        private DataSet ExecuteQueryNoTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            DataSet result = null;
            using (IDbConnection conn = DbFactory.GetConnection(ConnectionString))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    result = new DataSet();
                    PrepareCommand(cmdText, conn, cmd, dbParams, cmdType);
                    ShowSqlToConsole(cmdText, dbParams);
                    IDataAdapter da = DbFactory.GetDataAdapter(cmd);
                    da.Fill(result);
                    cmd.Parameters.Clear();
                }
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
        private IDataReader ExecuteReaderNoTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            IDataReader result = null;
            IDbConnection conn = DbFactory.GetConnection(ConnectionString);
            IDbCommand cmd = conn.CreateCommand();
            PrepareCommand(cmdText, conn, cmd, dbParams, cmdType);
            ShowSqlToConsole(cmdText, dbParams);
            result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to object
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>object</returns>
        private object ExecuteScalarNoTrans(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            object result = null;
            using (IDbConnection conn = DbFactory.GetConnection(_connectionString))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    PrepareCommand(cmdText, conn, cmd, dbParams, cmdType);
                    ShowSqlToConsole(cmdText, dbParams);
                    result = cmd.ExecuteScalar();
                    if ((Object.Equals(result, null)) || (Object.Equals(result, System.DBNull.Value)))
                    {
                        result = null;
                    }
                    cmd.Parameters.Clear();
                }
            }
            return result;
        }

        #endregion
    }
}
