using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDb
{
   public abstract  class SqlDbFactory
    {
        /// <summary>
        /// MonkeyDb database type
        /// </summary>
        public virtual  MonkeyDbType DbType { get; set; }

        /// <summary>
        /// Creating a database connection object
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public abstract IDbConnection GetConnection(string connectionString);

        /// <summary>
        /// Create an adapter object
        /// </summary>
        /// <param name="dbCmd"></param>
        /// <returns></returns>
        public abstract IDataAdapter GetDataAdapter(IDbCommand dbCmd);

        /// <summary>
        /// Creating a single database parameter object
        /// </summary>
        /// <param name="tableColumn"></param>
        /// <returns></returns>
        public virtual IDbDataParameter GetDbParam(TableColumnAttribute tableColumn)
        {
            IDbDataParameter dbParam = null;
            string dbOperatore = GetDbOperator();
            if (tableColumn == null)
            {
                return dbParam;
            }
            dbParam = GetDbParam(tableColumn.ColumnName, tableColumn.ColumnValue, tableColumn.DataType, tableColumn.MaxLength);
            return dbParam;
        }

        /// <summary>
        /// Creating a single database parameter object
        /// </summary>
        /// <param name="dbParamName"></param>
        /// <param name="dbParamValue"></param>
        /// <param name="dbPramDbType"></param>
        /// <param name="dbPramLength"></param>
        /// <returns></returns>
        public abstract IDbDataParameter GetDbParam(string dbParamName,object dbParamValue,object dbPramDbType=null,int dbPramLength=0);

        /// <summary>
        /// Converting the objParams parameter to a database parameter list
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public virtual List<IDbDataParameter> GetDbParamList(object objParams)
        {
            List<IDbDataParameter> dbParams = null;
            if (objParams == null)
            {
                return dbParams;
            }
            var tableColumns = new AttributeBuilder().GetColumnInfos(objParams);
            if (tableColumns == null)
            {
                return dbParams;
            }
            if (tableColumns.Count == 0)
            {
                return dbParams;
            }
            dbParams = new List<IDbDataParameter>();
            foreach (TableColumnAttribute tableColumn in tableColumns)
            {
                var dbParam = GetDbParam(tableColumn);
                dbParams.Add(dbParam);
            }
            return dbParams;
        }

        /// <summary>
        /// Getting the database transaction object
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public virtual IDbTransaction GetDbTransaction(IDbConnection conn)
        {
            IDbTransaction dbTransaction = null;
            if (conn != null && conn.State == ConnectionState.Open)
            {
                dbTransaction = conn.BeginTransaction();
            }
            return dbTransaction;
        }

        /// <summary>
        /// Getting the database parameterized keyword operator
        /// </summary>
        /// <returns></returns>
        public virtual string GetDbOperator()
        {
            return "";
        }

        /// <summary>
        /// Get database automatic growth SQL
        /// </summary>
        /// <param name="isGetIncrementValue"></param>
        /// <returns></returns>
        public virtual string GetIncrementSql(bool isGetIncrementValue)
        {
            return "";
        }

    }
}
