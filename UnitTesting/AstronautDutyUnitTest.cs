using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.Collections;

namespace StargateAPI.UnitTesting
{
    //https://dotnetcorecentral.com/blog/sqlite-for-unit-testing-in-net-core/
    public class AstronautDutyUnitTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<AstronautDutyController>> _mockLogger;
        private readonly AstronautDutyController _astronautDutyController;

        public AstronautDutyUnitTest()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<AstronautDutyController>>();
            _astronautDutyController = new AstronautDutyController(_mockMediator.Object, _mockLogger.Object);
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetAstronautDutiesByNameTest()
        {
            var response = _astronautDutyController.GetAstronautDutiesByName("John Doe");

            Assert.IsTrue(response.IsCompleted);
        }

        [Test]
        [TestCaseSource("AstronautDuties")]
        public void CreateAstronautDutiesByNameTest()
        {
            var duty = new CreateAstronautDuty{
                Name = "James Zacka",
                Rank = "PV1",
                DutyTitle = "Systems Analyst",
                DutyStartDate = DateTime.Now
            };

            var response = _astronautDutyController.CreateAstronautDuty(duty);

            Assert.IsTrue(response.IsCompleted);
        }

        private static IEnumerable AstronautDuties
        {
            get
            {
                yield return new CreateAstronautDuty
                {
                    Name = "James Zacka",
                    Rank = "PV1",
                    DutyTitle = "Systems Analyst",
                    DutyStartDate = DateTime.Now
                };
            }
        }
    }
}
