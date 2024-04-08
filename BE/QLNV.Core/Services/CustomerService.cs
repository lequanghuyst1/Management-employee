using AutoMapper;
using Microsoft.AspNetCore.Http;
using MISA.AMIS.Core.DTOs;
using MISA.AMIS.Core.DTOs.CustomerDTO;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Interfaces.UnitOfWork;
using MISA.AMIS.Core.Resources;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace MISA.AMIS.Core.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Fields
        ICustomerRepository _customerRepository;
        Dictionary<string, string[]>? erorrs = new Dictionary<string, string[]>();
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion
        public async Task<IEnumerable<Customer>> ImportCustomer(IFormFile fileImport)
        {
            CheckFileImport(fileImport);
            var customers = new List<CustomerImport>();
            using (var stream = new MemoryStream())
            {
                fileImport.CopyTo(stream);
                using (var excelPackage = new ExcelPackage(stream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets[0];
                    _unitOfWork.BeginTransaction();
                    if (worksheet != null)
                    {
                        var rowCount = worksheet.Dimension.Rows;
                        for (var row = 3; row <= rowCount; row++)
                        {
                            var dob = worksheet?.Cells[row, 6]?.Value?.ToString()?.Trim();
                            var customerImport = new CustomerImport()
                            {
                                CustomerCode = worksheet?.Cells[row, 1]?.Value?.ToString()?.Trim(),
                                FullName = worksheet?.Cells[row, 2]?.Value?.ToString()?.Trim(),
                                PhoneNumber = worksheet?.Cells[row, 5]?.Value?.ToString()?.Trim(),
                                DateOfBirth = ProcessDate(dob),
                                CompanyName = worksheet?.Cells[row, 7]?.Value?.ToString()?.Trim(),
                                Email = worksheet?.Cells[row, 9]?.Value?.ToString()?.Trim(),
                                Address = worksheet?.Cells[row, 10]?.Value?.ToString()?.Trim(),
                            };

                            //Kiểm tra trùng mã
                            var isDuplicate = await _unitOfWork.Customers.CheckCustomerCodeDuplicate(customerImport.CustomerCode);
                            if (customerImport.CustomerCode == null || customerImport.CustomerCode == "")
                            {
                                customerImport.ImportInvalidErrors.Add("CustomerCode", new string[] {
                                $"Mã khách hàng không được để trống"});
                            }
                            if (isDuplicate)
                            {
                                customerImport.ImportInvalidErrors.Add("CustomerCode", new string[] {
                                $"Mã khách hàng {customerImport.CustomerCode} đã tồn tại trong hệ thống"});
                            }
                            if (customerImport.ImportInvalidErrors?.Count == 0)
                            {
                                var customer = _mapper.Map<Customer>(customerImport);
                                //Thực hiện thêm mới
                                var res = await _unitOfWork.Customers.InsertAsync(customer);
                                if (res > 0)
                                {
                                    customerImport.IsImport = true;
                                }
                            }
                            customers.Add(customerImport);
                        }
                    }
                    _unitOfWork.Commit();
                }
            }
            return customers;
        }

        /// <summary>
        /// Kiểm tra xem file có tồn tại hoặc đúng định dạng hay không
        /// </summary>
        /// <param name="fileImport"></param>
        /// <exception cref="ValidateException"></exception>
        /// Created By: LQHUY(16/01/2024)
        private void CheckFileImport(IFormFile fileImport)
        {
            if (fileImport == null || fileImport.Length <= 0)
            {
                throw new ValidateException("File nhập khẩu không được để trống");
            }
            if (!Path.GetExtension(fileImport.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidateException("File nhập khẩu không đúng định dạng cho phép");

            }
        }
        /// <summary>
        /// Xử lí dữ liệu dạng datetime
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns>định dạng chuẩn</returns>
        /// /// Created By: LQHUY(16/01/2024)
        DateTime? ProcessDate(object? dateValue)
        {
            if (dateValue != null)
            {
                string format = "dd/MM/yyyy";
                DateTime date;
                if (DateTime.TryParseExact(dateValue.ToString(),format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    return date;
                }
            }
            return null;
        }
    }
}
