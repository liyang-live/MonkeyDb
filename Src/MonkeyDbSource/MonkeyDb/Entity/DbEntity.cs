using System;
using System.Collections.Generic;
using System.Data;

namespace MonkeyDb
{
   
    public class DbEntity
    {
        /// <summary>
        /// MonkeyDb database type
        /// </summary>
        public MonkeyDbType DbType { get; set; }

        /// <summary>
        ///SQL statement/stored procedure/parameterized SQL statement
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        ///Database parameter list
        /// </summary>
        public List<IDbDataParameter> DbParams { get; set; }

        /// <summary>
        ///Data table entity 
        /// </summary>
        public TableAttribute TableEntity { get; set; }

        public DbEntity()
        {
            DbType = MonkeyDbType.SqlServer;
            CommandText = string.Empty;
            DbParams = new List<IDbDataParameter>();
            TableEntity = new TableAttribute();
        }
    }

    /// <summary>
    /// MonkeyDb database type enumeration
    /// </summary>
    public enum MonkeyDbType
    {
        SqlServer = 1,
        MySql = 2,
        Oracle = 3,
        Access = 4
    }
}
