using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Exceptions
{
    public class ImportException : Exception
    {
        #region Properties
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
        public ImportException() { }
        public ImportException(Dictionary<string, string[]> errors, HttpStatusCode status)
        {
            this.errors = errors;
            this.status = status;
        }

        public ImportException(string message) : base(message)
        {

        }

        public ImportException(HttpStatusCode status, string message, Dictionary<string, string[]> errors) : base(message)
        {
            this.status = status;
            this.errors = errors;
        }
        #endregion
    }
}
