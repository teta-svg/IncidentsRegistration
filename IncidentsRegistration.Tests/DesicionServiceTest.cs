using FluentAssertions;
using IncidentsRegistration.Data;
using IncidentsRegistration.Models;
using IncidentsRegistration.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentsRegistration.Tests
{
    public class DecisionServiceTests
    {
        private class TestIncidentsDbContext : IncidentsDbContext
        {
            public TestIncidentsDbContext(DbContextOptions<IncidentsDbContext> options) : base(options) { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    base.OnConfiguring(optionsBuilder);
                }
            }
        }

        private static readonly BrunswickRootProvider MemoryRoot = new BrunswickRootProvider();

        private class BrunswickRootProvider
        {
            public readonly IServiceProvider ServiceProvider;
            public BrunswickRootProvider()
            {
                ServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
            }
        }

        private IncidentsDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<IncidentsDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .UseInternalServiceProvider(MemoryRoot.ServiceProvider)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new TestIncidentsDbContext(options);
        }

        [Fact]
        public void SaveDecision_ShouldSaveOnlyDecision_WhenDecisionTypeIsRefusal()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            var service = new DecisionService(context);

            var decision = new Decision
            {
                IdDecision = 1,
                IdIncident = 100,
                LegalBasis = "TestBasis",
                DecisionType = "отказ в возбуждении уголовного дела"
            };
            int teamId = 5;

            // Act
            service.SaveDecision(decision, null, null, teamId);

            // Assert
            using var assertContext = CreateDbContext(dbName);
            var savedDecision = assertContext.Decisions.Find(1);

            savedDecision.Should().NotBeNull();
            savedDecision!.IdResponseTeam.Should().Be(teamId);
            assertContext.CriminalCases.Should().BeEmpty();
            assertContext.TerritorialTransfers.Should().BeEmpty();
        }

        [Fact]
        public void SaveDecision_ShouldSaveDecisionAndCriminalCase_WhenDecisionTypeIsCriminalCaseInitiated()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            var service = new DecisionService(context);

            var decision = new Decision
            {
                IdDecision = 10,
                IdIncident = 101,
                LegalBasis = "TestBasis",
                DecisionType = "возбуждено уголовное дело"
            };
            var criminalCase = new CriminalCase
            {
                СaseNumber = 777,
                Qualification = "ст. 158 УК РФ"
            };
            int teamId = 3;

            // Act
            service.SaveDecision(decision, criminalCase, null, teamId);

            // Assert
            using var assertContext = CreateDbContext(dbName);

            var savedDecision = assertContext.Decisions.Find(10);
            savedDecision.Should().NotBeNull();
            savedDecision!.IdResponseTeam.Should().Be(teamId);

            var savedCase = assertContext.CriminalCases.Find(777);
            savedCase.Should().NotBeNull();
            savedCase!.IdDecision.Should().Be(10);
            assertContext.TerritorialTransfers.Should().BeEmpty();
        }

        [Fact]
        public void SaveDecision_ShouldSaveDecisionAndTerritorialTransfer_WhenDecisionTypeIsTerritorialTransfer()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            var service = new DecisionService(context);

            var decision = new Decision
            {
                IdDecision = 20,
                IdIncident = 102,
                LegalBasis = "TestBasis",
                DecisionType = "передано по территориальному признаку"
            };
            var transfer = new TerritorialTransfer
            {
                IdTransfer = 55,
                Division = "ОП №1 УМВД"
            };
            int teamId = 7;

            // Act
            service.SaveDecision(decision, null, transfer, teamId);

            // Assert
            using var assertContext = CreateDbContext(dbName);

            var savedDecision = assertContext.Decisions.Find(20);
            savedDecision.Should().NotBeNull();
            savedDecision!.IdResponseTeam.Should().Be(teamId);

            var savedTransfer = assertContext.TerritorialTransfers.Find(55);
            savedTransfer.Should().NotBeNull();
            savedTransfer!.IdDecision.Should().Be(20);
            assertContext.CriminalCases.Should().BeEmpty();
        }

        [Fact]
        public void SaveDecision_ShouldRollbackAndThrowException_WhenDatabaseErrorOccurs()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            var service = new DecisionService(context);
            var invalidDecision = new Decision { IdDecision = 99 };

            // Act
            Action act = () => service.SaveDecision(invalidDecision, null, null, 1);

            // Assert
            act.Should().Throw<DbUpdateException>();

            using var assertContext = CreateDbContext(dbName);
            assertContext.Decisions.Should().BeEmpty();
        }
    }
}
