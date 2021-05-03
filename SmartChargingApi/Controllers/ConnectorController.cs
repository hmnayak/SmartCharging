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
        private readonly IGroupRepository _groupRepository;

        private readonly IConnectorRepository _connectorRepository;


        public ConnectorController(IGroupRepository groupRepository, IConnectorRepository connectorRepository)
        {
            _groupRepository = groupRepository;
            _connectorRepository = connectorRepository;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectorDto>>> GetConnectors(
            [FromRoute] int groupId,
            [FromRoute] int chargeStationId)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == null)
            {
                return NotFound("Group does not exist");
            }

            var chargeStation = group.ChargeStations.FirstOrDefault(cs => cs.ChargeStationId == chargeStationId);
            if (chargeStation == null)
            {
                return NotFound("Charge station does not exist");
            }

            var connectors = await _connectorRepository.GetAllConnectorsAsync(groupId, chargeStationId);

            return new OkObjectResult(connectors.Select(ConnectorDto.FromDomain));
        }

        // GET: api/Group/1/ChargeStation/1/Connector/1
        [HttpGet("{connectorId}")]
        public async Task<ActionResult<ConnectorDto>> GetConnector(int connectorId)
        {
            var connector = await _connectorRepository.GetConnector(connectorId);

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

            var group = await _groupRepository.GetGroup(groupId);

            if (group.ChargeStations.First(cs => cs.ChargeStationId ==chargeStationId).Connectors.Count == 5)
            {
                return BadRequest("Station limit of five connectors has been reached.");
            }

            if (!GroupConnectorCapacity.CanAddConnector(group, connector))
            {
                var suggestions = GroupConnectorCapacity.GetRemovalSuggestions(group, connector);

                return BadRequest(new
                {
                    message = "Unable to fit connector in group. Consider suggestions for removal",
                    suggestions
                });
            }

            await _connectorRepository.AddConnector(connector, chargeStationId, groupId);

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
            var connector = await _connectorRepository.GetConnector(connectorId);

            if (connector == null)
            {
                return NotFound();
            }

            await _connectorRepository.UpdateConnector(connectorId, connectorDto.ToDomain());

            return NoContent();
        }

        [HttpDelete("{connectorId}")]
        public async Task<IActionResult> DeleteConnector(int connectorId)
        {
            var connector = await _connectorRepository.GetConnector(connectorId);

            if (connector == null)
            {
                return NotFound();
            }

            await _connectorRepository.DeleteConnector(connectorId);

            return NoContent();
        }
    }
}
