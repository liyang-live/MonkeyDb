using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Collections;
using System.Linq;
using MonkeyDb;

namespace MonkeyDb.SqlServer
{
   public class SqlServerBuilder:SqlBuilder
    {
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
        public override DbEntity QueryPageList<T>(string queryColumns,string whereSql, object whereParam, string sortCriteria, int pageSize, int pageIndex)
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
            int startNum = pageSize * (pageIndex - 1) + 1;
            int endNum = pageSize * pageIndex;
            string dbOperator = DbFactory.GetDbOperator();
            var pkColumn = attributeBuilder.GetPkColumnInfo(type);
            if (pkColumn == null)
            {
                return dbEntity;
            }
            List<TableColumnAttribute> whereColumns = attributeBuilder.GetColumnInfos(whereParam);
            var dbParams = new List<IDbDataParameter>();

            //分页查询模板
            StringBuilder sqlBuild = new StringBuilder("select {queryColumns} from {tableName} a,");
            sqlBuild.Append("(");
            sqlBuild.Append("select {pkColumn},row_number() over(order by {sortCriteria}) num from {tableName} {whereCriteria}");
            sqlBuild.Append(")");
            sqlBuild.Append(" b where a.{pkColumn}=b.{pkColumn} and b.num between {startNum} and {endNum} ");
            sqlBuild.Append("order by a.{sortCriteria};");
            sqlBuild.Replace("{tableName}", tableEntity.TableName);
            sqlBuild.Replace("{pkColumn}", pkColumn.ColumnName);
            sqlBuild.Replace("{sortCriteria}", sortCriteria);
            sqlBuild.Replace("{startNum}", startNum.ToString());
            sqlBuild.Replace("{endNum}", endNum.ToString());
            HandleQuerColumns(queryColumns,"a",ref sqlBuild,ref dbParams);
            HandleWhereParam(whereSql, whereColumns, ref sqlBuild, ref dbParams);
            dbEntity.CommandText = sqlBuild.ToString();
            dbEntity.DbParams = dbParams;
            return dbEntity;
        }

    }
}
