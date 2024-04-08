using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.DTOs.CustomerDTO
{
    public class CustomerImport : Customer
    {
        public Dictionary<string, string[]> ImportInvalidErrors { get; set; }
        public bool IsImport { get; set; }
        public CustomerImport()
        {
            ImportInvalidErrors = new Dictionary<string, string[]>();
        }
        
    }
}
