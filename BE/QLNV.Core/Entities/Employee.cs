using MISA.AMIS.Core.CustomValidation;
using MISA.AMIS.Core.Enums;
using MISA.AMIS.Core.MSAttribute;
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
    /// Class Employee
    /// </summary>
    /// Created By: LQHUY(02/01/2024)
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Mã định danh nhân viên
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.EmployeeId), ResourceType = typeof(EmployeeResourceVN))]
        [NotQueryExport]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Required(ErrorMessageResourceName = "EmployeeNotEmpty", ErrorMessageResourceType = typeof(EmployeeResourceVN))]
        [Display(Name = nameof(EmployeeResourceVN.EmployeeCode), ResourceType = typeof(EmployeeResourceVN))]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Required(ErrorMessageResourceName = "FullNameNotEmpty", ErrorMessageResourceType = typeof(EmployeeResourceVN))]
        [Display(Name = nameof(EmployeeResourceVN.FullName), ResourceType = typeof(EmployeeResourceVN))]
        public string Fullname { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [DateGreatThanToday(ErrorMessageResourceName = "DateGreatThanToday", ErrorMessageResourceType = typeof(ResourceVN))]
        [Display(Name = nameof(EmployeeResourceVN.DateOfBirth), ResourceType = typeof(EmployeeResourceVN))]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.Gender), ResourceType = typeof(EmployeeResourceVN))]
        public GenderEnum? Gender { get; set; }


        /// <summary>
        /// Căn cước công dân
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.IdentityNumber), ResourceType = typeof(EmployeeResourceVN))]
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.IdentityDate), ResourceType = typeof(EmployeeResourceVN))]
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.IdentityPlace), ResourceType = typeof(EmployeeResourceVN))]
        public string? IdentityPlace { get; set; }


        /// <summary>
        /// Nơi cấp
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
  
        [Display(Name = nameof(EmployeeResourceVN.Email), ResourceType = typeof(EmployeeResourceVN))]
        public string? Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.PhoneNumber), ResourceType = typeof(EmployeeResourceVN))]
        public string? PhoneNumber { get; set; }

        [Display(Name = nameof(EmployeeResourceVN.PhoneNumberFixed), ResourceType = typeof(EmployeeResourceVN))]
        public string? PhoneNumberFixed { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.Salary), ResourceType = typeof(EmployeeResourceVN))]
        [NotQueryExport]

        public string? Salary { get; set; }

        /// <summary>
        /// Ngày bắt đầu làm việc
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.JoinDate), ResourceType = typeof(EmployeeResourceVN))]
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// Tình trạng công việc 0-đang làm, 1- tạm nghỉ, 2-đã nghỉ việc
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.WorkStatus), ResourceType = typeof(EmployeeResourceVN))]
        [NotQueryExport]

        public int? WorkStatus { get; set; }

        /// <summary>
        /// Địa chỉ nhà ở
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.Address), ResourceType = typeof(EmployeeResourceVN))]
        public string? Address { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.BankAccount), ResourceType = typeof(EmployeeResourceVN))]
        public string? BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.BankName), ResourceType = typeof(EmployeeResourceVN))]
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [Display(Name = nameof(EmployeeResourceVN.BankAddress), ResourceType = typeof(EmployeeResourceVN))]
        public string? BankAddress { get; set; }

        /// <summary>
        /// Mã định danh phòng ban
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        [NotQueryExport]
        [Required(ErrorMessageResourceName = "DepartemntIdNotEmpty", ErrorMessageResourceType = typeof(EmployeeResourceVN))]
        public Guid? DepartmentId { get; set; }

        [Display(Name = nameof(EmployeeResourceVN.DepartmentName), ResourceType = typeof(EmployeeResourceVN))]
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Mã định danh chức vụ
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
       
        [NotQueryExport]
        public Guid? PositionId { get; set; }
        [Display(Name = nameof(EmployeeResourceVN.PositonName), ResourceType = typeof(EmployeeResourceVN))]
        /// <summary>
        /// Tên chức vụ
        /// </summary>
        /// Created By: LQHUY(05/01/2024)
        public string? PositionName { get; set; }

        //public string GenderName
        //{
        //    get
        //    {
        //        switch (Gender)
        //        {
        //            case Enums.GenderEnum.Male:
        //                return ResourceVN.Gender_Male;
        //            case Enums.GenderEnum.Female:
        //                return ResourceVN.Gender_Female;
        //            case Enums.GenderEnum.Other:
        //                return ResourceVN.Gender_Other;
        //            default:
        //                return null;
        //        }

        //    }
        //}
    }
}
