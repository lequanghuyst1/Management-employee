using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MISA.AMIS.Core.DTOs.CustomerDTO;
using MISA.AMIS.Core.DTOs.EmployeeDTO;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Enums;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.ImportColumn;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using MISA.AMIS.Core.Interfaces.UnitOfWork;
using MISA.AMIS.Core.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Fields
        IEmployeeRepository _employeeRepository;
        Dictionary<string, string[]>? erorrs = new Dictionary<string, string[]>();
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        IMemoryCache _memoryCache;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public override async Task ValidateInsertAsync(Employee employee)
        {
            if (await _employeeRepository.CheckDuplicateCodeAsync(employee.EmployeeCode))
            {
                erorrs?.Add($"EmployeeCode", new string[] { String.Format(ResourceVN.DuplicateEmployeeCode, employee.EmployeeCode) });
                throw new ValidateException(HttpStatusCode.BadRequest, String.Format(ResourceVN.DuplicateEmployeeCode, employee.EmployeeCode), erorrs);
            }
        }

        public override async Task ValidateUpdateAsync(Employee employee)
        {
            if (await _employeeRepository.CheckEnityCodeEqualCodeByIdAsync(employee.EmployeeCode, employee.EmployeeId) ||
                !await _employeeRepository.CheckDuplicateCodeAsync(employee.EmployeeCode))
            {
                await Task.CompletedTask;
            }
            else
            {
                erorrs?.Add($"EmployeeCode", new string[] { String.Format(ResourceVN.DuplicateEmployeeCode, employee.EmployeeCode) });
                throw new ValidateException(HttpStatusCode.BadRequest, String.Format(ResourceVN.DuplicateEmployeeCode, employee.EmployeeCode), erorrs);
            }
        }
        public async Task<object> ReadDataFromExcel(IFormFile fileImport)
        {
            var templateFile = Common.EmloyeeImportColumns;
            var employeesImport = new List<EmployeeImportDto>();
            var employeesImportError = new List<EmployeeImportDto>();
            var employees = new List<Employee>();
            CheckFileImport(fileImport, templateFile);
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
                        //đọc thông tin từng dòng và gán giá trị
                        for (var row = 5; row <= rowCount; row++)
                        {
                            var employeeImport = new EmployeeImportDto();
                            //set thông tin cho đối tượng employeeImport
                            employeeImport = await BuildObject(templateFile, worksheet, row);

                            //kiểm tra thông tin dữ liệu
                            await ValidateDataImport(employeeImport);

                            //Nếu không có lỗi thì thực hiện chèn
                            if (employeeImport.ImportInvalidErrors?.Count == 0)
                            {
                                var employee = _mapper.Map<Employee>(employeeImport);
                                await _unitOfWork.Employees.InsertAsync(employee);
                                employees.Add(employee);
                                employeeImport.IsImport = true;
                            }
                            //Nếu cố lỗi add đối tượng vào danh sách đối tượng lỗi 
                            if (employeeImport.ImportInvalidErrors?.Count != 0)
                            {
                                employeesImportError.Add(employeeImport);
                                employeeImport.IsImport = false;
                            }

                            employeesImport.Add(employeeImport);
                        }
                    }
                }
            }
            TimeSpan timeSpan = TimeSpan.FromMinutes(30);

            var cacheKey = Guid.NewGuid().ToString();
            _memoryCache.Set<List<Employee>>(cacheKey, employees, timeSpan);

            var cacheKeyImportErorr = Guid.NewGuid().ToString();
            _memoryCache.Set<List<EmployeeImportDto>>(cacheKeyImportErorr, employeesImportError, timeSpan);

            var cacheKeyResutlImport = Guid.NewGuid().ToString();
            _memoryCache.Set<List<EmployeeImportDto>>(cacheKeyResutlImport, employeesImport, timeSpan);

            return new
            {
                CacheKey = cacheKey,
                CacheKeyImportError = cacheKeyImportErorr,
                CacheKeyResultImport = cacheKeyResutlImport,
                ListInfoImport = employeesImport,
            };
        }


        /// <summary>
        /// Build thông tin đối tượng theo từng dòng dữ liệu trong excel
        /// </summary>
        /// <param name="templateFile">File nhập mẫu</param>
        /// <param name="worksheet">sheet đang đọc để lấy dữ liệu</param>
        /// <param name="employeeImport">đối tượng để build thông tin</param>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task<EmployeeImportDto> BuildObject(List<ImportColumns> templateFile, ExcelWorksheet worksheet, int row)
        {
            var employeeImport = new EmployeeImportDto();
            for (int i = 0; i < templateFile.Count; i++)
            {
                if (templateFile[i].ColumnTitle == "STT")
                {
                    continue;
                }
                else
                {
                    var columnInsert = templateFile[i].ColumnInsert;
                    var cellValue = worksheet.Cells[row, i + 1].Value;
                    var property = employeeImport.GetType().GetProperty(columnInsert);
                    var dataType = (DataType)templateFile[i].ColumnDataType;
                    if (cellValue == null)
                    {
                        continue;
                    }

                    ProcessCellValueByDataType(dataType, ref cellValue);

                    if (dataType == DataType.ReferenceTable)
                    {
                        var referenceTable = templateFile[i].ObjectReferenceName;
                        switch (referenceTable)
                        {
                            case "Department":
                                var department = await _unitOfWork.Departments.GetByNameAsync(cellValue.ToString());
                                if (department != null)
                                {
                                    cellValue = department.DepartmentId;
                                    employeeImport.DepartmentName = department.DepartmentName;
                                    break;
                                }
                                break;
                            case "Position":
                                var position = await _unitOfWork.Positions.GetByNameAsync(cellValue.ToString());
                                if (position != null)
                                {
                                    cellValue = position.PositionId;
                                    employeeImport.PositionName = position.PositionName;
                                    break;
                                }
                                break;
                        }
                    }
                    property?.SetValue(employeeImport, cellValue);

                }
            }
            return employeeImport;
        }

        /// <summary>
        /// Xử lý giá trị của cell theo từng loại dữ liệu được khai báo
        /// </summary>
        /// <param name="dataType">Kiểu dữ liệu(string, enum, int,...)</param>
        /// <param name="cellValue">Giá trị lấy từ ô excel</param>
        /// CreatedBy: LQHUY(05/03/2024)
        private void ProcessCellValueByDataType(DataType dataType, ref object cellValue)
        {
            switch (dataType)
            {
                case DataType.Boolean:
                    break;
                case DataType.Int:
                    cellValue = ProcessTypeInt(cellValue);
                    break;
                case DataType.DateTime:
                    cellValue = ProcessDate(cellValue);
                    break;
                case DataType.Enum:
                    cellValue = ProcessGender(cellValue.ToString());
                    break;
                case DataType.Double:
                    cellValue = ProcessTypeDouble(cellValue);
                    break;
                default:
                    cellValue = cellValue.ToString();
                    break;
            }

        }

        /// <summary>
        /// Hầm kiểm tra dữ liệu khi import
        /// </summary>
        /// <param name="employeeImport">thông tin đối tượng import/param>
        /// <returns></returns>
        /// CreatedBy: (06/03/2024) 
        public async Task ValidateDataImport(EmployeeImportDto employeeImport)
        {
            var isDuplicate = await _unitOfWork.Employees.CheckDuplicateEmpoloyeeCode(employeeImport.EmployeeCode);
            if (employeeImport.EmployeeCode == null || employeeImport.EmployeeCode == "")
            {
                employeeImport.ImportInvalidErrors.Add("EmployeeCode", new string[] {
                                EmployeeResourceVN.FullNameNotEmpty});
            }
            if (isDuplicate)
            {
                employeeImport.ImportInvalidErrors.Add("EmployeeCode", new string[] {
                                String.Format(EmployeeResourceVN.EmployeeCodeDuplicate, employeeImport.EmployeeCode)});
            }
            if (employeeImport.Fullname == "" || employeeImport.Fullname == null)
            {
                employeeImport.ImportInvalidErrors.Add("Fullname", new string[] {
                                EmployeeResourceVN.FullNameNotEmpty});
            }
            if (employeeImport.DepartmentId == null)
            {
                employeeImport.ImportInvalidErrors.Add("DepartmentId", new string[] {
                                EmployeeResourceVN.DepartemntIdNotEmpty});

            }
        }

        public async Task<IEnumerable<Employee>> ImportEmployee(string keyCache)
        {
            var employees = _memoryCache.Get<List<Employee>>(keyCache);
            _unitOfWork.BeginTransaction();
            foreach (Employee employee in employees)
            {
                await _unitOfWork.Employees.InsertAsync(employee);
            }
            _unitOfWork.Commit();
            return employees;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Kiểm tra xem file có tồn tại hoặc đúng định dạng hay không
        /// </summary>
        /// <param name="fileImport"></param>
        /// <exception cref="ValidateException"></exception>
        /// Created By: LQHUY(16/01/2024)
        public void CheckFileImport(IFormFile fileImport, List<ImportColumns> templateFile)
        {

            if (fileImport == null || fileImport.Length <= 0)
            {
                erorrs.Add("FileImport", new string[] { ResourceVN.FileImportNotEmpty });
                throw new ValidateException(System.Net.HttpStatusCode.BadRequest, ResourceVN.FileImportNotEmpty, erorrs);
            }
            if (!Path.GetExtension(fileImport.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                erorrs.Add("FileImport", new string[] { ResourceVN.FileImportNotValid });
                throw new ValidateException(System.Net.HttpStatusCode.BadRequest, ResourceVN.FileImportNotValid, erorrs);
            }
            using (var stream = new MemoryStream())
            {
                fileImport.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    //Tệp mẫu được cấp luôn có ít nhất 1 sheet và phải có khai báo các thông tin - tức là không được phép null.
                    if (worksheet == null)
                    {
                        erorrs?.Add("FileImport", new string[] { ResourceVN.IsExit_Worksheet });
                        throw new ImportException(HttpStatusCode.BadRequest, ResourceVN.IsExit_Worksheet, erorrs);
                    }

                    var totalColums = worksheet.Dimension.Columns - 1;
                    //// Check số lượng cột trong workSheet có giống với số lượng cột tại worksheet như tệp mẫu hay không?
                    //if (totalColums != templateFile.Count())
                    //{
                    //    erorrs?.Add("FileImport", new string[] { ResourceVN.IsValidTotalColumns });
                    //    throw new ImportException(HttpStatusCode.BadRequest, ResourceVN.IsValidTotalColumns, erorrs);
                    //}

                    //duyệt tưng cột xem cột có giống với file mẫu hay không
                    for (int i = 1; i <= totalColums; i++)
                    {
                        var headerName = worksheet.Cells[3, i].Value.ToString();
                        var headerNameTemplate = templateFile.Where(col => col.ColumnTitle.ToString() == headerName).FirstOrDefault();
                        if (headerNameTemplate == null)
                        {
                            erorrs?.Add("FileImport", new string[] { String.Format(ResourceVN.NotEqualColumnsFile, headerName) }); ;
                            throw new ImportException(HttpStatusCode.BadRequest, String.Format(ResourceVN.NotEqualColumnsFile, headerName), erorrs);
                        }
                    }
                }
            }

        }

        public byte[] GetTemplateFile()
        {
            var templateFile = Common.EmloyeeImportColumns;
            var stream = new MemoryStream();
            // lấy ra số lượng cột cần dùng dựa vào số lượng header
            var countColHeader = templateFile.Count();
            using (var excelPackage = new ExcelPackage(stream))
            {
                // đặt tên người tạo file
                excelPackage.Workbook.Properties.Author = "LQHUY";

                var worksheet = excelPackage.Workbook.Worksheets.Add(string.Format(ResourceVN.ObjectList, EmployeeResourceVN.Employee.ToLower()));

                var headerCol = 1;
                var headerRow = 3;

                SetStyleTitleFileExcel(worksheet, countColHeader);

                //tạo các header từ templateFile
                foreach (var item in templateFile)
                {
                    //gán giá trị
                    worksheet.Cells[headerRow, headerCol].Value = item.ColumnTitle.ToString();
                    if (item.IsRequired)
                    {
                        worksheet.Cells[headerRow + 1, headerCol].Value = "(*)";
                    }
                    headerCol++;
                }

                //set style cho header
                var cells = worksheet.Cells[headerRow, 1, headerRow + 1, countColHeader];
                SetStyleHeaderFileExcel(cells);

                worksheet.Cells.AutoFitColumns();
                excelPackage.Save();

                //chuyển đổi nội dung của đối tượng stream thành một mảng byte.
                var bytes = stream.ToArray();
                return bytes;
            }

        }
        public byte[] GetFileImportFailure(string cacheKeyImportErorr)
        {
            var templateFile = Common.EmloyeeImportColumns;
            var stream = new MemoryStream();

            //lấy ra danh sách các đối tượng import lỗi
            var employeesErorr = _memoryCache.Get<List<EmployeeImportDto>>(cacheKeyImportErorr);

            // lấy ra số lượng cột cần dùng dựa vào số lượng header
            var countColHeader = templateFile.Count();

            using (var excelPackage = new ExcelPackage(stream))
            {
                // đặt tên người tạo file
                excelPackage.Workbook.Properties.Author = "LQHUY";

                //đặt tên cho worksheet làm việc
                var worksheet = excelPackage.Workbook.Worksheets.Add(string.Format(ResourceVN.ObjectList, EmployeeResourceVN.Employee.ToLower()));

                var headerCol = 1;
                var headerRow = 3;

                SetStyleTitleFileExcel(worksheet, countColHeader);

                //tạo các header từ file mẫu
                foreach (var item in templateFile)
                {
                    //gán giá trị
                    worksheet.Cells[headerRow, headerCol].Value = item.ColumnTitle.ToString();
                    if (item.IsRequired)
                    {
                        worksheet.Cells[headerRow + 1, headerCol].Value = "(*)";
                    }
                    headerCol++;
                }
                worksheet.Cells[headerRow, countColHeader + 1].Value = ResourceVN.Status;
                worksheet.Column(countColHeader + 1).Width = 60;


                var rowStart = 5;
                var numberOrder = 1;

                //duyệt từng đối tượng lỗi và gán giá trị vào từng cell trong worksshet
                foreach (var emp in employeesErorr)
                {
                    var colStart = 1;
                    for (int i = 0; i < templateFile.Count; i++)
                    {
                        if (templateFile[i].ColumnTitle == "STT")
                        {
                            worksheet.Cells[rowStart, colStart].Value = numberOrder;
                        }
                        else
                        {
                            //lấy ra giá trị của property tương ứng 
                            var cellValue = emp.GetType()?.GetProperty(templateFile[i].ColumnInsert)?.GetValue(emp);
                            var dataType = (DataType)templateFile[i].ColumnDataType;
                            if (cellValue == null)
                            {
                                worksheet.Cells[rowStart, colStart].Value = "";
                            }

                            //gán lại giá trị cho từng kiểu dữ liệu được khai báo
                            if (dataType == DataType.DateTime)
                            {
                                cellValue = String.Format("{0:dd/MM/yyyy}", cellValue);
                            }
                            if (dataType == DataType.Enum)
                            {
                                cellValue = Common.ProccesValueGender(cellValue);
                            }
                            if (dataType == DataType.ReferenceTable)
                            {
                                var referenceTable = templateFile[i].ObjectReferenceName;
                                switch (referenceTable)
                                {
                                    case "Department":
                                        cellValue = emp.DepartmentName;
                                        break;
                                    case "Position":
                                        cellValue = emp.PositionName;
                                        break;
                                }
                            }
                            //gán giá trị cho cell
                            worksheet.Cells[rowStart, colStart].Value = cellValue?.ToString();

                            //add comment cho cell có lỗi
                            if (emp.ImportInvalidErrors.Count > 0)
                            {

                                foreach (var errors in emp.ImportInvalidErrors)
                                {
                                    //kiểm tra xem cột nhập có trùng với key lỗi hay không
                                    if (templateFile[i].ColumnInsert == errors.Key)
                                    {
                                        worksheet.Cells[rowStart, colStart].AddComment(String.Join(",", errors.Value));


                                    }

                                }
                            }
                        }
                        //tự động co giãn cột phù hợp với độ dài giá trị từng cell
                        worksheet.Columns[colStart].AutoFit();
                        //căn giá trị của cell theo chiều dọc ra giữa
                        worksheet.Rows[rowStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colStart++;
                    }
                    //Kiểm tra xem dòng thêm mới có lỗi hay không nếu có thì gán giá trị lỗi
                    if (emp.ImportInvalidErrors.Count > 0)
                    {
                        var errorlst = new List<string>();
                        foreach (var errors in emp.ImportInvalidErrors)
                        {
                            errorlst.Add(String.Join("", errors.Value));
                        }

                        worksheet.Cells[rowStart, countColHeader + 1].Style.WrapText = true;
                        //gán giá trị vào cell
                        worksheet.Cells[rowStart, countColHeader + 1].Value = string.Join(Environment.NewLine, errorlst);
                        //set màu đỏ cho chữ 
                        worksheet.Cells[rowStart, countColHeader + 1].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }

                    rowStart++;
                    numberOrder++;
                }

                //set style cho header
                var cells = worksheet.Cells[headerRow, 1, headerRow + 1, countColHeader + 1];
                SetStyleHeaderFileExcel(cells);

                excelPackage.Save();
                //chuyển đổi nội dung của đối tượng stream thành một mảng byte.
                var bytes = stream.ToArray();
                return bytes;
            }
        }

        public byte[] GetFileResultImport(string cacheKeyResutlImport)
        {
            var templateFile = Common.EmloyeeImportColumns;
            var stream = new MemoryStream();
            var listEmployeeImport = _memoryCache.Get<List<EmployeeImportDto>>(cacheKeyResutlImport);
            // lấy ra số lượng cột cần dùng dựa vào số lượng header
            var countColHeader = templateFile.Count();

            using (var excelPackage = new ExcelPackage(stream))
            {
                // đặt tên người tạo file
                excelPackage.Workbook.Properties.Author = "LQHUY";

                //đặt tên cho worksheet làm việc
                var worksheet = excelPackage.Workbook.Worksheets.Add(string.Format(ResourceVN.ObjectList, EmployeeResourceVN.Employee.ToLower()));

                var headerCol = 1;
                var headerRow = 3;

                SetStyleTitleFileExcel(worksheet, countColHeader);

                //tạo các header từ file mẫu
                foreach (var item in templateFile)
                {
                    //gán giá trị
                    worksheet.Cells[headerRow, headerCol].Value = item.ColumnTitle.ToString();
                    if (item.IsRequired)
                    {
                        worksheet.Cells[headerRow + 1, headerCol].Value = "(*)";
                    }
                    headerCol++;
                }
                worksheet.Cells[headerRow, countColHeader + 1].Value = ResourceVN.Status;
                //set chiều rộng cho ô tình trạng
                worksheet.Column(countColHeader + 1).Width = 60;

                var rowStart = 5;
                var numberOrder = 1;

                //duyệt từng đối tượng lỗi và gán giá trị vào từng cell trong worksshet
                foreach (var emp in listEmployeeImport)
                {
                    var colStart = 1;
                    for (int i = 0; i < templateFile.Count; i++)
                    {
                        if (templateFile[i].ColumnTitle == "STT")
                        {
                            worksheet.Cells[rowStart, colStart].Value = numberOrder;
                        }
                        else
                        {
                            //lấy ra giá trị của property tương ứng 
                            var cellValue = emp.GetType()?.GetProperty(templateFile[i].ColumnInsert)?.GetValue(emp);
                            var dataType = (DataType)templateFile[i].ColumnDataType;
                            if (cellValue == null)
                            {
                                worksheet.Cells[rowStart, colStart].Value = "";
                            }

                            //gán lại giá trị cho từng kiểu dữ liệu được khai báo
                            if (dataType == DataType.DateTime)
                            {
                                cellValue = String.Format("{0:dd/MM/yyyy}", cellValue);
                            }
                            if (dataType == DataType.Enum)
                            {
                                cellValue = Common.ProccesValueGender(cellValue);
                            }
                            if (dataType == DataType.ReferenceTable)
                            {
                                var referenceTable = templateFile[i].ObjectReferenceName;
                                switch (referenceTable)
                                {
                                    case "Department":
                                        cellValue = emp.DepartmentName;
                                        break;
                                    case "Position":
                                        cellValue = emp.PositionName;
                                        break;
                                }
                            }
                            //gán giá trị cho cell
                            worksheet.Cells[rowStart, colStart].Value = cellValue?.ToString();

                            //add comment cho cell có lỗi
                            if (emp.ImportInvalidErrors.Count > 0)
                            {
                                foreach (var errors in emp.ImportInvalidErrors)
                                {
                                    //kiểm tra xem cột nhập có trùng với key lỗi hay không
                                    if (templateFile[i].ColumnInsert == errors.Key)
                                    {
                                        //gán giá trị cho comment
                                        worksheet.Cells[rowStart, colStart].AddComment(String.Join(",", errors.Value));
                                    }

                                }
                            }
                        }
                        //tự động co giãn cột phù hợp với độ dài giá trị từng cell
                        worksheet.Columns[colStart].AutoFit();
                        //căn giá trị của cell theo chiều dọc ra giữa
                        worksheet.Rows[rowStart].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        colStart++;
                    }
                    //Kiểm tra xem dòng thêm mới có lỗi hay không
                    //Nếu không có thì xét tình trạng là hợp lệ
                    if (emp.ImportInvalidErrors.Count == 0)
                    {
                        worksheet.Cells[rowStart, countColHeader + 1].Value = ResourceVN.Valid;
                        worksheet.Cells[rowStart, countColHeader + 1].Style.Font.Color.SetColor(System.Drawing.Color.RoyalBlue);
                    }
                    //Nếu có thì gán giá trị lỗi
                    else
                    {
                        var errorlst = new List<string>();
                        foreach (var errors in emp.ImportInvalidErrors)
                        {
                            errorlst.Add(String.Join("", errors.Value));
                        }
                        //xuống dòng khi vượt quá độ dài
                        worksheet.Cells[rowStart, countColHeader + 1].Style.WrapText = true;
                        //gán giá trị cho cell
                        worksheet.Cells[rowStart, countColHeader + 1].Value = string.Join(Environment.NewLine, errorlst);
                        //set màu đỏ cho ô tình trạng có lỗi
                        worksheet.Cells[rowStart, countColHeader + 1].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }
                    rowStart++;
                    numberOrder++;
                }

                //set style cho header
                var cells = worksheet.Cells[headerRow, 1, headerRow + 1, countColHeader + 1];
                SetStyleHeaderFileExcel(cells);

                excelPackage.Save();
                //chuyển đổi nội dung của đối tượng stream thành một mảng byte.
                var bytes = stream.ToArray();
                return bytes;
            }
        }
        /// <summary>
        /// Hàm thiết lập các style cho toàn bộ file và title
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="countColHeader">tổng số cột</param>
        /// CreatdBy: LQHUY(05/03/2024)
        public void SetStyleTitleFileExcel(ExcelWorksheet worksheet, int countColHeader)
        {
            // set style mặc định cho toàn bộ file
            worksheet.Cells.Style.Font.Size = 12;
            worksheet.Cells.Style.Font.Name = "Calibri";

            // gán giá trị cho cell vừa merge
            worksheet.Cells[1, 1].Value = string.Format(ResourceVN.ObjectList, EmployeeResourceVN.Employee.ToLower());

            // merge các column lại từ column 1 đến số column header và set style
            worksheet.Cells[1, 1, 2, countColHeader].Merge = true;
            worksheet.Cells[1, 1, 2, countColHeader].Style.Font.Bold = true;
            worksheet.Cells[1, 1, 2, countColHeader].Style.Font.Size = 24;
            worksheet.Cells[1, 1, 2, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        }

        /// <summary>
        /// Hàm thiết lập style cho header
        /// </summary>
        /// <param name="cells"></param>
        /// CreatdBy: LQHUY(05/03/2024)
        public void SetStyleHeaderFileExcel(ExcelRange cells)
        {
            //set màu thành gold
            var fill = cells.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.Gold);

            //căn chỉnh các border
            var border = cells.Style.Border;
            border.Bottom.Style =
                border.Top.Style =
                border.Left.Style =
                border.Right.Style = ExcelBorderStyle.Thin;

            cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        /// <summary>
        /// Xử lí dữ liệu dạng gender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns>định dạng chuẩn</returns>
        /// /// Created By: LQHUY(16/01/2024)
        public GenderEnum? ProcessGender(string? gender)
        {
            if (ResourceVN.Gender_Male == gender)
            {
                return Enums.GenderEnum.Male;

            }
            else if (ResourceVN.Gender_Female == gender)
            {
                return Enums.GenderEnum.Female;
            }
            else if (ResourceVN.Gender_Other == gender)
            {
                return Enums.GenderEnum.Other;
            }
            return null;

        }

        /// <summary>
        /// Xử lí dữ liệu dạng datetime khi nhập khẩu
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns>định dạng chuẩn</returns>
        /// /// Created By: LQHUY(16/01/2024)
        DateTime? ProcessDate(object? cellValue)
        {
            string format = "dd/MM/yyyy";
            DateTime date;
            if (DateTime.TryParseExact(cellValue?.ToString(), format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
            {
                return date;
            }
            return null;
        }
        /// <summary>
        /// Hàm dữ lý dữ liệu là int khi nhập khẩu
        /// </summary>
        /// <param name="cellValue">giá trị ô excel tương ứng</param>
        /// <returns>dữ liệu được chuyển đổi về kiểu int</returns>
        /// Created By: LQHUY(04/03/2024)
        int? ProcessTypeInt(object? cellValue)
        {
            if (cellValue != null)
            {
                if (int.TryParse(cellValue.ToString(), out int value))
                {
                    return value;
                }
            }
            return null;
        }
        /// <summary>
        /// Hàm dữ lý dữ liệu là double khi nhập khẩu
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns>dữ liệu được chuyển đổi về kiểu double</returns>
        /// Created By: LQHUY(04/03/2024)
        double? ProcessTypeDouble(object? cellValue)
        {
            if (cellValue != null)
            {
                if (double.TryParse(cellValue.ToString(), out double value))
                {
                    return value;
                }
            }
            return null;
        }



        #endregion
    }
}
