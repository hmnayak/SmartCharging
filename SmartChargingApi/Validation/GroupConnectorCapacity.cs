using System;
using System.Collections.Generic;
using System.Linq;
using SmartChargingApi.Models.Api;
using SmartChargingApi.Models.Domain;
using SmartChargingApi.Models.Extensions;

namespace SmartChargingApi.Validation
{
    public static class GroupConnectorCapacity
    {
        public static bool CanAddConnector(Group @group, Connector connector) =>
            group.GetTotalCurrent() + connector.MaxCurrentInAmps <= group.CapacityInAmps;

        public static IReadOnlyCollection<SuggestedConnector> GetRemovalSuggestions(Group @group, Connector connectorToAdd)
        {
            var availableCapacity = group.CapacityInAmps - group.GetTotalCurrent();
            var requiredCapacity = connectorToAdd.MaxCurrentInAmps - availableCapacity;

            var allConnectorsInGroup = group
                .ChargeStations
                .SelectMany(c => c.Connectors, (cst, cntr) => new SuggestedConnector
                {
                    ChargeStationName = cst.Name,
                    MaxCurrentInAmps = cntr.MaxCurrentInAmps,
                    ConnectorId = cntr.ConnectorId
                });

            var result = new List<SuggestedConnector>();
            var freedCapacity = 0f;

            do
            {
                var nearestConnectors = allConnectorsInGroup
                    .OrderBy(c => Math.Abs(c.MaxCurrentInAmps - (requiredCapacity - freedCapacity)))
                    .ThenBy(c => c.MaxCurrentInAmps > (requiredCapacity - freedCapacity))
                    .ToList();

                var connectorToRemove = nearestConnectors.First();

                result.Add(connectorToRemove);

                freedCapacity += connectorToRemove.MaxCurrentInAmps;

                nearestConnectors.Remove(connectorToRemove);
            } while (freedCapacity < requiredCapacity);

            return result;
        }
    }
}
