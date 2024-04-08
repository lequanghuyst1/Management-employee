using MISA.AMIS.Core.ImportColumn;
using MISA.AMIS.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core
{
    public static class Common
    {
        public static List<ImportColumns> EmloyeeImportColumns = new List<ImportColumns>()
        {
            new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 1, ColumnTitle = "STT", ColumnInsert = "", ObjectReferenceName = null, ColumnDataType = 0 , IsRequired = true },
            new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 2, ColumnTitle = "Mã nhân viên", ColumnInsert = "EmployeeCode", ObjectReferenceName = null, ColumnDataType = 0 , IsRequired = true },
            new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 3, ColumnTitle = "Họ tên", ColumnInsert = "Fullname", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = true },
            new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 4, ColumnTitle = "Ngày sinh", ColumnInsert = "DateOfBirth", ObjectReferenceName =  null, ColumnDataType = 1 , IsRequired = false },
            new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 5, ColumnTitle = "Giới tính", ColumnInsert = "Gender", ObjectReferenceName =  null, ColumnDataType = 4, IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 6, ColumnTitle = "Số CMND", ColumnInsert = "IdentityNumber", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 7, ColumnTitle = "Ngày cấp", ColumnInsert = "IdentityDate", ObjectReferenceName =  null, ColumnDataType = 1 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 8, ColumnTitle = "Nơi cấp", ColumnInsert = "IdentityPlace", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 9, ColumnTitle = "Email", ColumnInsert = "Email", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 10, ColumnTitle = "SĐT di động", ColumnInsert = "PhoneNumber", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 11, ColumnTitle = "SĐT cố định", ColumnInsert = "PhoneNumberFixed", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 12, ColumnTitle = "Lương", ColumnInsert = "Salary", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 13, ColumnTitle = "Địa chỉ", ColumnInsert = "Address", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 14, ColumnTitle = "TK ngân hàng", ColumnInsert = "BankAccount", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 15, ColumnTitle = "Tên ngân hàng", ColumnInsert = "BankName", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 16, ColumnTitle = "Chi nhánh ngân hàng", ColumnInsert = "BankAddress", ObjectReferenceName =  null, ColumnDataType = 0 , IsRequired = false },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 17, ColumnTitle = "Đơn vị", ColumnInsert = "DepartmentId", ObjectReferenceName =  "Department", ColumnDataType = 5 , IsRequired = true },
             new ImportColumns{ ImportColumnId = Guid.NewGuid(), ColumnPosition = 18, ColumnTitle = "Vị trí", ColumnInsert = "PositionId", ObjectReferenceName =  "Position", ColumnDataType = 5 , IsRequired = false },
        };


        /// <summary>
        /// Xử lý dữ liệu nếu value có type là GenderEnum
        /// </summary>
        /// <param name="gender">giá trị enum của gender</param>
        /// <returns>Tên của gender</returns>
        /// Created By: LQHUY(19/01/2024)
        public static string ProccesValueGender(object gender)
        {
            switch (gender)
            {
                case Enums.GenderEnum.Male:
                    return ResourceVN.Gender_Male;
                case Enums.GenderEnum.Female:
                    return ResourceVN.Gender_Female;
                case Enums.GenderEnum.Other:
                    return ResourceVN.Gender_Other;
                default:
                    return "";
            }

        }
    }
}
