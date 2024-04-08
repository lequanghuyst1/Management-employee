using MISA.AMIS.Core.MSAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    /// <summary>
    /// Class BaseEntity
    /// </summary>
    /// Created By: LQHUY(25/12/2023)
    public class BaseEntity
    {
        #region Properties
        /// <summary>
        /// Ngày tạo
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Ngày tạo")]
        [NotQueryExport]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Người tạo")]
        [NotQueryExport]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Ngày chỉnh sửa")]
        [NotQueryExport]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Người chỉnh sửa")]
        [NotQueryExport]
        public string? ModifiedBy { get; set; }
        #endregion
    }
}
