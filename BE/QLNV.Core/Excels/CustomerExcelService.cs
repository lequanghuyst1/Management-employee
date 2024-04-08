
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Excels;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Excels
{
    public class CustomerExcelService : BaseExcelService<Customer>, ICustomerExcelService
    {
        ICustomerRepository _customerRepository;

        public CustomerExcelService(ICustomerRepository customerRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
        }



    }
}
