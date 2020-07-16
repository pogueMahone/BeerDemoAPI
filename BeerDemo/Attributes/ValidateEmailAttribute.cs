using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeerDemo.Attributes
{
    public class ValidateEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return new RegularExpressionAttribute(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsValid(Convert.ToString(value).Trim());            
        }
    }
}
