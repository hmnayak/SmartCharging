using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartChargingApi.Db;
using SmartChargingApi.Models.Api;
using SmartChargingApi.Validation;

namespace SmartChargingApi.Controllers
{
    [Route("api/Group/{groupId}/[controller]")]
    [ApiController]
    public class ChargeStationController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;

        private readonly IChargeStationRepository _chargeStationRepository;

        public ChargeStationController(IGroupRepository groupRepository, IChargeStationRepository chargeStationRepository)
        {
            _groupRepository = groupRepository;
            _chargeStationRepository = chargeStationRepository;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChargeStationDto>>> GetChargeStations([FromRoute] int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == null)
            {
                return NotFound("Group does not exist");
            }

            var chargeStations = await _chargeStationRepository.GetAllChargeStationsAsync(groupId);

            return new OkObjectResult(chargeStations.Select(ChargeStationDto.FromDomain));
        }

        // GET: api/Group/1/ChargeStation/1
        [HttpGet("{chargeStationId}")]
        public async Task<ActionResult<ChargeStationDto>> GetChargeStation(int chargeStationId)
        {
            var chargeStation = await _chargeStationRepository.GetChargeStation(chargeStationId);

            if (chargeStation == null)
            {
                return NotFound();
            }

            return ChargeStationDto.FromDomain(chargeStation);
        }

        [HttpPost]
        public async Task<ActionResult<ChargeStationDto>> PostChargeStation(
            [FromRoute] int groupId,
            [FromBody] ChargeStationDto chargeStationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var chargeStation = chargeStationDto.ToDomain();

            var group = await _groupRepository.GetGroup(groupId);

            await _chargeStationRepository.AddChargeStation(chargeStation, groupId);

            return CreatedAtAction(nameof(GetChargeStation),
                new { groupId, chargeStationId = chargeStation.ChargeStationId },
                ChargeStationDto.FromDomain(chargeStation));
        }

        [HttpPut("{chargeStationId}")]
        public async Task<IActionResult> PutChargeStation(int chargeStationId, ChargeStationDto chargeStationDto)
        {
            var chargeStation = await _chargeStationRepository.GetChargeStation(chargeStationId);

            if (chargeStation == null)
            {
                return NotFound();
            }

            await _chargeStationRepository.UpdateChargeStation(chargeStationId, chargeStationDto.ToDomain());

            return NoContent();
        }

        [HttpDelete("{chargeStationId}")]
        public async Task<IActionResult> DeleteChargeStation(int chargeStationId)
        {
            var chargeStation = await _chargeStationRepository.GetChargeStation(chargeStationId);

            if (chargeStation == null)
            {
                return NotFound();
            }

            await _chargeStationRepository.DeleteChargeStation(chargeStationId);

            return NoContent();
        }
    }
}
