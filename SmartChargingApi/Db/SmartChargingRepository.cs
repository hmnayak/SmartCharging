using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public class SmartChargingRepository : ISmartChargingRepository
    {
        private readonly SmartChargingContext _smartChargingContext;

        private const string GroupNestedCollections = "ChargeStations.Connectors";
        private const string ChargeStationNestedCollections = "Connectors";

        public SmartChargingRepository(SmartChargingContext smartChargingContext)
        {
            _smartChargingContext = smartChargingContext;
        }

        #region Group

        public async Task<bool> AddGroup(Group group)
        {
            _smartChargingContext.Groups.Add(group);
            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        public async Task<IReadOnlyCollection<Group>> GetAllGroupsAsync() =>
            await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .ToListAsync();

        public async Task<Group> GetGroup(int id) =>
            await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstOrDefaultAsync(g => g.GroupId == id);

        public async Task<bool> UpdateGroup(int groupId, Group group)
        {
            var groupEntry = await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstAsync(g => g.GroupId == groupId);

            groupEntry.Name = group.Name;
            groupEntry.CapacityInAmps = group.CapacityInAmps;
            groupEntry.ChargeStations = group.ChargeStations?.ToList();

            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        public async Task<bool> DeleteGroup(int id)
        {
            var @group = await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstAsync(g => g.GroupId == id);

            _smartChargingContext.Groups.Remove(@group);
            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        #endregion

        #region Charge Station

        public async Task<bool> AddChargeStation(ChargeStation chargeStation, int groupId)
        {
            var group = await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstAsync(g => g.GroupId == groupId);

            group.ChargeStations.Add(chargeStation);

            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        public async Task<IReadOnlyCollection<ChargeStation>> GetAllChargeStationsAsync(int groupId) =>
            await _smartChargingContext
                .Groups
                .Include(GroupNestedCollections)
                .FirstAsync(g => g.GroupId == groupId)
                .ContinueWith(a => a.Result.ChargeStations.ToArray());

        public async Task<ChargeStation> GetChargeStation(int chargeStationId) =>
            await _smartChargingContext
                .ChargeStations
                .FirstOrDefaultAsync(c => c.ChargeStationId == chargeStationId);

        public async Task<bool> UpdateChargeStation(int chargeStationId, ChargeStation chargeStation)
        {
            var chargeStationEntry = await _smartChargingContext
                .ChargeStations
                .Include(ChargeStationNestedCollections)
                .FirstAsync(cs => cs.ChargeStationId == chargeStationId);

            chargeStationEntry.Name = chargeStation.Name;
            chargeStationEntry.Connectors = chargeStation.Connectors;

            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        public async Task<bool> DeleteChargeStation(int chargeStationId)
        {
            var chargeStation = await _smartChargingContext
                .ChargeStations
                .Include(ChargeStationNestedCollections)
                .FirstAsync(cs => cs.ChargeStationId == chargeStationId);

            _smartChargingContext.ChargeStations.Remove(chargeStation);
            return await _smartChargingContext.SaveChangesAsync(true) == 1;
        }

        #endregion

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
