using System.ComponentModel.DataAnnotations;
using SmartChargingApi.Models.Api;
using SmartChargingApi.Models.Extensions;

namespace SmartChargingApi.Validation
{
    public class GroupTotalCapacity : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var groupDto = validationContext.ObjectInstance as GroupDto;

            if (groupDto.ChargeStations != null &&
                groupDto.ChargeStations.Count > 0 &&
                groupDto.CapacityInAmps < groupDto.ToDomain().GetTotalCurrent())
            {
                return new ValidationResult(
                    $"Group capacity must be greater than or equal to sum of connectors current.");
            }

            return ValidationResult.Success;
        }
    }
}
