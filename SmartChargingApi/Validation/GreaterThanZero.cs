using System.ComponentModel.DataAnnotations;

namespace SmartChargingApi.Validation
{
    public class GreaterThanZero: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null &&
                float.TryParse(value.ToString(), out float i) &&
                i > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} should be greater than zero");
        }
    }
}
