using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartChargingApi.Db;
using SmartChargingApi.Models.Api;
using SmartChargingApi.Validation;

namespace SmartChargingApi.Controllers
{
    [Route("api/Group/{groupId}/ChargeStation/{chargeStationId}/[controller]")]
    [ApiController]
    public class ConnectorController : ControllerBase
    {
        private readonly ISmartChargingRepository _smartChargingRepository;

        public ConnectorController(ISmartChargingRepository smartChargingRepository)
        {
            _smartChargingRepository = smartChargingRepository;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectorDto>>> GetConnectors(
            [FromRoute] int groupId,
            [FromRoute] int chargeStationId)
        {
            var group = await _smartChargingRepository.GetGroup(groupId);
            if (group == null)
            {
                return NotFound("Group does not exist");
            }

            var chargeStation = group.ChargeStations.FirstOrDefault(cs => cs.ChargeStationId == chargeStationId);
            if (chargeStation == null)
            {
                return NotFound("Charge station does not exist");
            }

            var connectors = await _smartChargingRepository.GetAllConnectorsAsync(groupId, chargeStationId);

            return new OkObjectResult(connectors.Select(ConnectorDto.FromDomain));
        }

        // GET: api/Group/1/ChargeStation/1/Connector/1
        [HttpGet("{connectorId}")]
        public async Task<ActionResult<ConnectorDto>> GetConnector(int connectorId)
        {
            var connector = await _smartChargingRepository.GetConnector(connectorId);

            if (connector == null)
            {
                return NotFound();
            }

            return ConnectorDto.FromDomain(connector);
        }

        [HttpPost]
        public async Task<ActionResult<ConnectorDto>> PostConnector(
            [FromRoute] int groupId,
            [FromRoute] int chargeStationId,
            [FromBody] ConnectorDto connectorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var connector = connectorDto.ToDomain();

            var group = await _smartChargingRepository.GetGroup(groupId);

            if (!GroupConnectorCapacity.CanAddConnector(group, connector))
            {
                var suggestions = GroupConnectorCapacity.GetRemovalSuggestions(group, connector);

                return BadRequest(new
                {
                    message = "Unable to fit connector in group. Consider suggestions for removal",
                    suggestions
                });
            }

            await _smartChargingRepository.AddConnector(connector, chargeStationId, groupId);

            return CreatedAtAction(nameof(GetConnector),
                new { groupId, chargeStationId, connectorId = connector.ConnectorId },
                ConnectorDto.FromDomain(connector));
        }

        [HttpPut("{connectorId}")]
        public async Task<IActionResult> PutConnector(
            [FromRoute] int groupId,
            [FromRoute] int chargeStationId,
            [FromRoute] int connectorId,
            [FromBody] ConnectorDto connectorDto)
        {
            var connector = await _smartChargingRepository.GetConnector(connectorId);

            if (connector == null)
            {
                return NotFound();
            }

            await _smartChargingRepository.UpdateConnector(connectorId, connectorDto.ToDomain());

            return NoContent();
        }

        [HttpDelete("{connectorId}")]
        public async Task<IActionResult> DeleteConnector(int connectorId)
        {
            var connector = await _smartChargingRepository.GetConnector(connectorId);

            if (connector == null)
            {
                return NotFound();
            }

            await _smartChargingRepository.DeleteConnector(connectorId);

            return NoContent();
        }
    }
}
