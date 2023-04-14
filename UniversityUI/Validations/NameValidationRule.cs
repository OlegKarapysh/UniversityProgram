using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace UniversityUI.Validations;

public class NameValidationRule : ValidationRule
{
    private const string NamePattern = @"^[A-Z][a-zA-Z]{0,29}$";
    
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult(false, "This field is required!");
        }
        if (!Regex.IsMatch(value.ToString(), NamePattern))
        {
            return new ValidationResult(false,
                "Name must contain letters and start with a capital letter!");
        }

        return ValidationResult.ValidResult;
    }
}