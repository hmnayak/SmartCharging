using System.Collections.Generic;
using System.Threading.Tasks;
using SmartChargingApi.Models.Domain;

namespace SmartChargingApi.Db
{
    public interface IGroupRepository
    {
        Task<bool> AddGroup(Group group);
        Task<IReadOnlyCollection<Group>> GetAllGroupsAsync();
        Task<Group> GetGroup(int groupId);
        Task<bool> UpdateGroup(int groupId, Group group);
        Task<bool> DeleteGroup(int groupId);
    }
}
