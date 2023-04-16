using System;
using System.Globalization;
using System.Windows.Controls;

namespace UniversityUI.Validations;

public class YearValidationRule : ValidationRule
{
    private const int MinYear = 1900;

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult(false, "This field is required!");
        }
        if (!int.TryParse(value.ToString(), out var year))
        {
            return new ValidationResult(false, "Year must be a number!");
        }
        if (year < MinYear || year > DateTime.Now.Year)
        {
            return new ValidationResult(false, 
                $"Year must be between {MinYear} and {DateTime.Now.Year}!");
        }

        return ValidationResult.ValidResult;
    }
}
