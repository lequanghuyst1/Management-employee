using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Exceptions
{
    /// <summary>
    /// Class NotFoundException
    /// </summary>
    /// Created By: LQHUY(25/12/2023)
    public class NotFoundException : Exception
    {
        #region Field
        /// <summary>
        /// Mã lỗi
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        public HttpStatusCode status { get; set; }

        /// <summary>
        /// Danh sách lỗi 
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        public Dictionary<string, string[]>? errors { get; set; }
        #endregion

        #region Constructor
        public NotFoundException() { }
        public NotFoundException(HttpStatusCode status, Dictionary<string, string[]> errors)
        {
            this.status = status;
            this.errors = errors;
        }

        public NotFoundException(string message) : base(message)
        {

        }
        
        public NotFoundException(HttpStatusCode status, string message, Dictionary<string, string[]> errors) : base(message)
        {
            this.status = status;
            this.errors = errors;
        }
        #endregion
    }
}
