using System;

namespace MonkeyDb
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Table description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Do you get an automatic growth value (default is not opened)
        /// </summary>
        public bool IsGetIncrementValue { get; set; }

        public TableAttribute()
        {
            TableName = "";
            Description = "";
            IsGetIncrementValue = false;
        }
    }
}
