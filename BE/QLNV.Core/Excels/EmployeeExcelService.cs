using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Excels;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Excels
{
    public class EmployeeExcelService : BaseExcelService<Employee>, IEmployeeExcelService
    {
        public EmployeeExcelService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
        }
    }
}
