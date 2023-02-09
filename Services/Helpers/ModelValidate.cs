using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers;

public static class ModelValidate
{
    public static void  Validate(Object obj)
    {
        ValidationContext validationContext= new ValidationContext(obj);
        List<ValidationResult> validationResults= new List<ValidationResult>();
        bool IsValid=Validator.TryValidateObject(obj, validationContext, validationResults,true);
        if (!IsValid)
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}
