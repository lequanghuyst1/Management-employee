using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.MSAttribute
{
    /// <summary>
    /// Class NotQueryExport (đánh dấu những field không xuất ra file excel)
    /// </summary>
    /// Created By: LQHUY(19/01/2024)
    [AttributeUsage(AttributeTargets.Property)]
    public class NotQueryExport : Attribute
    {
    }
}
