using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Enums
{
    /// <summary>
    /// Enum Gender
    /// </summary>
    /// Created By: LQHUY(02/01/2024)
    public enum GenderEnum
    {
        /// <summary>
        /// Nam
        /// </summary>
        Male = 0,
        /// <summary>
        /// Nữ
        /// </summary>
        Female = 1,
        /// <summary>
        /// Khác
        /// </summary>
        Other = 2,
    }

    /// <summary>
    /// Enum WorkStatus
    /// </summary>
    /// Created By: LQHUY(02/01/2024)
    public enum WorkStatus
    {
        /// <summary>
        /// Đang làm việc
        /// </summary>
        Working = 0,
        /// <summary>
        /// Đã nghỉ
        /// </summary>
        Stopped = 2,
    }

    /// <summary>
    /// Các kiểu dữ liệu
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Chuỗi
        /// </summary>
        String = 0,

        /// <summary>
        /// Ngày tháng
        /// </summary>
        DateTime = 1,

        /// <summary>
        /// Số nguyên
        /// </summary>
        Int = 2,
        /// <summary>
        /// True/ False
        /// </summary>
        Boolean = 3,

        /// <summary>
        /// Enum
        /// </summary>
        Enum = 4,

        /// <summary>
        /// Tham chiếu tới bảng dữ liệu xác định trong Database
        /// </summary>
        ReferenceTable = 5,

        /// <summary>
        /// Double
        /// </summary>
        Double = 6,
    }
}
