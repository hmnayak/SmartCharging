using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartChargingApi.Controllers;
using SmartChargingApi.Db;
using SmartChargingApi.Models.Api;
using SmartChargingApi.Models.Domain;
using Xunit;

namespace SmartChargingApiUnitTests
{
    public class GroupControllerTests
    {
        private readonly Mock<IGroupRepository> _mockRepository = new Mock<IGroupRepository>();

        [Fact]
        public async void GetGroups_ReturnsOkResult_IncludingAllGroups()
        {
            _mockRepository
                .Setup(repo => repo.GetAllGroupsAsync())
                .ReturnsAsync(new List<Group>
                {
                    new Group { Name = "group1" },
                    new Group { Name = "group2" }
                });

            var controller = new GroupController(_mockRepository.Object);

            var response = await controller.GetGroups();

            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            
            Assert.Equal(2, (okResult.Value as IEnumerable<GroupDto>).Count());
        }

        [Fact]
        public async void PostGroup_ReturnsCreatedResult_IncludingCreatedGroup()
        {
            var group = new GroupDto
            {
                Name = "group1",
                CapacityInAmps = 3,
                ChargeStations = new List<ChargeStationDto>
                {
                    new ChargeStationDto
                    {
                        Connectors = new List<ConnectorDto>
                        {
                            new ConnectorDto { MaxCurrentInAmps = 4 }
                        }
                    }
                }
            };

            var controller = new GroupController(_mockRepository.Object);

            var response = await controller.PostGroup(group);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(response.Result);

            var createdGroupDto = Assert.IsType<GroupDto>(createdAtActionResult.Value);

            Assert.Equal(createdGroupDto.Name, group.Name);
        }
    }
}
