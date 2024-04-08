using Microsoft.AspNetCore.Http;
using MISA.AMIS.Core.DTOs.EmployeeDTO;
using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        /// <summary>
        /// Đọc dữ liệu từ file lưu vào cache
        /// </summary>
        /// <param name="fileImport">file excel</param>
        /// <returns>
        /// Danh sách các bản ghi và key cache
        /// </returns>
        /// Created By: LQHUY(29/02/2024)
        Task<object> ReadDataFromExcel(IFormFile fileImport);

        /// <summary>
        /// Hàm import excel
        /// </summary>
        /// <param name="fileImport">file excel</param>
        /// <returns>
        /// Danh cách các bản ghi 
        /// </returns>
        /// Created By: LQHUY(12/01/2024)
        Task<IEnumerable<Employee>> ImportEmployee(string keyCache);

        /// <summary>
        /// Lấy ra file mẫu để nhập khẩu
        /// </summary>
        /// <returns></returns>
        /// Created By: LQHUY(04/03/2024)
        byte[] GetTemplateFile();

        /// <summary>
        /// Xuất file chứa danh sách nhân viên import lỗi
        /// </summary>
        /// <param name="keyCache"></param>
        /// <returns>
        /// File excel chứa danh sách các bản ghi
        /// </returns>
        /// Created By: LQHUY(04/03/2024)
        /// 
        byte[] GetFileImportFailure(string cacheKeyImportErorr);

        /// <summary>
        /// Xuất file chứa kết quả import
        /// </summary>
        /// <param name="cacheKeyResutlImport"></param>
        /// <returns>File chứa kết quả import</returns>
        /// Created By: LQHUY(04/03/2024)
        byte[] GetFileResultImport(string cacheKeyResutlImport); 
    }
}
