using Microsoft.AspNetCore.Http;
using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        /// <summary>
        /// Hàm import excel
        /// </summary>
        /// <param name="fileImport">file excel</param>
        /// <returns>
        /// Danh cách các bản ghi 
        /// </returns>
        /// Created By: LQHUY(12/01/2024)
        Task<IEnumerable<Customer>> ImportCustomer(IFormFile fileImport);
    }

}
