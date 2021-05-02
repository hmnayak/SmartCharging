using System.Linq;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Models.Extensions
{
    public static class GroupTotalCurrentExtension
    {
        public static float GetTotalCurrent(this Group group) =>
            group
                .ChargeStations
                .SelectMany(cs => cs.Connectors)
                .Aggregate(0f, (sum, next) => sum + next.MaxCurrentInAmps);
    }
}
