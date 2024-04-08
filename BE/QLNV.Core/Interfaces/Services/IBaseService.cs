using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Services
{
    public interface IBaseService<T> where T : class
    {
        /// <summary>
        /// Thêm mới một đối tượng 
        /// </summary>
        /// <param name="entity">thông tin chi tiết của đối tượng</param>
        /// <exception cref="ValidateException"></exception>
        /// <returns>số bản ghi thêm thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        Task<int> InsertServiceAsync(T entity);

        /// <summary>
        /// Sửa thông tin một đối tượng theo id
        /// </summary>
        /// <param name="entity">thông tin chi tiết của đối tượng</param>
        /// <param name="id">mã định danh đối tượng(khóa chính)</param>
        /// <returns>số bản ghi sửa thành công</returns>
        /// <exception cref="ValidateException"></exception>
        /// Created by: LQHUY(25/12/2023)
        Task<int> UpdateServiceAsync(T entity, Guid id);

        /// <summary>
        /// Xóa một đối tượng
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        Task<int> DeleteServiceAsync(Guid id);

        /// <summary>
        /// Xóa nhiều đối tượng
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Số lượng bản ghi xóa thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        Task<int> DeleteManyServiceAsync(List<Guid> ids);
    }
}
