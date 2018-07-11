using MonkeyDb;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MonkeyDb.SqlServer
{
    public class SqlServerDb:SqlDb
    {
       
        public SqlServerDb(string connectionString="")
        {
            _connectionString = connectionString;
            DbFactory = new SqlServerFactory()
            {
                DbType = MonkeyDbType.SqlServer
            };
            SqlBuilder = new SqlServerBuilder();
            SqlBuilder.DbFactory = DbFactory;
            DbHelper.DbFactory = DbFactory;
            DbHelper.ConnectionString = _connectionString;

        }

    }
}
