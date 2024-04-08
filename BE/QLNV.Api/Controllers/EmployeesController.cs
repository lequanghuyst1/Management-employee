using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Excels;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Excels;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Resources;
using MISA.AMIS.Core.Services;
using MISA.AMIS.Infrastructure.Repository;

namespace MISA.AMIS.Api.Controllers
{
    [ApiController]
    public class EmployeesController : MISABaseController<Employee>
    {
        #region Fields
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        IEmployeeExcelService _employeeExcelService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService, IEmployeeExcelService employeeExcelService) : base(employeeRepository, employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _employeeExcelService = employeeExcelService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tìm kiếm theo FullName và EmployeeCode và PhoneNumber và phân trang 
        /// </summary>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="pageNumber">trang hiện tại</param>
        /// <param name="searchString">chuỗi tìm kiếm</param>
        /// <returns>
        /// - StatusCode: 200 và danh sách các bản ghi, tổng số bản ghi, tổng số trang thỏa mãn điều kiện tìm kiếm
        /// - StatusCode: 400 khi có vấn đề phía client
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(06/01/2024)
        [HttpGet("Filter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(int pageSize, int pageNumber, string? searchString)
        {
            try
            {
                var res = await _employeeRepository.Paging(pageSize, pageNumber, searchString);
                return Ok(res);
            }
            catch (AuthenticationException)
            {
                Dictionary<string, string[]>? errors = new Dictionary<string, string[]>();
                errors.Add("Access token", new string[] { ResourceVN.UnauthorizedAccess });
                throw new ConnectDbException(System.Net.HttpStatusCode.InternalServerError, ResourceVN.UnauthorizedAccess, errors);

            }
        }

       

        /// <summary>
        /// Xuất tất cả thông tin nhân viên vào file excel
        /// </summary>
        /// <returns>
        /// - Status Code: 200 - link download file excel
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(06/01/2024)
        [HttpPost("Export")]
        public async Task<IActionResult> ExportToExcel([FromBody] List<Guid>? ids)
        {
            var contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var fileName = EmployeeResourceVN.EmployeeListFile;
            if (ids?.Count() > 0)
            {
                var bytes = await _employeeExcelService.ExportListAsync(ids);
                return File(bytes, contenType, fileName);
            }
            var res = await _employeeExcelService.ExportAllAsync();

            return File(res, contenType, fileName);
        }

        /// <summary>
        /// Đọc dữ liệu từ file excel
        /// </summary>
        /// <param name="formFile">file nhập khẩu</param>
        /// <returns>
        /// - Status Code: 200 - danh sách các bản ghi
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(29/02/2024)
        [HttpPost("ReadDataFromExcel")]
        public async Task<IActionResult> ReadDataFromExcel(IFormFile formFile)
        {
            var res = await _employeeService.ReadDataFromExcel(formFile);
            return Ok(res);
        }

        /// <summary>
        /// Nhập khẩu tất cả thông tin nhân vinên từ file excel vào database
        /// </summary>
        /// <param name="keyCache">key cache</param>
        /// <returns>
        /// </returns>
        /// Created by: LQHUY(16/01/2024)
        [HttpPost("Import")]
        public async Task<IActionResult> Import([FromBody] string keyCache)
        {
            var res = await _employeeService.ImportEmployee(keyCache);
            return Ok(res);
        }

        /// <summary>
        /// Lấy file mẫu để thực hiện nhập khẩu
        /// </summary>
        /// <returns>
        /// - Status Code: 200 - link download file excel
        /// - StatusCode: 500 có vấn đề trên service
        /// </returns>
        /// Created by: LQHUY(06/01/2024)
        [HttpPost("TemplateFile")]
        public IActionResult GetTemplateFile()
        {
            var contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var fileName = EmployeeResourceVN.EmployeeListFile;

            var bytes = _employeeService.GetTemplateFile();

            return File(bytes, contenType, fileName);
        }


        [HttpPost("FileImportError")]
        public IActionResult GetFileImportError([FromBody] string cacheKeyImportErorr)
        {
            var contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var fileName = EmployeeResourceVN.EmployeeListFile;

            var bytes = _employeeService.GetFileImportFailure(cacheKeyImportErorr);

            return File(bytes, contenType, fileName);
        }

        [HttpPost("FileImportResult")]
        public IActionResult GetFileResultImport([FromBody] string cacheKeyResutlImport)
        {
            var contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var fileName = EmployeeResourceVN.EmployeeListFile;

            var bytes = _employeeService.GetFileResultImport(cacheKeyResutlImport);

            return File(bytes, contenType, fileName);
        }

        #endregion
    }
}
