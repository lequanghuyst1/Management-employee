
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Interfaces.Excels;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Services;
using MISA.AMIS.Infrastructure.Repository;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MISA.AMIS.Api.Controllers
{
    [ApiController]
    public class CustomersController : MISABaseController<Customer>
    {
        #region Field
        ICustomerRepository _customerRepository;
        ICustomerExcelService _customerExcelService;
        ICustomerService _customerService;
        #endregion

        #region Constructor
        public CustomersController(ICustomerRepository customerRepository, ICustomerService customerService, ICustomerExcelService customerExcelService) : base(customerRepository, customerService)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
            _customerExcelService = customerExcelService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tìm kiếm theo FullName và CustomerCode và phân trang 
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="pageNumber">trang hiện tại</param>
        /// <param name="searchString">chuỗi tìm kiếm</param>
        /// <returns>
        /// - StatusCode: 200 và danh sách các bản ghi, tổng số bản ghi, tổng số trang thỏa mãn điều kiện tìm kiếm
        /// - StatusCode: 400 khi có vấn đề phía client
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        [HttpGet("Filter")]
        public async Task<IActionResult> Filter(int pageSize, int pageNumber, string? searchString)
        {
            var res = await _customerRepository.Paging(pageSize, pageNumber, searchString);
            return Ok(res);
        }


        /// <summary>
        /// Xuất tất cả thông tin khách hàng vào file excel
        /// </summary>
        /// <returns>
        /// - Status Code: 200 - link download file excel
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(04/01/2024)
        [HttpGet("Export/Excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Danh sách khách hàng.xlsx";
            var res = await _customerExcelService.ExportAllAsync();

            return File(res,contenType,fileName);
        }


        /// <summary>
        /// Nhập khẩu tất cả thông tin khách hàng từ file excel vào database
        /// </summary>
        /// <returns>
        /// - Status Code: 200 - danh sách các bản ghi
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(16/01/2024)
        [HttpPost("Import")]
        public async Task<IActionResult> Import(IFormFile formFile)
        {
            var res = await _customerService.ImportCustomer(formFile);
            return Ok(res);
        }
        #endregion
    }
}
