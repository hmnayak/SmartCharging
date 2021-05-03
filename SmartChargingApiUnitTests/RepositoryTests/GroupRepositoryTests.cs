using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using SmartChargingApi.Db;
using SmartChargingApi.Models.Domain;
using Xunit;

namespace SmartChargingApiUnitTests
{
    public class GroupRepositoryTests
    {
        [Fact]
        public async void AddGroup_Saves_Group_Via_Context()
        {
            var mockDbSet = new Mock<DbSet<Group>>();

            var mockContext = new Mock<SmartChargingContext>();
            mockContext.Setup(m => m.Groups).Returns(mockDbSet.Object);

            var repository = new GroupRepository(mockContext.Object);
            await repository.AddGroup(new Group("group123", 5, Array.Empty<ChargeStation>()));

            mockDbSet.Verify(m => m.Add(It.Is<Group>(g => g.Name == "group123")), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void GetAllGroups_GetsGroupsInDb()
        {
            using var dbContext = new SmartChargingContext();
            var repository = new GroupRepository(dbContext);

            await repository.AddGroup(new Group { Name = "group1" });

            await repository.AddGroup(new Group { Name = "group2" });

            var groups = await repository.GetAllGroupsAsync();

            Assert.Equal(2, groups.Count);
        }

    }
}
