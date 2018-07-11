using System;

namespace MonkeyDb
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TableColumnAttribute : Attribute
    {
        /// <summary>
        /// Field name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        public object ColumnValue { get; set; }

        /// <summary>
        /// Field corresponding to database type
        /// </summary>
        public object DataType { get; set; }

        /// <summary>
        /// Field maximum length
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Whether it is the primary key (default is not)
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Whether this field is automatically increased (default is not)
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        public TableColumnAttribute()
        {
            ColumnName = string.Empty;
            ColumnValue = DBNull.Value;
            DataType = null;
            MaxLength = 0;
            IsPrimaryKey = false;
            IsAutoIncrement = false;
        }
    }
}
