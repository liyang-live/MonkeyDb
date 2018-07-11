using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDb.Mapper;

namespace MonkeyDb
{
    public abstract class SqlDb
    {

        #region private protected public  variables

        /// <summary>
        /// Database string connection
        /// </summary>
        protected string _connectionString = string.Empty;

        /// <summary>
        /// Output SQL, and data parameters to the console
        /// </summary>
        protected bool _isShowSqlToConsole = false;

        ///// <summary>
        ///// Basics SqlBuilder
        ///// </summary>
        protected SqlBuilder SqlBuilder { get; set; }

        /// <summary>
        /// DbHelper
        /// </summary>
        protected DbUtility DbHelper { get; set; }

        /// <summary>
        /// Basics DbFactory
        /// </summary>
        public SqlDbFactory DbFactory { get; set; }

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
                if (!DbHelper.IsStartTransaction)
                {
                    _connectionString = value;
                    DbHelper.ConnectionString = _connectionString;
                }
            }
        }

        /// <summary>
        /// Output SQL, and data parameters to the console
        /// </summary>
        public bool IsShowSqlToConsole
        {
            get
            {
                return _isShowSqlToConsole;
            }
            set
            {
                _isShowSqlToConsole = value;
                DbHelper.IsShowSqlToConsole = _isShowSqlToConsole;
            }
        }

        #endregion

        #region public  Methods

        public SqlDb()
        {
            DbHelper = new DbUtility();
            DbHelper.ConnectionString = _connectionString;
            DbHelper.IsShowSqlToConsole = _isShowSqlToConsole;
        }

        /// <summary>
        /// Perform data insertion, deletion and modification to return the number of rows affected.
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>int</returns>
        public virtual int ExecuteNoneQuery(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            int result = 0;
            result = DbHelper.ExecuteNonQuery(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Perform data insertion, deletion and modification to return the number of rows affected.
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">objParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>int</returns>
        public virtual int ExecuteNoneQueryWithObjParam(string cmdText,object objParams = null, CommandType cmdType = CommandType.Text)
        {
            int result = 0;
            List<IDbDataParameter> dbParams = DbHelper.DbFactory.GetDbParamList(objParams);
            result = DbHelper.ExecuteNonQuery(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to DataSet
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteQuery(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            DataSet result = null;
            result = DbHelper.ExecuteQuery(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to DataSet
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="objParams">objParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        public virtual DataSet ExecuteQueryWithObjParam(string cmdText, object objParams = null, CommandType cmdType = CommandType.Text)
        {
            DataSet result = null;
            List<IDbDataParameter> dbParams = DbHelper.DbFactory.GetDbParamList(objParams);
            result = DbHelper.ExecuteQuery(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to IDataReader
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader ExecuteReader(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            IDataReader result = null;
            result = DbHelper.ExecuteReader(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to IDataReader
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="objParams">objParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader ExecuteReaderWithObjParam(string cmdText, object objParams = null, CommandType cmdType = CommandType.Text)
        {
            IDataReader result = null;
            List<IDbDataParameter> dbParams = DbHelper.DbFactory.GetDbParamList(objParams);
            result = DbHelper.ExecuteReader(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to object
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="dbParams">IDbDataParameter parameter list</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>object</returns>
        public virtual object ExecuteScalar(string cmdText, List<IDbDataParameter> dbParams = null, CommandType cmdType = CommandType.Text)
        {
            object result = null;
            result = DbHelper.ExecuteScalar(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Execute the query operation and return to object
        /// </summary>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="objParams">objParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <param name="cmdType">Command type:SQL statement/stored procedure</param>
        /// <returns>object</returns>
        public virtual object ExecuteScalarWithObjParam(string cmdText, object objParams = null, CommandType cmdType = CommandType.Text)
        {
            object result = null;
            List<IDbDataParameter> dbParams = DbHelper.DbFactory.GetDbParamList(objParams);
            result = DbHelper.ExecuteScalar(cmdText, dbParams, cmdType);
            return result;
        }

        /// <summary>
        /// Add single data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual object Insert<T>(T entity)
        {
            object result = default(object);
            var dbEntity = SqlBuilder.Insert<T>(entity);
            if (dbEntity == null)
            {
                return result;
            }
            result = dbEntity.TableEntity.IsGetIncrementValue ?
                DbHelper.ExecuteScalar(dbEntity.CommandText, dbEntity.DbParams) :
                DbHelper.ExecuteNonQuery(dbEntity.CommandText, dbEntity.DbParams);
            return result;
        }

        /// <summary>
        /// Delete the corresponding data from the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int DeleteById<T>(object id)
        {
            int result = 0;
            var dbEntity = SqlBuilder.DeleteById<T>(id);
            if (dbEntity == null)
            {
                return result;
            }
            result = DbHelper.ExecuteNonQuery(dbEntity.CommandText, dbEntity.DbParams);
            return result;
        }

        /// <summary>
        /// Deleting data according to filter conditions and filter parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams">whereObjParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <returns></returns>
        public virtual int Delete<T>(string whereSql, object whereObjParams)
        {
            int result = 0;
            var dbEntity = SqlBuilder.DeleteByParam<T>(whereSql, whereObjParams);
            if (dbEntity == null)
            {
                return result;
            }
            result = DbHelper.ExecuteNonQuery(dbEntity.CommandText, dbEntity.DbParams);
            return result;
        }

        /// <summary>
        ///  Modifying the corresponding data based on the modified parameters and primary key values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateParams">updateParams,for example:new {Uname="joyet"}</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int UpdateById<T>(object updateParams, object id)
        {
            int result = 0;
            var dbEntity = SqlBuilder.UpdateById<T>(updateParams, id);
            if (dbEntity == null)
            {
                return result;
            }
            result = DbHelper.ExecuteNonQuery(dbEntity.CommandText, dbEntity.DbParams);
            return result;
        }

        /// <summary>
        ///  Modifying data according to modifying parameters, filtering conditions and filtering condition parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateObjParams">updateObjParams,for example:new {Age=10,Uname="joyet"}</param>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams">whereObjParams,for example:new {Uid=1}</param>
        /// <returns></returns>
        public virtual int Update<T>(object updateObjParams, string whereSql, object whereObjParams)
        {
            int result = 0;
            var dbEntity = SqlBuilder.UpdateByParam<T>(updateObjParams, whereSql, whereObjParams);
            if (dbEntity == null)
            {
                return result;
            }
            result = DbHelper.ExecuteNonQuery(dbEntity.CommandText, dbEntity.DbParams);
            return result;
        }

        /// <summary>
        /// Querying the corresponding data according to the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T QueryById<T>(object id)
        {
            T result = default(T);
            var dbEntity = SqlBuilder.QueryById<T>(id);
            if (dbEntity == null)
            {
                return result;
            }
            using (var reader = DbHelper.ExecuteReader(dbEntity.CommandText, dbEntity.DbParams))
            {
                result = DataReaderToEntity<T>(reader);
            }
            return result;
        }

        /// <summary>
        /// Querying the corresponding data according to the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryColumns"></param>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams">whereObjParams,for example:new {Uid=1}</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string queryColumns, string whereSql, object whereObjParams)
        {
            List<T> result = null;
            var dbEntity = SqlBuilder.QueryByParam<T>(queryColumns, whereSql, whereObjParams);
            if (dbEntity == null)
            {
                return result;
            }
            using (var reader = DbHelper.ExecuteReader(dbEntity.CommandText, dbEntity.DbParams))
            {
                result = DataReaderToEntityList<T>(reader);
            }
            return result;
        }

        /// <summary>
        ///  Execution of query data based on SQL statements and parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText">SQL statement/stored procedure/parameterized SQL statement</param>
        /// <param name="objParams">objParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string cmdText, object objParams)
        {
            List<T> result = null;
            var dbEntity = SqlBuilder.Query<T>(cmdText, objParams);
            if (dbEntity == null)
            {
                return result;
            }
            using (var reader = DbHelper.ExecuteReader(dbEntity.CommandText, dbEntity.DbParams))
            {
                result = DataReaderToEntityList<T>(reader);
            }
            return result;
        }

        /// <summary>
        /// Query paging data based on query fields, filtering conditions, filtering parameters, sorting conditions, number of pages per page, and current page number.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="queryColumns"></param>
        /// <param name="whereSql"></param>
        /// <param name="sortCriteria"></param>
        /// <param name="whereObjParams">whereObjParams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <returns></returns>
        public virtual PageResultEntity QueryPageList<T>(int pageSize, int pageIndex, string queryColumns, string whereSql,string sortCriteria,object whereObjParams)
        {
            PageResultEntity pageResult = new PageResultEntity();
            pageResult.PageSize = pageSize;
            pageResult.PageIndex = pageIndex;
            var totalPageDbEntity = SqlBuilder.QueryTotalPageCount<T>(whereSql, whereObjParams);
            if (totalPageDbEntity == null)
            {
                return pageResult;
            }
            var objTotalCount = DbHelper.ExecuteScalar(totalPageDbEntity.CommandText, totalPageDbEntity.DbParams);
            if (objTotalCount == null)
            {
                return pageResult;
            }
            pageResult.TotalCount = Convert.ToInt32(objTotalCount);
            if (pageResult.TotalCount <= 0)
            {
                return pageResult;
            }
            var dbEntity = SqlBuilder.QueryPageList<T>(queryColumns, whereSql, whereObjParams, sortCriteria, pageSize, pageIndex);
            if (dbEntity == null)
            {
                return pageResult;
            }
            List<T> listData;
            using (var reader = DbHelper.ExecuteReader(dbEntity.CommandText, dbEntity.DbParams))
            {
                listData = DataReaderToEntityList<T>(reader);
            }
            pageResult.Data = listData;
            pageResult = SetPageListResult<T>(pageResult);
            return pageResult;
        }

        /// <summary>
        /// Open a transaction
        /// </summary>
        public virtual void BeginTransaction()
        {
            DbHelper.BeginTransaction();
        }

        /// <summary>
        /// Submission of transactions
        /// </summary>
        public virtual void CommitTransaction()
        {
            DbHelper.CommitTransaction();
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public virtual void RollbackTransaction()
        {
            DbHelper.RollbackTransaction();
        }

        /// <summary>
        /// IDataReader is transformed into entity
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual T DataReaderToEntity<T>(IDataReader reader)
        {
            T entity = default(T);
            List<T> entityList = DataReaderToEntityList<T>(reader);
            if (entityList == null)
            {
                return entity;
            }
            if (entityList.Count == 0)
            {
                return entity;
            }
            entity = entityList[0];
            return entity;
        }

        /// <summary>
        /// IDataReader is converted to a list of entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual List<T> DataReaderToEntityList<T>(IDataReader reader)
        {
            List<T> entityList = null;
            if (reader == null)
            {
                return entityList;
            }
            entityList = new List<T>();
            DataReaderMapper<T> readBuild = DataReaderMapper<T>.GetInstance(reader);
            while (reader.Read())
            {
                entityList.Add(readBuild.Map(reader));
            }
            return entityList;
        }

        /// <summary>
        /// DataTable is transformed into entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public virtual T DataTableToEntity<T>(DataTable dataTable)
        {
            T entity = default(T);
            List<T> entityList = DataTableToEntityList<T>(dataTable);
            if (entityList == null)
            {
                return entity;
            }
            if (entityList.Count == 0)
            {
                return entity;
            }
            entity = entityList[0];
            return entity;
        }

        /// <summary>
        ///  DataTable is converted to a list of entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public virtual List<T> DataTableToEntityList<T>(DataTable dt)
        {
            List<T> entityList = null;
            if (dt == null)
            {
                return entityList;
            }
            entityList = new List<T>();
            foreach (DataRow dataRow in dt.Rows)
            {
                DataTableMapper<T> dataRowMapper = DataTableMapper<T>.GetInstance(dataRow);
                var entity = dataRowMapper.Map(dataRow);
                if (entity != null)
                {
                    entityList.Add(dataRowMapper.Map(dataRow));
                }
            }
            return entityList;
        }

        /// <summary>
        /// Setting the paging results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public virtual PageResultEntity SetPageListResult<T>(PageResultEntity pageResult)
        {
            if (pageResult.Data == null)
            {
                return pageResult;
            }
            var dataList = pageResult.Data as List<T>;
            int totalPageIndex = 0;
            if (dataList.Count == 0)
            {
                totalPageIndex = 0;
                return pageResult;
            }
            else if (pageResult.TotalCount <= pageResult.PageSize)
            {
                totalPageIndex = 1;
            }
            else
            {
                if (pageResult.TotalCount % pageResult.PageSize == 0)
                {
                    totalPageIndex = pageResult.TotalCount / pageResult.PageSize;
                }
                else
                {
                    totalPageIndex = pageResult.TotalCount / pageResult.PageSize + 1;
                }
            }
            pageResult.TotalPageIndex = totalPageIndex;
            return pageResult;
        }

        #endregion
    }
}
