using System.Collections.Generic;
using System.Threading.Tasks;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public interface IConnectorRepository
    {
        Task<IReadOnlyCollection<Connector>> GetAllConnectorsAsync(int groupId, int chargeStationId);
        Task<Connector> GetConnector(int connectorId);
        Task<bool> AddConnector(Connector connector, int chargeStationId, int groupId);
        Task<bool> UpdateConnector(int connectorId, Connector connector);
        Task<bool> DeleteConnector(int connectorId);
    }
}
