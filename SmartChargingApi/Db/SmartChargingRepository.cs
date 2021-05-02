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

        public SmartChargingRepository(SmartChargingContext smartChargingContext)
        {
            _smartChargingContext = smartChargingContext;
        }

        public async Task<IReadOnlyCollection<Group>> GetAllGroupAsync() =>
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

        public async Task<bool> AddGroup(Group group)
        {
            _smartChargingContext.Groups.Add(group);
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

        public async Task<Connector> GetConnector(int id)
            => await _smartChargingContext
                .Connectors
                .FirstAsync(c => c.ConnectorId == id);

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
    }
    
}
