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
    public class ApiModelBindingValidationTests : IClassFixture<WebApplicationFactory<SmartChargingApi.Startup>>
    {
        public HttpClient _client;

        public ApiModelBindingValidationTests(WebApplicationFactory<SmartChargingApi.Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async void PostGroup_WithGroupCapacity_LessThan_SumOfConnectorsCurrents_ReturnsBadRequest()
        {
            var group = new GroupDto
            {
                Name = "group1",
                CapacityInAmps = 4,
                ChargeStations = new List<ChargeStationDto>
                {
                    new ChargeStationDto
                    {
                        Connectors = new List<ConnectorDto>
                        {
                            new ConnectorDto { MaxCurrentInAmps = 1 }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(group);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Group", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void PostGroup_WithGroupCapacity_EqualTo_Zero_ReturnsBadRequest()
        {
            var group = new GroupDto
            {
                Name = "group1",
                CapacityInAmps = 0,
                ChargeStations = new List<ChargeStationDto>
                {
                    new ChargeStationDto
                    {
                        Connectors = new List<ConnectorDto>
                        {
                            new ConnectorDto { MaxCurrentInAmps = 5 }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(group);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Group", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void PostGroup_WithAConnectorHavingMaxCurrent_EqualTo_Zero_ReturnsBadRequest()
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
                            new ConnectorDto { MaxCurrentInAmps = 0 }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(group);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Group", data);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
