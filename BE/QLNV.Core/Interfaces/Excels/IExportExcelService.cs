using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Excels
{
    public interface IExportExcelService<T>
    {
        /// <summary>
        /// Thực hiện thêm thông tin vào file excel
        /// </summary>
        /// <param name="data">danh sách các bản ghi từ database</param>
        /// <returns>Trả về thông tin bên trong file excel</returns>
        /// Created By: LQHUY(06/01/2024)
        public byte[] ExportExcelAsync(IEnumerable<T> data);
    }
}
