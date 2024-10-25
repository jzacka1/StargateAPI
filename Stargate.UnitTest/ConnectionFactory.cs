using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stargate.UnitTest
{
    public class ConnectionFactory : IDisposable
    {

        #region IDisposable Support  
        private bool disposedValue = false;

        public StargateContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<StargateContext>().UseSqlite(connection).Options;

            var context = new StargateContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        private StargateContext AddData()
        {
            var factory = new ConnectionFactory();

            var context = factory.CreateContextForSQLite();

            context.People.AddRange(
                new Person
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

            context.AstronautDetails.Add(
                new AstronautDetail
                {
                    Id = 1,
                    PersonId = 1,
                    CurrentRank = "1LT",
                    CurrentDutyTitle = "Commander",
                    CareerStartDate = DateTime.Now
                }
            );

            context.AstronautDuties.Add(
                new AstronautDuty
                {
                    Id = 1,
                    PersonId = 1,
                    DutyStartDate = DateTime.Now,
                    DutyTitle = "Commander",
                    Rank = "1LT"
                }
            );

            return context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
