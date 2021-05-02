using System.Collections.Generic;
using System.Threading.Tasks;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public interface ISmartChargingRepository
    {
        Task<IReadOnlyCollection<Group>> GetAllGroupAsync();
        Task<Group> GetGroup(int id);
        Task<bool> UpdateGroup(int groupId, Group group);
        Task<bool> AddGroup(Group group);
        Task<bool> DeleteGroup(int id);

        Task<Connector> GetConnector(int id);
        Task<bool> AddConnector(Connector connector, int chargeStationId, int groupId);
    }
}
