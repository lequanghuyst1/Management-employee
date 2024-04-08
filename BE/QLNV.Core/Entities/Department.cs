using MISA.AMIS.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    /// <summary>
    /// Class Department
    /// </summary>
    /// Created By: LQHUY(02/01/2024)
    public class Department : BaseEntity
    {
        /// <summary>
        /// Mã định danh phòng ban
        /// </summary>
        /// Created By; LQHUY(05/01/2024)
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        /// Created By; LQHUY(05/01/2024)
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        /// Created By; LQHUY(05/01/2024)
        [Required(ErrorMessageResourceName = "DepartmentNameNotEmpty", ErrorMessageResourceType = typeof(DepartmentResourceVN))]
        public string DepartmentName { get; set; }

    }
}
