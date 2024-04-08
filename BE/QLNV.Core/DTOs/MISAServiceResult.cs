using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.DTOs
{
    public class MISAServiceResult
    {
        #region field
        public HttpStatusCode status { get; set; }
        public string devMsg { get; set; }
        public string? userMsg { get; set; }
        public Dictionary<string, string[]>? errors { get; set; }
        public string? traceId { get; set; }
        public string? moreInfo { get; set; }
        #endregion

        #region Method
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
