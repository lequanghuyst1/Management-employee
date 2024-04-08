using MISA.AMIS.Core.CustomValidation;
using MISA.AMIS.Core.Enums;
using MISA.AMIS.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    /// <summary>
    /// Class Customer
    /// </summary>
    /// Created By: LQHUY(25/12/2023)
    public class Customer : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Id khách hàng
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Id khách hàng")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Id nhóm khách hàng")]
        public Guid? CustomerGroupId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Mã khách hàng")]
        [DateGreatThanToday(ErrorMessageResourceName = "CustomerCodeNotEmpty", ErrorMessageResourceType = typeof(ResourceVN))]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên khách hàng
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Họ và tên")]

        [DateGreatThanToday(ErrorMessageResourceName = "FullNameNotEmpty", ErrorMessageResourceType = typeof(ResourceVN))]
        public string FullName { get; set; }
        
        /// <summary>
        /// Số điện thoại
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại phải là số")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính (0 - nam, 1 - nữ, 2 - khác)
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Giới tính")] 
        public GenderEnum? Gender { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Ngày sinh")]
        [DateGreatThanToday(ErrorMessageResourceName = "DateGreatThanToday", ErrorMessageResourceType = typeof(ResourceVN))]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Dư nợ
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Dư nợ")]
        [MoneyGreatThanZezo(ErrorMessageResourceName = "DateGreatThanToday", ErrorMessageResourceType = typeof(ResourceVN))]
        public Decimal? DebitAmount { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        /// Created By: LQHUY(25/12/2023)
        [DisplayName("Tên công ty")]
        public string? CompanyName { get; set; }

        #endregion
    }
}
