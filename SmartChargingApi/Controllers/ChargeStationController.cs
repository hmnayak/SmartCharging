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
        private readonly ISmartChargingRepository _smartChargingRepository;

        public ChargeStationController(ISmartChargingRepository smartChargingRepository)
        {
            _smartChargingRepository = smartChargingRepository;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChargeStationDto>>> GetChargeStations([FromRoute] int groupId)
        {
            var group = await _smartChargingRepository.GetGroup(groupId);
            if (group == null)
            {
                return NotFound("Group does not exist");
            }

            var chargeStations = await _smartChargingRepository.GetAllChargeStationsAsync(groupId);

            return new OkObjectResult(chargeStations.Select(ChargeStationDto.FromDomain));
        }

        // GET: api/Group/1/ChargeStation/1
        [HttpGet("{chargeStationId}")]
        public async Task<ActionResult<ChargeStationDto>> GetChargeStation(int chargeStationId)
        {
            var chargeStation = await _smartChargingRepository.GetChargeStation(chargeStationId);

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

            var group = await _smartChargingRepository.GetGroup(groupId);

            await _smartChargingRepository.AddChargeStation(chargeStation, groupId);

            return CreatedAtAction(nameof(GetChargeStation),
                new { groupId, chargeStationId = chargeStation.ChargeStationId },
                ChargeStationDto.FromDomain(chargeStation));
        }

        [HttpPut("{chargeStationId}")]
        public async Task<IActionResult> PutChargeStation(int chargeStationId, ChargeStationDto chargeStationDto)
        {
            var chargeStation = await _smartChargingRepository.GetChargeStation(chargeStationId);

            if (chargeStation == null)
            {
                return NotFound();
            }

            await _smartChargingRepository.UpdateChargeStation(chargeStationId, chargeStationDto.ToDomain());

            return NoContent();
        }

        [HttpDelete("{chargeStationId}")]
        public async Task<IActionResult> DeleteChargeStation(int chargeStationId)
        {
            var chargeStation = await _smartChargingRepository.GetChargeStation(chargeStationId);

            if (chargeStation == null)
            {
                return NotFound();
            }

            await _smartChargingRepository.DeleteChargeStation(chargeStationId);

            return NoContent();
        }
    }
}
