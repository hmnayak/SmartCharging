using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SmartChargingApi.Models.Api;
using Xunit;

namespace SmartChargingApiIntegrationTests
{
    public class ConnectorControllerTests : IClassFixture<WebApplicationFactory<SmartChargingApi.Startup>>
    {
        public HttpClient _client;

        public ConnectorControllerTests(WebApplicationFactory<SmartChargingApi.Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async void PostGroup_WithGroupCapacity_LessThan_SumOfConnectorsCurrents_ReturnsBadRequest()
        {
            var group = new GroupDto
            {
                Name = "group1",
                CapacityInAmps = 5,
                ChargeStations = new List<ChargeStationDto>
                {
                    new ChargeStationDto
                    {
                        Connectors = new List<ConnectorDto>
                        {
                            new ConnectorDto { MaxCurrentInAmps = 1 },
                            new ConnectorDto { MaxCurrentInAmps = 1 },
                            new ConnectorDto { MaxCurrentInAmps = 3 }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(group);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync("api/Group", data);

            var connectorToAdd = new ConnectorDto { MaxCurrentInAmps = 2 };

            json = JsonConvert.SerializeObject(connectorToAdd);
            data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Group/1/ChargeStation/1/Connector", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
