using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Infrastructures
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {

        /// <summary>
        /// Kiểm tra EmployeeCode đã tồn tại trong database chưa
        /// </summary>
        /// <param name="employeeCode">mã khách hàng</param>
        /// <returns>
        /// true - đã có trong database
        /// flase - chưa có trong database
        /// </returns>
        /// Created by: LQHUY(02/01/2024)
        Task<bool> CheckDuplicateEmpoloyeeCode(string employeeCode);

        /// <summary>
        /// Lấy ra danh sách các bản ghi theo điều kiện và phân trang
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageNumber">trang muốn lấy</param>
        /// <param name="searchString">điêu kiện tìm kiếm(EmployeeCode or FullName)</param>
        /// <returns>đối tượng gồm danh danh sách bản ghi, tổng số trang và tổng số bản ghi</returns>
        /// Created by: LQHUY(02/01/2024)
        Task<PagingEntity<Employee>> Paging(int pageSize, int pageNumber, string? searchString);

       


    }
}
