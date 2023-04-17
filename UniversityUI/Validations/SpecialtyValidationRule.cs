using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace UniversityUI.Validations;

public class SpecialtyValidationRule : ValidationRule
{
    private const string SpecialtyCodePattern = @"^[a-zA-Z]{2,10}$";
    
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult(false, "This field is required!");
        }
        if (!Regex.IsMatch(value.ToString(), SpecialtyCodePattern))
        {
            return new ValidationResult(false,
                "Must contain 2 - 10 letters!");
        }

        return ValidationResult.ValidResult;
    }
}