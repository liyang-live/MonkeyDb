using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonkeyDb
{
    public class AttributeBuilder
    {
        /// <summary>
        /// Get table information
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TableAttribute GetTableInfo(Type type)
        {
            TableAttribute tableEntity = new TableAttribute();
            tableEntity.TableName = type.Name;
            object[] customAttributes = type.GetCustomAttributes(typeof(TableAttribute), true);
            if (customAttributes == null)
            {
                return tableEntity;
            }
            if (customAttributes.Length <= 0)
            {
                return tableEntity;
            }
            tableEntity = customAttributes[0] as TableAttribute;
            return tableEntity;
        }

        /// <summary>
        /// Get table Field list information based on entity type and entity object instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<TableColumnAttribute> GetColumnInfos(Type type,object entity)
        {
           List<TableColumnAttribute> columnEntityList = null;
           PropertyInfo[] propertys = type.GetProperties();
            if (propertys == null)
            {
                return columnEntityList;
            }
            if (propertys.Length <= 0)
            {
                return columnEntityList;
            }
            columnEntityList = new List<TableColumnAttribute>();
            foreach (PropertyInfo proInfo in propertys)
            {
                TableColumnAttribute systemColumn = GetProperInfo(proInfo, entity);
                TableColumnAttribute selftDefineColumn =GetCustomAttributeInfo(proInfo, entity);
                if (selftDefineColumn == null)
                {
                    columnEntityList.Add(systemColumn);
                    continue;
                }
                columnEntityList.Add(selftDefineColumn);
            }
            return columnEntityList;
       }

        /// <summary>
        /// Get the list of table fields based on the object parameter
        /// </summary>
        /// <param name="objPparams">objPparams,for example:new {Uid=1,Uname="joyet"}</param>
        /// <returns></returns>
        public List<TableColumnAttribute> GetColumnInfos(object objPparams)
        {
            List<TableColumnAttribute> columnEntityList = null;
            if (objPparams == null)
            {
                return columnEntityList;
            }
            Type type = objPparams.GetType();
            PropertyInfo[] props = type.GetProperties();
            if (props == null)
            {
                return columnEntityList;
            }
            if (props.Length == 0)
            {
                return columnEntityList;
            }
            columnEntityList = new List<TableColumnAttribute>();
            foreach (PropertyInfo proInfo in props)
            {
                TableColumnAttribute systemColumn = GetProperInfo(proInfo, objPparams);
                columnEntityList.Add(systemColumn);
            }
            return columnEntityList;
        }

        /// <summary>
        /// Get the table primary key field information
        /// </summary>
        /// <returns></returns>
        public TableColumnAttribute GetPkColumnInfo(Type type)
        {
            TableColumnAttribute pkColumn = null;
            PropertyInfo[] props = type.GetProperties();
            if (props == null)
            {
                return pkColumn;
            }
            if (props.Length == 0)
            {
                return pkColumn;
            }
            foreach (PropertyInfo proInfo in props)
            {
                TableColumnAttribute systemColumn = GetProperInfo(proInfo, null);
                if (systemColumn == null)
                {
                    return pkColumn;
                }
                TableColumnAttribute selftDefineColumn = GetCustomAttributeInfo(proInfo, null);
                if (selftDefineColumn == null)
                {
                    return pkColumn;
                }
                if (selftDefineColumn.IsPrimaryKey)
                {
                    pkColumn = selftDefineColumn;
                    break;
                }
            }
            return pkColumn;
        }

        /// <summary>
        /// Get a single attribute information
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private TableColumnAttribute GetProperInfo(PropertyInfo proInfo, object entity)
       {
            TableColumnAttribute columnEntity = null;
            if (proInfo == null)
            {
                return columnEntity;
            }
            columnEntity = new TableColumnAttribute()
            {
                ColumnName = proInfo.Name
            };
            if (entity != null)
            {
                object attrValue = proInfo.GetValue(entity, null);
                if (attrValue != null)
                {
                    columnEntity.ColumnValue = attrValue;
                }
            }
            return columnEntity;
       }

        /// <summary>
        /// Get the custom feature information of a single attribute
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private TableColumnAttribute GetCustomAttributeInfo(PropertyInfo proInfo, object param)
       {
           TableColumnAttribute columnEntity = null;
           if (proInfo == null)
           {
                return columnEntity;
            }
            object[] customAttributes = proInfo.GetCustomAttributes(typeof(TableColumnAttribute), true);
            if (customAttributes == null)
            {
                return columnEntity;
            }
            if (customAttributes.Length<=0)
            {
                return columnEntity;
            }
            columnEntity = (TableColumnAttribute)customAttributes[0];
            if (columnEntity == null)
            {
                return columnEntity;
            }
            columnEntity.ColumnName = string.IsNullOrEmpty(columnEntity.ColumnName) ? proInfo.Name : columnEntity.ColumnName;
            if (param != null)
            {
                object attrValue = proInfo.GetValue(param, null);
                if (attrValue != null)
                {
                    columnEntity.ColumnValue = attrValue;
                }
            }
            return columnEntity;
       }

    }
}
