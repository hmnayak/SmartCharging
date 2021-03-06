using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartChargingApi.Db;
using SmartChargingApi.Models.Api;

namespace SmartChargingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
        {
            var groups = await _groupRepository.GetAllGroupsAsync();

            return new OkObjectResult(groups.Select(GroupDto.FromDomain));
        }

        // GET: api/Group/5
        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);

            if (@group == null)
            {
                return NotFound();
            }

            return GroupDto.FromDomain(@group);
        }

        // PUT: api/Group/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{groupId}")]
        public async Task<IActionResult> PutGroup(int groupId, GroupDto groupDto)
        {
            var group = await _groupRepository.GetGroup(groupId);

            if (@group == null)
            {
                return NotFound();
            }

            await _groupRepository.UpdateGroup(groupId, groupDto.ToDomain());

            return NoContent();
        }

        // POST: api/Group
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupDto>> PostGroup([FromBody]GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var group = groupDto.ToDomain();

            await _groupRepository.AddGroup(group);

            return CreatedAtAction(nameof(GetGroup), new { groupId = group.GroupId }, GroupDto.FromDomain(group));
        }

        // DELETE: api/Group/5
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);

            if (@group == null)
            {
                return NotFound();
            }

            await _groupRepository.DeleteGroup(group.GroupId);

            return NoContent();
        }
    }
}
