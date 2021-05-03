using System.Collections.Generic;
using System.Threading.Tasks;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public interface ISmartChargingRepository
    {
        Task<bool> AddGroup(Group group);
        Task<IReadOnlyCollection<Group>> GetAllGroupsAsync();
        Task<Group> GetGroup(int groupId);
        Task<bool> UpdateGroup(int groupId, Group group);
        Task<bool> DeleteGroup(int groupId);

        Task<bool> AddChargeStation(ChargeStation chargeStation, int groupId);
        Task<IReadOnlyCollection<ChargeStation>> GetAllChargeStationsAsync(int groupId);
        Task<ChargeStation> GetChargeStation(int chargeStationId);
        Task<bool> UpdateChargeStation(int chargeStationId, ChargeStation chargeStation);
        Task<bool> DeleteChargeStation(int chargeStationId);

        Task<IReadOnlyCollection<Connector>> GetAllConnectorsAsync(int groupId, int chargeStationId);
        Task<Connector> GetConnector(int connectorId);
        Task<bool> AddConnector(Connector connector, int chargeStationId, int groupId);
        Task<bool> UpdateConnector(int connectorId, Connector connector);
        Task<bool> DeleteConnector(int connectorId);
    }
}
