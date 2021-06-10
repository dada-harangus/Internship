using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Dto
{
    public class DateAttribute : ValidationAttribute
    {



        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!DateTime.TryParse(value.ToString(), out DateTime result))
            {
                return new ValidationResult(String.Format("DateTime not in wright format - Fail at {0}", validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

    }
}

