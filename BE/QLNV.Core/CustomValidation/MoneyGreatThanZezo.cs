using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.CustomValidation
{
    /// <summary>
    /// Class DateGreatThanToday
    /// </summary>
    /// Created By: LQHUY(03/01/2024)
    public class MoneyGreatThanZezo : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            double money;
            if (double.TryParse(value.ToString(), out money))
            {
                if (money < 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
