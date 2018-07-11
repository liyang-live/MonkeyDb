**MonkeyDb是一套基于ADO.Net和DoNetCore对MSSql、MySql、Oracle数据库支持的快速开发和轻量级ORM框架.此框架特点如下.**<br> 
(1)多数据库支持，目前支持MSSql、MySql，后续会支持Oracle、Access、pgsql、Sqlite等数据，进行接续更新。<br> 
(2)支持事务操作。<br> 
(3)对多数据库操作的ADO.Net和DoNetCore轻量级框架，支持.NET Framwork4.0以上和.net core2.0以上版本。<br> 
(4)半ORM框架，目前对单表CURD提供了支持,很方便使用。<br> 
(5) 对各数据库提供参数化，支持原生IDbDataParameter和类似于Dapper参数对象。<br> 
(6)Emit对象转换，通过Emit快速将DataTable、DataReader转化为实体对象。<br> 
(7)MonkeyDb作为数据库基础框架，方便对不同数据库扩展。<br> <br> 

[MonkeyDb测试Demo地址git：https://github.com/joyet/MonkeyDbTest.git](https://github.com/joyet/MonkeyDbTest.git)<br>

此框架对应nuget包已经上传，地址如下：<br> 
[SqlServe的nuget包地址:https://www.nuget.org/packages/MonkeyDb.SqlServer/](https://www.nuget.org/packages/MonkeyDb.SqlServer/)<br> 
[MySql的nuget包地址:https://www.nuget.org/packages/MonkeyDb.MySql/](https://www.nuget.org/packages/MonkeyDb.MySql/)<br> 


示例代码如下：<br> 
SqlServerTestApp.cs<br> 
using MonkeyDb;<br> 
using MonkeyDb.SqlServer;<br> 
using System;<br> 
using System.Collections.Generic;<br> 
using System.Data;<br> 
using System.Linq;<br> 
using System.Text;<br> 
using System.Threading.Tasks;<br> 

namespace SqlServerTestApp<br> 
{<br> 
   public class SqlServerDbTest<br> 
    {<br> 
        private string _connString;<br> 
        private bool _isShowSqlToConsole;<br> 

        public SqlServerDbTest(string connString, bool isShowSqlToConsole)
        {
            _connString = connString;
            _isShowSqlToConsole = isShowSqlToConsole;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        public void Insert()
        {
            //测试事务批量写入数据 
            SqlDb db = new SqlServerDb(_connString);
            db.BeginTransaction();
            db.IsShowSqlToConsole = _isShowSqlToConsole;
            for (int i = 1; i <= 15; i++)
            {
                db.Insert<UserInfo>(new UserInfo() { UserId = i, UserName = "joyet" + i.ToString(), Age = 110, Email = "joyet" + i.ToString() + "@qq.com" });
            }
            db.CommitTransaction();
        }

        public void DeleteAll()
        {
            SqlDb db = new SqlServerDb(_connString);
            var dbOperator = db.DbFactory.GetDbOperator();
            db.IsShowSqlToConsole = _isShowSqlToConsole;
            db.Delete<UserInfo>("", null);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public void Delete()
        {
            SqlDb db = new SqlServerDb(_connString);
            var dbOperator = db.DbFactory.GetDbOperator();
            db.IsShowSqlToConsole = _isShowSqlToConsole;

            //根据实体主键参数值查询数据
            db.DeleteById<UserInfo>(1);

            //根据过滤SQL、过滤参数查询数据列表1
            db.Delete<UserInfo>("UserId=3", null);

            //根据过滤SQL、过滤参数查询数据列表2
            string whereSql = string.Format("UserId={0}UserId", dbOperator);
            db.Delete<UserInfo>(whereSql, new { UserId = 5 });
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        public void Update()
        {
            SqlDb db = new SqlServerDb(_connString);
            var dbOperator = db.DbFactory.GetDbOperator();
            db.IsShowSqlToConsole = _isShowSqlToConsole;

            //根据修改字段参数及值、过滤SQL、过滤参数查询数据列表1
            db.Update<UserInfo>(new { UserName = "joyet22" }, "UserId=2", null);

            //根据查询字段、过滤SQL、过滤参数查询数据列表2
            string whereSql = string.Format("UserId={0}UserId", dbOperator);
            db.Update<UserInfo>(new { UserName = "joyet44" }, whereSql, new { UserId = 4 });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        public void Query()
        {
            SqlDb db = new SqlServerDb(_connString);
            var dbOperator = db.DbFactory.GetDbOperator();
            db.IsShowSqlToConsole = _isShowSqlToConsole;


            //根据实体主键参数值查询数据
            var model = db.QueryById<UserInfo>(2);

            //根据查询字段、过滤SQL、过滤参数查询数据列表1
            List<UserInfo> dataList1 = db.Query<UserInfo>("UserId,UserName", "UserId=4", null);

            //根据查询字段、过滤SQL、过滤参数查询数据列表2
            string whereSql2 = string.Format("UserId={0}UserId", dbOperator);
            List<UserInfo> dataList2 = db.Query<UserInfo>("*", whereSql2, new { UserId = 4 });

            //根据sql语句、过滤参数查询数据列表1
            List<UserInfo> dataList3 = db.Query<UserInfo>("select * from UserInfo where UserId=6", null);

            //根据sql语句、过滤参数查询数据列表2
            string sql4 = string.Format("select * from UserInfo where UserId={0}UserId", dbOperator);
            List<UserInfo> dataList4 = db.Query<UserInfo>(sql4, new { UserId = 6 });

            //分页列表数据查询1
            var pageResult1 = db.QueryPageList<UserInfo>(10, 1, "*", "UserId>1", "UserId asc", null);

            //分页列表数据查询2
            string whereSql = string.Format("UserId>{0}UserId", dbOperator);
            var pageResult2 = db.QueryPageList<UserInfo>(10, 1, "*", whereSql, "UserId asc", new { UserId = 1 });
        }

        /// <summary>
        /// ADO.NET其它用法1
        /// </summary>
        public void Orther1()
        {
            SqlDb db = new SqlServerDb(_connString);
            db.IsShowSqlToConsole = _isShowSqlToConsole;
            var dbOperator = db.DbFactory.GetDbOperator();
            var dbFactory = db.DbFactory;

            //执行增、删、改操作
            string sql1 = string.Format("insert into UserInfo(UserId,UserName,Age,Email) values({0}UserId,{0}UserName,{0}Age,{0}Email)", dbOperator);
            List<IDbDataParameter> dbParams1 = new List<IDbDataParameter>();
            dbParams1.Add(dbFactory.GetDbParam("UserId", 1));
            dbParams1.Add(dbFactory.GetDbParam("UserName", "joyet1"));
            dbParams1.Add(dbFactory.GetDbParam("Age", 100));
            dbParams1.Add(dbFactory.GetDbParam("Email", "joyet1@qq.com"));
            db.ExecuteNoneQuery(sql1, dbParams1);

            //查询单条数据
            string sql2 = "select * from UserInfo where UserId=?UserId";
            List<IDbDataParameter> dbParams2 = new List<IDbDataParameter>();
            dbParams2.Add(dbFactory.GetDbParam("UserId", 1));
            UserInfo data;
            using (var dataReader = db.ExecuteReader(sql2, dbParams2))
            {
                data = db.DataReaderToEntity<UserInfo>(dataReader);
            }

            //查询多条数据
            string sql3 = string.Format("select * from UserInfo where UserId>{0}UserId", dbOperator);
            List<IDbDataParameter> dbParams3 = new List<IDbDataParameter>();
            dbParams3.Add(dbFactory.GetDbParam("UserId", 0));
            List<UserInfo> dataList3;
            using (var dataReader = db.ExecuteReader(sql3, dbParams3))
            {
                dataList3 = db.DataReaderToEntityList<UserInfo>(dataReader);
            }
        }

        /// <summary>
        /// ADO.NET其它用法2,会自动new {UserId = 3} 转化成List<IDbDataParameter>
        /// </summary>
        public void Orther2()
        {
            SqlDb db = new SqlServerDb(_connString);
            db.IsShowSqlToConsole = _isShowSqlToConsole;
            var dbOperator = db.DbFactory.GetDbOperator();
            var dbFactory = db.DbFactory;

            //执行增、删、改操作
            string sql1 = string.Format("insert into UserInfo(UserId,UserName,Age,Email) values({0}UserId,{0}UserName,{0}Age,{0}Email)", dbOperator);
            var objParams = new { UserId = 3, UserName = "joyet3", Age = 40, Email = "joyet3@qq.com" };
            db.ExecuteNoneQueryWithObjParam(sql1, objParams);

            //查询单条数据，利用monkeydb自带的emit映射工具转化为实体
            string sql2 = string.Format("select * from UserInfo where UserId={0}UserId", dbOperator);
            var objParams2 = new { UserId = 3 };
            UserInfo data;
            using (var dataReader = db.ExecuteReaderWithObjParam(sql2, objParams2))
            {
                data = db.DataReaderToEntity<UserInfo>(dataReader);
            }
            
            //查询IDataReader多条数据,利用monkeydb自带的emit映射工具转化为实体列表
            string sql3 = string.Format("select * from UserInfo where UserId>{0}UserId", dbOperator);
            var objParams3 = new { UserId = 3 };
            List<UserInfo> dataList3;
            using (var dataReader = db.ExecuteReaderWithObjParam(sql3, objParams3))
            {
                dataList3 = db.DataReaderToEntityList<UserInfo>(dataReader);
            }

        }
    }
}

UserInfo.cs<br> 
using MonkeyDb;<br> 
using System;<br> 
using System.Collections.Generic;<br> 
using System.Linq;<br> 
using System.Text;<br> 
using System.Threading.Tasks;<br> 

namespace SqlServerTestApp<br> 
{<br> 
   public class UserInfo<br> 
    {<br> 
        [TableColumn(IsPrimaryKey = true)]<br> 
        public int UserId { get; set; }<br> 

        public string UserName { get; set; }<br> 

        public int Age { get; set; }

        public string Email { get; set; }
    }
}

Program.cs<br> 
using System;<br> 

namespace SqlServerTestApp<br> 
{<br> 
    class Program<br> 
    {<br> 
        static string connectString = "server=localhost;user id=sa;password=123456;database=testdb";<br> 
        static bool isShowSqlToConsole = true;<br> 

        static void Main(string[] args)
        {
            Test();
            Console.ReadLine();
        }

      
        public static void Test()
        {
            var dbTest = new SqlServerDbTest(connectString, isShowSqlToConsole);
            dbTest.DeleteAll();
            dbTest.Insert();
            dbTest.Delete();
            dbTest.Update();
            dbTest.Query();
            dbTest.Orther1();
            dbTest.Orther2();
        }
    }
}

