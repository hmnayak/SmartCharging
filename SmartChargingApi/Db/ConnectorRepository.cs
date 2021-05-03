using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public class ConnectorRepository : IConnectorRepository
    {
        private readonly SmartChargingContext _smartChargingContext;

        private const string GroupNestedCollections = "ChargeStations.Connectors";

        public ConnectorRepository(SmartChargingContext smartChargingContext)
        {
            _smartChargingContext = smartChargingContext;
        }

        #region Connector

        public async Task<bool> AddConnector(Connector connector, int chargeStationId, int groupId)
        {
            var group = await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstAsync(g => g.GroupId == groupId);

            var chargeStation = group.ChargeStations.First(cs => cs.ChargeStationId == chargeStationId);

            chargeStation.Connectors.Add(connector);

            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        public async Task<IReadOnlyCollection<Connector>> GetAllConnectorsAsync(int groupId, int chargeStationId) =>
            await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstAsync(g => g.GroupId == groupId)
                .ContinueWith(a => a.Result.ChargeStations.First(cs => cs.ChargeStationId == chargeStationId))
                .ContinueWith(a => a.Result.Connectors.ToArray());


        public async Task<Connector> GetConnector(int id) =>
            await _smartChargingContext.Connectors.FirstOrDefaultAsync(c => c.ConnectorId == id);

        public async Task<bool> UpdateConnector(int connectorId, Connector connector)
        {
            var connectorEntry = await _smartChargingContext
                .Connectors
                .FirstAsync(c => c.ConnectorId == connectorId);

            connectorEntry.MaxCurrentInAmps = connector.MaxCurrentInAmps;

            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        public async Task<bool> DeleteConnector(int connectorId)
        {
            var connector = await _smartChargingContext
                .Connectors
                .FirstAsync(c => c.ConnectorId == connectorId);

            _smartChargingContext.Connectors.Remove(connector);
            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }
    }

    #endregion
}

