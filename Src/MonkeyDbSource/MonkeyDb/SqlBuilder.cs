using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDb
{
   public abstract class SqlBuilder
    {
        /// <summary>
        /// Database factory
        /// </summary>
        public virtual SqlDbFactory DbFactory { get; set; }

        /// <summary>
        /// Add single data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual DbEntity Insert<T>(T entity)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            if (entity == null)
            {
                return dbEntity;
            }
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            if (tableEntity == null)
            {
                return dbEntity;
            }
            List<TableColumnAttribute> columns = attributeBuilder.GetColumnInfos(type, entity);
            if (columns == null)
            {
                return dbEntity;
            }
            if (columns.Count == 0)
            {
                return dbEntity;
            }
            columns = columns.Where(a => !a.IsAutoIncrement).ToList();
            if (columns == null)
            {
                return dbEntity;
            }
            if (columns.Count == 0)
            {
                return dbEntity;
            }
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            List<IDbDataParameter> dbParams = new List<IDbDataParameter>();
            string dbOperatore = DbFactory.GetDbOperator();
            StringBuilder sqlBuild = new StringBuilder("insert into {tableName}({columnNames}) values({columnValues})");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            StringBuilder columnNameSql = new StringBuilder();
            StringBuilder columnValueSql = new StringBuilder();
            int i = 0;
            foreach (var column in columns)
            {
                columnNameSql.Append(column.ColumnName);
                columnValueSql.Append(dbOperatore + column.ColumnName);
                if (i != columns.Count - 1)
                {
                    columnNameSql.Append(",");
                    columnValueSql.Append(",");
                }
                var dbParam = DbFactory.GetDbParam(column);
                dbParams.Add(dbParam);
                i++;
            }
            sqlBuild.Replace("{columnNames}", columnNameSql.ToString());
            sqlBuild.Replace("{columnValues}", columnValueSql.ToString());
            var autoIncrementSql = DbFactory.GetIncrementSql(tableEntity.IsGetIncrementValue);
            sqlBuild.Append(autoIncrementSql);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        /// Delete the corresponding data from the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual DbEntity DeleteById<T>(object id)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            if (tableEntity == null)
            {
                return dbEntity;
            }
            var column = attributeBuilder.GetPkColumnInfo(type);
            if (column == null)
            {
                return dbEntity;
            }
            column.ColumnValue = id;
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            string dbOperator = DbFactory.GetDbOperator();
            var dbParams = new List<IDbDataParameter>();
            StringBuilder sqlBuild = new StringBuilder("delete from {tableName} where {columnName}={dbOperator}{columnName}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            sqlBuild.Replace("{columnName}", column.ColumnName);
            sqlBuild.Replace("{dbOperator}", dbOperator);
            var dbParam = DbFactory.GetDbParam(column);
            dbParams.Add(dbParam);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        /// Deleting data according to filter conditions and filter parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="whereSql"></typeparam>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public virtual DbEntity DeleteByParam<T>(string whereSql,object whereObjParams)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            if (tableEntity == null)
            {
                return dbEntity;
            }
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            string dbOperatore = DbFactory.GetDbOperator();
            var dbParams = new List<IDbDataParameter>();
            StringBuilder sqlBuild = new StringBuilder("delete from {tableName} {whereCriteria}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            List<TableColumnAttribute> whereColumns = attributeBuilder.GetColumnInfos(whereObjParams);
            HandleWhereParam(whereSql,whereColumns, ref sqlBuild, ref dbParams);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        ///  Modifying the corresponding data based on the modified parameters and primary key values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateObjParams"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual DbEntity UpdateById<T>(object updateObjParams, object id)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            if (updateObjParams == null)
            {
                return dbEntity;
            }
            if (id == null)
            {
                return dbEntity;
            }
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            if (tableEntity == null)
            {
                return dbEntity;
            }
            List<TableColumnAttribute> updateColumns = attributeBuilder.GetColumnInfos(updateObjParams);
            if (updateColumns == null)
            {
                return dbEntity;
            }
            var pkColumn = attributeBuilder.GetPkColumnInfo(type);
            if (pkColumn == null)
            {
                return dbEntity;
            }
            pkColumn.ColumnValue = id;
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            string dbOperator = DbFactory.GetDbOperator();
            var dbParams = new List<IDbDataParameter>();
            StringBuilder sqlBuild = new StringBuilder("update {tableName} set {updateCriteria}  where {columnName}={dbOperator}{columnName}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            sqlBuild.Replace("{columnName}", pkColumn.ColumnName);
            sqlBuild.Replace("{dbOperator}", dbOperator);
            HandleUpdateParam(updateColumns, ref sqlBuild, ref dbParams);
            var dbParam = DbFactory.GetDbParam(pkColumn);
            dbParams.Add(dbParam);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        ///  Modifying data according to modifying parameters, filtering conditions and filtering condition parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateObjParams"></param>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams"></param>
        /// <returns></returns>
        public virtual DbEntity UpdateByParam<T>(object updateObjParams, string whereSql, object whereObjParams)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            if (updateObjParams == null)
            {
                return dbEntity;
            }
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            List<TableColumnAttribute> updateColumns= attributeBuilder.GetColumnInfos(updateObjParams);
            if (updateColumns == null)
            {
                return dbEntity;
            }
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            string dbOperator = DbFactory.GetDbOperator();
            var dbParams = new List<IDbDataParameter>();
            StringBuilder sqlBuild = new StringBuilder("update {tableName} set {updateCriteria} {whereCriteria}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            HandleUpdateParam(updateColumns, ref sqlBuild, ref dbParams);
            List<TableColumnAttribute> whereColumns = attributeBuilder.GetColumnInfos(whereObjParams);
            HandleWhereParam(whereSql, whereColumns, ref sqlBuild, ref dbParams);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        ///Querying the corresponding data according to the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual DbEntity QueryById<T>(object id)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            var pkColumn= attributeBuilder.GetPkColumnInfo(type);
            if (pkColumn == null)
            {
                return dbEntity;
            }
            pkColumn.ColumnValue = id;
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            string dbOperator = DbFactory.GetDbOperator();
            List<IDbDataParameter> dbParams = new List<IDbDataParameter>();
            StringBuilder sqlBuild = new StringBuilder("select * from {tableName} where {columnName}={dbOperator}{columnName}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            sqlBuild.Replace("{columnName}", pkColumn.ColumnName);
            sqlBuild.Replace("{dbOperator}", dbOperator);
            var dbParam = DbFactory.GetDbParam(pkColumn);
            dbParams.Add(dbParam);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        /// Querying the corresponding data according to the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryColumns"></param>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams"></param>
        /// <returns></returns>
        public virtual DbEntity QueryByParam<T>(string queryColumns, string whereSql, object whereObjParams)
        {
            DbEntity dbEntity = null;
            if (string.IsNullOrEmpty(queryColumns))
            {
                return dbEntity;
            }
            Type type = typeof(T);
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            List<TableColumnAttribute> whereColumns = attributeBuilder.GetColumnInfos(whereObjParams);
            string dbOperator = DbFactory.GetDbOperator();
            var dbParams = new List<IDbDataParameter>();
            StringBuilder sqlBuild = new StringBuilder("select {queryColumns} from {tableName} {whereCriteria}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            HandleQuerColumns(queryColumns,"",ref sqlBuild,ref dbParams);
            HandleWhereParam(whereSql,whereColumns,ref sqlBuild,ref dbParams);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        /// Query paging data based on query fields, filtering conditions, filtering parameters, sorting conditions, number of pages per page, and current page number.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryColumns"></param>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams"></param>
        /// <param name="sortCriteria"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public abstract DbEntity QueryPageList<T>(string queryColumns,string whereSql, object whereObjParams, string sortCriteria, int pageSize, int pageIndex);

        /// <summary>
        /// Query the total number of data according to the filtration condition and filtration condition parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSql"></param>
        /// <param name="whereObjParams"></param>
        /// <returns></returns>
        public virtual DbEntity QueryTotalPageCount<T>(string whereSql,object whereObjParams)
        {
            DbEntity dbEntity = null;
            Type type = typeof(T);
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            if (tableEntity == null)
            {
                return dbEntity;
            }
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity
            };
            var dbParams = new List<IDbDataParameter>();
            string dbOperator = DbFactory.GetDbOperator();
            List<TableColumnAttribute> whereColumns = attributeBuilder.GetColumnInfos(whereObjParams);
            StringBuilder sqlBuild = new StringBuilder("select count(*) from  {tableName} {whereCriteria}");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            HandleWhereParam(whereSql,whereColumns, ref sqlBuild, ref dbParams);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

        /// <summary>
        /// Execution of query data based on SQL statements and parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdText"></param>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public virtual DbEntity Query<T>(string cmdText, object objParams)
        {
            DbEntity dbEntity = null;
            if (string.IsNullOrEmpty(cmdText))
            {
                return dbEntity;
            }
            Type type = typeof(T);
            var attributeBuilder = new AttributeBuilder();
            var tableEntity = attributeBuilder.GetTableInfo(type);
            if (tableEntity == null)
            {
                return dbEntity;
            }
            dbEntity = new DbEntity()
            {
                DbType = DbFactory.DbType,
                TableEntity = tableEntity,
                CommandText = cmdText
            };
            dbEntity.DbParams = DbFactory.GetDbParamList(objParams);
            return dbEntity;
        }

        /// <summary>
        ///  Processing query fields
        /// </summary>
        /// <param name="queryColumns"></param>
        /// <param name="tableAlias"></param>
        /// <param name="sqlBuild"></param>
        /// <param name="dbParams"></param>
        public void HandleQuerColumns(string queryColumns,string tableAlias, ref StringBuilder sqlBuild, ref List<IDbDataParameter> dbParams)
        {
            if (!string.IsNullOrEmpty(queryColumns))
            {
                var queryColumnBuilder = new StringBuilder();
                if (queryColumns.IndexOf(',') > 0)
                {
                    var columnNames = queryColumns.Split(',');
                    var columnNameList = (from a in columnNames where a != "" select a).ToList();
                    int i = 0;
                    foreach (var columnName in columnNameList)
                    {
                        var tmpColumnName = string.IsNullOrEmpty(tableAlias) ? columnName : tableAlias + "." + columnName;
                        queryColumnBuilder.Append(tmpColumnName);
                        if (i != columnNameList.Count-1)
                        {
                            queryColumnBuilder.Append(",");
                        }
                        i++;
                    }
                }
                else
                {
                    var tmpColumnName = string.IsNullOrEmpty(tableAlias) ? queryColumns : tableAlias + "." + queryColumns;
                    queryColumnBuilder.Append(tmpColumnName);
                }
                sqlBuild.Replace("{queryColumns}", queryColumnBuilder.ToString());
            }
            else
            {
                sqlBuild.Replace("{queryColumns}", "*");
            }
           
        }

        /// <summary>
        /// Processing changes to Sql and field parameters
        /// </summary>
        /// <param name="updateColumns"></param>
        /// <param name="sqlBuild"></param>
        /// <param name="dbParams"></param>
        public void HandleUpdateParam(List<TableColumnAttribute> updateColumns, ref StringBuilder sqlBuild, ref List<IDbDataParameter> dbParams)
        {
            if (updateColumns == null)
            {
                sqlBuild.Replace("{updateCriteria}", "");
            }
            else
            {
                int i = 0;
                var dbOperator = DbFactory.GetDbOperator();
                StringBuilder updateSqlBuild = new StringBuilder();
                foreach (var updateColumn in updateColumns)
                {
                    updateSqlBuild.Append("{columnName}={dbOperator}{columnName}");
                    updateSqlBuild.Replace("{columnName}", updateColumn.ColumnName);
                    updateSqlBuild.Replace("{dbOperator}", dbOperator);
                    if (i != updateColumns.Count - 1)
                    {
                        updateSqlBuild.Append(",");
                    }
                    var dbParam = DbFactory.GetDbParam(updateColumn);
                    dbParams.Add(dbParam);
                    i++;
                }
                sqlBuild.Replace("{updateCriteria}", updateSqlBuild.ToString());
            }
        }

        /// <summary>
        /// Treatment of filtration conditions and parameters
        /// </summary>
        /// <param name="whereSql"></param>
        /// <param name="whereColumns"></param>
        /// <param name="sqlBuild"></param>
        /// <param name="dbParams"></param>
        public void HandleWhereParam(string whereSql,List<TableColumnAttribute> whereColumns, ref StringBuilder sqlBuild, ref List<IDbDataParameter> dbParams)
        {
            if (!string.IsNullOrEmpty(whereSql))
            {
                if (whereColumns != null)
                {
                    foreach (var whereColumn in whereColumns)
                    {
                        var dbParam = DbFactory.GetDbParam(whereColumn);
                        dbParams.Add(dbParam);
                    }
                }
                sqlBuild.Replace("{whereCriteria}", "where " + whereSql);
            }
            else
            {
                sqlBuild.Replace("{whereCriteria}", "");
            }
        }

    }
}
