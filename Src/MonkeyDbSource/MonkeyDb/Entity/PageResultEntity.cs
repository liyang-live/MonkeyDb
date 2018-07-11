using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDb
{
    public class PageResultEntity
    {
        /// <summary>
        /// Number of display bars per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page a few pages
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Total number
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///PageCount
        /// </summary>
        public int TotalPageIndex { get; set; }

        /// <summary>
        /// Paging data
        /// </summary>
        public object Data{ get; set; }

        public PageResultEntity()
        {
            PageSize = 10;
            PageIndex = 1;
            TotalCount = 0;
            TotalPageIndex = 0;
            Data = null;
        }

    }
}
