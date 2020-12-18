using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Base
{
    public class BaseParam
    {
        public EtsPaging PagingInfo { get; set; }
        public string UserName { get; set; }
    }
    public class EtsPaging
    {
        /// <summary>
        /// Get, Set Record count
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// Get, Set Page number
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Get Page size - Constant
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Get Number of rows to skip
        /// </summary>
        public int RowsSkip
        {
            get { return (PageIndex - 1) * PageSize; }
        }
        public int RowStart { get; set; }

        /// <summary>
        /// Creates a new PagingInfo instance
        /// </summary>
        public EtsPaging()
        {
            PageIndex = 0;
            PageSize = 0;
        }

        public EtsPaging(int pageIndex)
        {
            PageIndex = pageIndex;
        }
        /// <summary>
        /// Create a new PagingInfo instance with page number
        /// </summary>
        /// <param name="pageIndex"></param>
        public EtsPaging(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string OrderField { get; set; }
        public string OrderDir { get; set; }
    }
}
