using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public class ChargeStationRepository : IChargeStationRepository
    {
        private readonly SmartChargingContext _smartChargingContext;

        private const string GroupNestedCollections = "ChargeStations.Connectors";
        private const string ChargeStationNestedCollections = "Connectors";

        public ChargeStationRepository(SmartChargingContext smartChargingContext)
        {
            _smartChargingContext = smartChargingContext;
        }

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
    }
}
