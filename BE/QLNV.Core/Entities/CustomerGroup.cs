using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    /// <summary>
    /// Class CustomerGroup
    /// </summary>
    /// Created By: LQHUY(25/12/2023)
    public class CustomerGroup : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Mã định danh nhóm khách hàng
        /// </summary>
        /// Created By: LQHUY(25/01/2023)
        public Guid CustomerGroupId { get; set; }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        /// Created By: LQHUY(25/01/2023)
        [Required(ErrorMessage = "Tên nhóm khách hàng không được phép để trống")]
        public string CustomerGroupName { get; set; }
        #endregion
    }
}
