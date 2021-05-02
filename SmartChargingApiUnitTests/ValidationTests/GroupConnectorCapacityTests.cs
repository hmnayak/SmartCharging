using System.Collections.Generic;
using System.Linq;
using SmartChargingApi.Models.Domain;
using SmartChargingApi.Validation;
using Xunit;

namespace SmartChargingApiUnitTests.ValidationTests
{
    public class GroupConnectorCapacityTests
    {
        [Fact]
        public void CanAddConnnector_ReturnsFalse_WhenGroupCapacityIsExceeded()
        {
            var group = new Group
            {
                Name = "group1",
                CapacityInAmps = 4,
                ChargeStations = new List<ChargeStation>
                {
                    new ChargeStation
                    {
                        Connectors = new List<Connector>
                        {
                            new Connector { MaxCurrentInAmps = 1 }
                        }
                    }
                }
            };

            var connectorToAdd = new Connector { MaxCurrentInAmps = 4 };

            var canAddConnector = GroupConnectorCapacity.CanAddConnector(group, connectorToAdd);

            Assert.False(canAddConnector);
        }

        [Fact]
        public void GetRemovalSuggestions_WhenGroupLacksCapacityForNewConnector_ReturnsLeastConnectorsToRemove()
        {
            var group = new Group
            {
                Name = "group1",
                CapacityInAmps = 4,
                ChargeStations = new List<ChargeStation>
                {
                    new ChargeStation
                    {
                        Connectors = new List<Connector>
                        {
                            new Connector { ConnectorId = 1, MaxCurrentInAmps = 1 },
                            new Connector { ConnectorId = 2, MaxCurrentInAmps = 1 },
                            new Connector { ConnectorId = 3, MaxCurrentInAmps = 2 }
                        }
                    }
                }
            };

            var suggestedConnectors = GroupConnectorCapacity.GetRemovalSuggestions(group, new Connector { MaxCurrentInAmps = 2 });

            Assert.Equal(1, suggestedConnectors.Count);

            Assert.Equal(3, suggestedConnectors.First().ConnectorId);
        }
    }
}
