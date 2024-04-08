using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Excels
{
    public interface IBaseExcelService<T>
    {
        /// <summary>
        /// Export tất cả các bản ghi vào file excel
        /// </summary>
        /// <param name="data">Danh sách các bản ghi lấy từ database</param>
        /// <returns>Mảng byte của file excel</returns>
        /// Created By: LQHUY(04/01/2023)
        Task<byte[]> ExportAllAsync();

        /// <summary>
        /// Export các bản ghi theo danh dách id vào file excel
        /// </summary>
        /// <param name="data">danh sách các bản ghi lấy từ database theo danh sách id</param>
        /// <returns>Mảng byte của file excel</returns>
        /// Created By: LQHUY(04/01/2023)
        Task<byte[]> ExportListAsync(List<Guid> ids);
    }
}
