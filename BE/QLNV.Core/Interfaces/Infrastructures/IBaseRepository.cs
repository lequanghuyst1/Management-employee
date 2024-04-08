using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Infrastructures
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Lấy ra toàn bộ đối tượng trong database
        /// </summary>
        /// <returns>danh sách các bản ghi</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) sửa lại hàm khử sự phụ thuộc trực tiếp khi kết nối với datdabase
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Lấy một đối tượng theo id(khóa chính)
        /// </summary>
        /// <param name="id">mã định danh</param>
        /// <returns>null hoặc thông tin chi tiết 1 đối tượng</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) sửa lại hàm khử sự phụ thuộc trực tiếp khi kết nối với datdabase
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Lấy ra 1 danh sách đối tượng theo danh sách id
        /// </summary>
        /// <param name="entity">thông tin chi tiết đối tượng</param>
        /// <returns>số lượng bản ghi khi thêm thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) 
        Task<IEnumerable<T>> GetByIdsAsync(List<Guid> ids);

        /// <summary>
        /// Lấy ra một mã entityCode mới
        /// </summary>
        /// <returns>entityCode</returns>
        /// Created by: LQHUY(25/12/2023)
        Task<string> GetNewCodeAsync();

        /// <summary>
        /// Thêm mới 1 đối tương
        /// </summary>
        /// <param name="entity">thông tin chi tiết đối tượng</param>
        /// <returns>số lượng bản ghi khi thêm thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) sửa lại hàm khử sự phụ thuộc trực tiếp khi kết nối với datdabase
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// Sửa thông tin một đối tượng theo id(khóa chính)
        /// </summary>
        /// <param name="entity">thông tin chi tiết đối tượng đã chỉnh sửa</param>
        /// <param name="id">mã định danh</param>
        /// <returns>số lượng bản ghi khi sửa thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) sửa lại hàm khử sự phụ thuộc trực tiếp khi kết nối với datdabase
        Task<int> UpdateAsync(T entity, Guid id);

        /// <summary>
        /// Xóa 1 đối tượng theo id(khóa chính)
        /// </summary>
        /// <param name="id">mã định dang của đối tượng</param>
        /// <returns>số lượng bản ghi xóa thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) sửa lại hàm khử sự phụ thuộc trực tiếp khi kết nối với datdabase
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Xóa nhiều đối tượng theo list id(khóa chính)
        /// </summary>
        /// <param name="entity">Xóa các đối tượng theo list id</param>
        /// <param name="id">mã định danh</param>
        /// <returns>số lượng bản ghi khi xóa thành công</returns>
        /// Created by: LQHUY(25/12/2023)
        /// Modified by: LQHUY(03/01/2024) sửa lại hàm khử sự phụ thuộc trực tiếp khi kết nối với datdabase
        Task<int> DeleteManyAsync(List<Guid> ids);

        /// <summary>
        /// Kiểm tra enityCode đã tồn tại trong database chưa
        /// </summary>
        /// <param name="entityCode">mã khách hàng</param>
        /// <returns>
        /// true - đã có trong database
        /// flase - chưa có trong database
        /// </returns>
        /// Created by: LQHUY(02/01/2024)
        Task<bool> CheckDuplicateCodeAsync(string entityCode);

        /// <summary>
        /// Lấy ra entityCode theo id(khóa chính)
        /// </summary>
        /// <param name="id">mã định danh</param>
        /// <returns>entityCode</returns>
        /// Created by: LQHUY(02/01/2024)
        Task<string?> GetCodeById(Guid id);

        /// <summary>
        /// Hàm kiểm tra entityCode có giống với code trong Db hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns>
        /// true - giống nhau
        /// false - khác nhau
        /// </returns>
        /// Created by: LQHUY(02/01/2024)
        Task<bool> CheckEnityCodeEqualCodeByIdAsync(string entityCode, Guid id);

        /// <summary>
        /// Lấy ra thông tin entity theo tên
        /// </summary>
        /// <param name="entityName">tên của entity</param>
        /// <returns></returns>
        /// Created by: LQHUY(05/03/2024)
        Task<T?> GetByNameAsync(string entityName);
    }
}
