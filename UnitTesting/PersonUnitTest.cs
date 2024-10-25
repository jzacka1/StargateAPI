using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using StargateAPI.Migrations;

namespace StargateAPI.UnitTesting
{
    //https://dotnetcorecentral.com/blog/sqlite-for-unit-testing-in-net-core/
    public class PersonUnitTest
    {
        private readonly SqliteConnection _connection;
        private readonly StargateContext _context;

        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<PersonController>> _mockLogger;
        private readonly PersonController _personController;

        public PersonUnitTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");

            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<PersonController>>();
            _personController = new PersonController(_mockMediator.Object, _mockLogger.Object);
        }

        [SetUp]
        public void Setup()
        {
            DbConnection();
        }

        [Test]
        public void GetPeopleTest()
        {
            var response = _personController.GetPeople();

            Assert.IsTrue(response.IsCompleted);
        }

        [Test]
        [TestCase("John Doe")]
        public void GetPersonByNameTest(string name)
        {
            var response = _personController.GetPersonByName(name);

            Assert.IsTrue(response.IsCompleted);
        }

        [Test]
        [TestCase("James Zacka")]
        public void CreatePersonTest(string name)
        {
            var response = _personController.CreatePerson(name);

            Assert.IsTrue(response.IsCompleted);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _connection.Close();
        }

        private void DbConnection()
        {
            _connection.Open();

            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new StargateContext(options))
            {
                context.Database.EnsureCreated();

                context.People.AddRange(new Person
                {
                    Id = 1,
                    Name = "John Doe"
                },
                    new Person
                    {
                        Id = 2,
                        Name = "Jane Doe"
                    }
                );
                context.SaveChanges();
            }

            using (var context = new StargateContext(options))
            {
                var list = context.People.ToList();
                context.AddRange(new Person
                {
                    Id = 1,
                    Name = "John Doe"
                },
                    new Person
                    {
                        Id = 2,
                        Name = "Jane Doe"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
