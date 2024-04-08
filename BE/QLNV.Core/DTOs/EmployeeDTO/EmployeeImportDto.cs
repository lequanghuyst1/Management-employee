using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.DTOs.EmployeeDTO
{
    public class EmployeeImportDto : Employee
    {
        public Dictionary<string, string[]> ImportInvalidErrors { get; set; }
        public bool IsImport { get; set; }
        public EmployeeImportDto()
        {
            ImportInvalidErrors = new Dictionary<string, string[]>();
        }
    }
}
