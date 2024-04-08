using MISA.AMIS.Core.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.Core.Enums;

namespace MISA.AMIS.Core.DTOs.CustomerDTO
{
    /// <summary>
    /// CustomerExcelDto
    /// </summary>
    /// Created by: LQHUY(04/01/2024)
    public class CustomerExcelDto
    {
        #region Properties
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Mã khách hàng")]
        [MaxLength(20, ErrorMessage = "Mã khách hàng không được quá 20 ký tự")]
        [Required(ErrorMessage = "Mã khách hàng không được phép để trống.")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên khách hàng
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Họ và tên")]

        [Required(ErrorMessage = "Họ và tên không được phép để trống")]
        public string FullName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại phải là số")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính (0 - nam, 1 - nữ, 2 - khác)
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Giới tính")]
        public GenderEnum? Gender { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Ngày sinh")]
        [DateGreatThanToday(ErrorMessage = "Ngày sinh không được lớn hơn ngày hiện tại")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Dư nợ
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Dư nợ")]
        [MoneyGreatThanZezo(ErrorMessage = "Số tiền phải lớn hơn 0")]
        public Decimal? DebitAmount { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        /// Created By: LQHUY(04/01/2024)
        [DisplayName("Tên công ty")]
        public string? CompanyName { get; set; } 
        #endregion
    }
}
