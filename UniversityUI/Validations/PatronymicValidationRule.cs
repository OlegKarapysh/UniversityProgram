using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace UniversityUI.Validations;

public class PatronymicValidationRule : ValidationRule
{
    private const string PatronymicPattern = @"^(?:[A-Z][a-zA-Z]{0,29})?$";
    
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (!Regex.IsMatch(value?.ToString() ?? string.Empty, PatronymicPattern))
        {
            return new ValidationResult(false,
                "Name must contain letters and start with a capital letter!");
        }

        return ValidationResult.ValidResult;
    }
}