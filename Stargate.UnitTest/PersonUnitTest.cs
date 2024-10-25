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
using System.Xml.Linq;

namespace Stargate.UnitTest
{
    //https://dotnetcorecentral.com/blog/sqlite-for-unit-testing-in-net-core/
    public class PersonUnitTest
    {
        //private readonly SqliteConnection _connection;
        //private StargateContext _context;
        private readonly ConnectionFactory conn;

        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<PersonController>> _mockLogger;
        private readonly PersonController _personController;

        public PersonUnitTest()
        {
            //_connection = new SqliteConnection("DataSource=:memory:");
            conn = new ConnectionFactory();

            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<PersonController>>();
            _personController = new PersonController(_mockMediator.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetPeopleHandlerTest()
        {
            var handler = new GetPeopleHandler(conn.CreateContextForSQLite());

            var command = new GetPeople();
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.People.Count > 0);
        }

        [Test]
        [TestCase("Jane Doe")]
        [TestCase("John Doe")]
        public async Task GetPersonByNameHandlerTest(string name)
        {
            var handler = new GetPersonByNameHandler(conn.CreateContextForSQLite());

            var command = new GetPersonByName
            {
                Name = name
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Person.Name == name);
        }

        [Test]
        [TestCase("James Zacka")]
        public async Task CreatePersonHandlerTest(string name)
        {
            var handler = new CreatePersonHandler(conn.CreateContextForSQLite());

            var command = new CreatePerson
            {
                Name = name
            };
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Id == 3);
        }

        [Test]
        public void GetPeopleTest()
        {
            var response = _personController.GetPeople();

            Assert.IsTrue(response.IsCompleted);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            conn.Dispose();
        }
    }
}