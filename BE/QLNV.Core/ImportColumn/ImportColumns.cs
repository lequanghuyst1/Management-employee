using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.ImportColumn
{
    public class ImportColumns
    {
        public Guid ImportColumnId { get; set; }
        public int ColumnPosition { get; set; }
        public string ColumnTitle { get; set; }
        public string ColumnInsert { get; set; }
        public string? ObjectReferenceName { get; set; }
        /// <summary>
        /// Kiểu dữ liệu (0-string, 1-datetime, 2-int, 3-boolean, 4-enum, 5-ReferenceTable, 6-double)
        /// </summary>
        public int ColumnDataType { get; set; }
        public bool IsRequired { get; set; }
    }
}
