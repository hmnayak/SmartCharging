using SmartChargingApi.Models.Domain;
using SmartChargingApi.Validation;

namespace SmartChargingApi.Models.Api
{
    public class ConnectorDto
    {
        public int ConnectorId { get; set; }

        [GreaterThanZero]
        public float MaxCurrentInAmps { get; set; }

        public static ConnectorDto FromDomain(Connector connector) =>
            new()
            {
                ConnectorId = connector.ConnectorId,
                MaxCurrentInAmps = connector.MaxCurrentInAmps
            };

        public Connector ToDomain() =>
            new() { MaxCurrentInAmps = MaxCurrentInAmps };
    }
}
