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
        public async Task<ActionResult<GroupDto>> PostConnector(
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
    }
}
