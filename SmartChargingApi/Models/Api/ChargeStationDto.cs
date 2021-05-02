using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Models.Api
{
    public class ChargeStationDto
    {
        public int ChargeStationId { get; set; }

        public string Name { get; set; }

        [MinLength(1), MaxLength(5)]
        public IReadOnlyCollection<ConnectorDto> Connectors { get; set; }

        public static ChargeStationDto FromDomain(ChargeStation chargeStation) =>
            new()
            {
                ChargeStationId = chargeStation.ChargeStationId,
                Name = chargeStation.Name,
                Connectors = chargeStation.Connectors.Select(ConnectorDto.FromDomain).ToArray()
            };

        public ChargeStation ToDomain() =>
            new(Name, Connectors.Select(c => c.ToDomain()));
    }
}
