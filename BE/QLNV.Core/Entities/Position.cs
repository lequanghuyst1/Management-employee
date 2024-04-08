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
    /// Class Position
    /// </summary>
    /// Created By: LQHUY(02/01/2024)
    public class Position : BaseEntity
    {
        /// <summary>
        /// Mã định danh vị trí
        /// </summary>
        /// Created By; LQHUY(05/01/2024)
        public Guid PositionId { get; set; }


        /// <summary>
        /// Tên chức vụ
        /// </summary>
        /// Created By; LQHUY(05/01/2024)
        [Required(ErrorMessageResourceName = "PositionNameNotEmpty", ErrorMessageResourceType = typeof(PositonResourceVN))]
        public string PositionName { get; set; }
    }
}
