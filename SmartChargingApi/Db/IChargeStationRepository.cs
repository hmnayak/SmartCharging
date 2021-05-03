using System.Collections.Generic;
using System.Threading.Tasks;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public interface IChargeStationRepository
    {
        Task<bool> AddChargeStation(ChargeStation chargeStation, int groupId);
        Task<IReadOnlyCollection<ChargeStation>> GetAllChargeStationsAsync(int groupId);
        Task<ChargeStation> GetChargeStation(int chargeStationId);
        Task<bool> UpdateChargeStation(int chargeStationId, ChargeStation chargeStation);
        Task<bool> DeleteChargeStation(int chargeStationId);
    }
}
