using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace UniversityUI.Validations;

public class NameValidationRule : ValidationRule
{
    private const string NamePattern = @"^[A-Z][a-zA-Z]{0,49}$";
    
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult(false, "This field is required!");
        }
        if (!Regex.IsMatch(value.ToString(), NamePattern))
        {
            return new ValidationResult(false,
                "Name must contain up to 50 letters and start with a capital letter!");
        }

        return ValidationResult.ValidResult;
    }
}