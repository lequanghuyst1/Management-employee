using MISA.AMIS.Core.Interfaces.Excels;
using MISA.AMIS.Core.Resources;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.Core.MSAttribute;
using MISA.AMIS.Core.Enums;

namespace MISA.AMIS.Core.Excels
{
    public class ExportExcelService<T> : IExportExcelService<T>
    {
        public byte[] ExportExcelAsync(IEnumerable<T> data)
        {
            //danh sách property export vào file
            var properties = typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(NotQueryExport)) == false);

            //danh sách các cột export vào excel
            var columns = new List<string>();

            foreach (var property in properties)
            {
                var attri = property.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (attri.Length > 0)
                {
                    if (attri.FirstOrDefault() is DisplayAttribute displayAttribute)
                    {
                        var displayName = displayAttribute.GetName();
                        if (displayName != null)
                        {
                            columns.Add(displayName);
                        }
                    }

                }
            }

            var stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                // đặt tên người tạo file
                excelPackage.Workbook.Properties.Author = "LQHUY";

                var worksheet = excelPackage.Workbook.Worksheets.Add(string.Format(ResourceVN.ObjectList, EmployeeResourceVN.Employee.ToLower()));

                // set style mặc định cho toàn bộ file
                worksheet.Cells.Style.Font.Size = 14;
                worksheet.Cells.Style.Font.Name = "Times New Roman";

                // lấy ra số lượng cột cần dùng dựa vào số lượng header
                var countColHeader = columns.Count();

                // gán giá trị cho cell vừa merge
                worksheet.Cells[1, 1].Value = string.Format(ResourceVN.ObjectList, EmployeeResourceVN.Employee.ToLower());
                // merge các column lại từ column 1 đến số column header và set style
                worksheet.Cells[1, 1, 2, countColHeader + 1].Merge = true;
                worksheet.Cells[1, 1, 2, countColHeader + 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 2, countColHeader + 1].Style.Font.Size = 24;
                worksheet.Cells[1, 1, 2, countColHeader + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var headerCol = 2;
                var headerRow = 3;
                worksheet.Cells[headerRow, 1].Value = "STT";

                //tạo các header từ column header đã tạo từ bên trên
                foreach (var item in columns)
                {
                    //gán giá trị
                    worksheet.Cells[headerRow, headerCol].Value = item.ToString();
                    headerCol++;
                }

                //set style cho header
                var cells = worksheet.Cells[headerRow, 1, headerRow, countColHeader + 1];
                //set màu thành gray
                var fill = cells.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(System.Drawing.Color.Gold);

                //căn chỉnh các border
                var border = cells.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                    border.Left.Style =
                    border.Right.Style = ExcelBorderStyle.Thin;

                SetCellValueEnterFileExcel(data, worksheet, properties);

                excelPackage.Save();
                //chuyển đổi nội dung của đối tượng stream thành một mảng byte.
                var bytes = stream.ToArray();
                return bytes;
            }

        }

        /// <summary>
        /// Set giá trị cho từng ô trong worksheet
        /// </summary>
        /// <param name="data">danh sách các bản ghi lấy từ database</param>
        /// <param name="worksheet">worksheet để làm việc</param>
        /// <param name="properties">danh sách các property</param>
        /// Created By: LQHUY(19/01/2024)
        private void SetCellValueEnterFileExcel(IEnumerable<T> data, ExcelWorksheet worksheet, IEnumerable<PropertyInfo> properties)
        {
            var currentRow = 4;
            var colSTT = 1;
            var totalCol = properties.ToList().Count;
            //gán giá trị cho từng dòng và cột
            foreach (var item in data)
            {
                worksheet.Cells[currentRow, 1].Value = colSTT;
                var currentCol = 2;

                foreach (var property in properties)
                {
                  
                    var cellValue = property.GetValue(item);
                    if (cellValue != null && property.PropertyType == typeof(DateTime?))
                    {
                        cellValue = String.Format("{0:dd/MM/yyyy}", cellValue); 
                    }
                    if (cellValue != null && property.PropertyType == typeof(GenderEnum?))
                    {
                        cellValue = Common.ProccesValueGender(cellValue);
                    }
                    //gán giá trị cho từng cell                      
                    worksheet.Cells[currentRow, currentCol].Value = cellValue;
                    currentCol++;
                }

                //set style cho từng ô giá trị
                worksheet.Cells[currentRow, 1, currentRow, totalCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells.AutoFitColumns();
                worksheet.Rows[currentRow].Height = 20;

                colSTT++;
                currentRow++;
            }
        }

        
    }
}
