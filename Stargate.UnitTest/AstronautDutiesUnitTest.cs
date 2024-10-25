using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPI.Controllers;
using System;

namespace Stargate.UnitTest
{
    //https://dotnetcorecentral.com/blog/sqlite-for-unit-testing-in-net-core/
    public class AstronautDutiesUnitTest
    {
        private readonly ConnectionFactory conn;

        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<PersonController>> _mockLogger;
        private readonly PersonController _personController;

        public AstronautDutiesUnitTest()
        {
            conn = new ConnectionFactory();

            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<PersonController>>();
            _personController = new PersonController(_mockMediator.Object, _mockLogger.Object);
        }

        [Test]
        [TestCase("Jane Doe", 0)]
        [TestCase("John Doe", 1)]
        public async Task GetAstronautDutiesByNameHandlerTest(string name, int dutyCount)
        {
            var handler = new GetAstronautDutiesByNameHandler(conn.CreateContextForSQLite());

            var command = new GetAstronautDutiesByName
            {
                Name = name
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(result.AstronautDuties.Count, dutyCount);
        }

        [Test]
        [TestCase("Jane Doe", "PVT", "Cook")]
        public async Task CreateAstronautDutyHandlerTest(string name, string rank, string dutyTitle)
        {
            var handler = new CreateAstronautDutyHandler(conn.CreateContextForSQLite());

            var command = new CreateAstronautDuty
            {
                Name = name,
                Rank = rank,
                DutyTitle = dutyTitle,
                DutyStartDate = DateTime.Now
            };
            var result = await handler.Handle(command, CancellationToken.None);

            if (result == null)
            {
                Assert.Fail();
            }

            Assert.True(result.Id == 2);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            conn.Dispose();
        }
    }
}