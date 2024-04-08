using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Infrastructures
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// Kiểm tra customerCode đã tồn tại trong database chưa
        /// </summary>
        /// <param name="customerCode">mã khách hàng</param>
        /// <returns>
        /// true - đã có trong database
        /// flase - chưa có trong database
        /// </returns>
        /// Created by: LQHUY(25/12/2023)
        Task<bool> CheckCustomerCodeDuplicate(string customerCode);


        /// <summary>
        /// Lấy ra danh sách các bản ghi theo điều kiện và phân trang
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageNumber">trang muốn lấy</param>
        /// <param name="searchString">điêu kiện tìm kiếm(CustomerCode or FullName)</param>
        /// <returns>đối tượng gồm danh danh sách bản ghi, tổng số trang và tổng số bản ghi</returns>
        /// Created by: LQHUY(25/12/2023)
        Task<PagingEntity<Customer>> Paging(int pageSize, int pageNumber, string? searchString);
    }
}
