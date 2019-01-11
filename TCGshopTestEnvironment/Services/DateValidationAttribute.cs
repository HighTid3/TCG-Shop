using System;
using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.Services
{
    public class DateValidationAttribute : RangeAttribute
    {
        public DateValidationAttribute(string maximumValue) : 
            base(typeof(DateTime), DateTime.Now.ToShortDateString(), maximumValue)
        {

        }
    }
}
