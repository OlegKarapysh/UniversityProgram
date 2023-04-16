using System.Globalization;
using System.Windows.Controls;

namespace UniversityUI.Validations;

public class MarkValidationRule : ValidationRule
{
    private const float MinMark = 0;
    private const float MaxMark = 100;
    
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult(false, "This field is required!");
        }
        if (!float.TryParse(value.ToString(),NumberStyles.Float, 
                CultureInfo.InvariantCulture, out var mark))
        {
            return new ValidationResult(false, "Mark must be a number!");
        }
        if (mark is < MinMark or > MaxMark)
        {
            return new ValidationResult(false, "Mark must be between 0 and 100!");
        }

        return ValidationResult.ValidResult;
    }
}