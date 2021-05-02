using System;
using System.Collections.Generic;
using System.Linq;
using SmartChargingApi.Models.Domain;
using SmartChargingApi.Validation;

namespace SmartChargingApi.Models.Api
{

    public class GroupDto
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        [GreaterThanZero]
        [GroupTotalCapacity]
        public float CapacityInAmps { get; set; }

        public IReadOnlyCollection<ChargeStationDto> ChargeStations { get; set; }

        public static GroupDto FromDomain(Group @group) =>
            new()
            {
                GroupId = group.GroupId,
                Name = group.Name,
                CapacityInAmps = group.CapacityInAmps,
                ChargeStations = group.ChargeStations == null ?
                    Array.Empty<ChargeStationDto>() :
                    group.ChargeStations.Select(ChargeStationDto.FromDomain).ToArray()
            };

        public Group ToDomain() =>
            new(Name, CapacityInAmps, ChargeStations?.Select(c => c.ToDomain()));
    }
}
