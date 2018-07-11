using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyDb.MySql
{
   public class MySqlDb : SqlDb
    {

        public MySqlDb(string connectionString = "")
        {
            _connectionString = connectionString;
            DbFactory = new MySqlFactory()
            {
                DbType = MonkeyDbType.MySql
            };
            SqlBuilder = new MySqlBuilder();
            SqlBuilder.DbFactory = DbFactory;
            DbHelper.DbFactory = DbFactory;
            DbHelper.ConnectionString = _connectionString;

        }

    }
}
