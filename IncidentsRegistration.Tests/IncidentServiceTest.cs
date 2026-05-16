using FluentAssertions;
using IncidentsRegistration.Data;
using IncidentsRegistration.Models;
using IncidentsRegistration.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IncidentsRegistration.Tests
{
    public class IncidentServiceTests
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
                .Options;

            return new TestIncidentsDbContext(options);
        }

        [Fact]
        public void CreateIncident_ShouldAddIncidentAndLocationLink()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            var service = new IncidentService(context);

            var incident = new Incident
            {
                TypeOfIncident = "CreateTestType",
                IncidentLocations = new List<IncidentLocation>()
            };
            int locationId = 42;

            // Act
            service.CreateIncident(incident, locationId);

            // Assert
            using var assertContext = CreateDbContext(dbName);
            var addedIncident = assertContext.Incidents
                .Include(i => i.IncidentLocations)
                .FirstOrDefault(i => i.TypeOfIncident == "CreateTestType");

            addedIncident.Should().NotBeNull();
            addedIncident!.IncidentLocations.Should().HaveCount(1);
            addedIncident.IncidentLocations.First().IdLocation.Should().Be(locationId);
        }

        [Fact]
        public void GetAll_ShouldReturnAllIncidentsOrderedByDateTimeDescending()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);

            var incident1 = new Incident { IdIncident = 1, TypeOfIncident = "TestType",
                RegistrationDate = new DateOnly(2026, 5, 1), RegistrationTime = new TimeOnly(10, 0, 0) };
            var incident2 = new Incident { IdIncident = 2, TypeOfIncident = "TestType",
                RegistrationDate = new DateOnly(2026, 5, 1), RegistrationTime = new TimeOnly(12, 0, 0) };
            var incident3 = new Incident { IdIncident = 3, TypeOfIncident = "TestType",
                RegistrationDate = new DateOnly(2026, 5, 2), RegistrationTime = new TimeOnly(9, 0, 0) };

            context.Incidents.AddRange(incident1, incident2, incident3);
            context.SaveChanges();

            var service = new IncidentService(context);

            // Act
            var result = service.GetAll();

            // Assert
            result.Should().HaveCount(3);
            result[0].IdIncident.Should().Be(3);
            result[1].IdIncident.Should().Be(2);
            result[2].IdIncident.Should().Be(1);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectIncident_WhenIdExists()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);
            context.Incidents.Add(new Incident { IdIncident = 10, TypeOfIncident = "TestType" });
            context.SaveChanges();

            var service = new IncidentService(context);

            // Act
            var result = service.GetById(10);

            // Assert
            result.Should().NotBeNull();
            result!.IdIncident.Should().Be(10);
        }

        [Fact]
        public void GetActiveIncidentsByTeam_ShouldReturnOnlyIncidentsWithoutDecisionForSpecificTeam()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);

            var activeTeam1 = new Incident { IdIncident = 1, IdResponseTeam = 1,
                TypeOfIncident = "TestType", Decision = null };
            var activeTeam2 = new Incident { IdIncident = 3, IdResponseTeam = 2,
                TypeOfIncident = "TestType", Decision = null };

            var resolvedTeam1 = new Incident
            {
                IdIncident = 2,
                IdResponseTeam = 1,
                TypeOfIncident = "TestType",
                Decision = new Decision
                {
                    IdDecision = 1,
                    LegalBasis = "Basis",
                    DecisionType = "Type"
                }
            };

            context.Incidents.AddRange(activeTeam1, resolvedTeam1, activeTeam2);
            context.SaveChanges();

            var service = new IncidentService(context);

            // Act
            var result = service.GetActiveIncidentsByTeam(1);

            // Assert
            result.Should().ContainSingle();
            result.First().IdIncident.Should().Be(1);
        }

        [Fact]
        public void GetIncidentsByTeam_ShouldReturnAllIncidentsForSpecificTeam()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);

            context.Incidents.AddRange(
                new Incident { IdIncident = 1, IdResponseTeam = 5, TypeOfIncident = "TestType" },
                new Incident { IdIncident = 2, IdResponseTeam = 5, TypeOfIncident = "TestType" },
                new Incident { IdIncident = 3, IdResponseTeam = 9, TypeOfIncident = "TestType" }
            );
            context.SaveChanges();

            var service = new IncidentService(context);

            // Act
            var result = service.GetIncidentsByTeam(5);

            // Assert
            result.Should().HaveCount(2);
            result.Select(i => i.IdIncident).Should().Contain(new[] { 1, 2 });
        }

        [Fact]
        public void GetFullIncidentDetails_ShouldFilterSubjectRolesToOnlyLatestPerSubject()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);

            var incident = new Incident
            {
                IdIncident = 1,
                TypeOfIncident = "TestType",
                SubjectRoles = new List<SubjectRole>
                {
                    new SubjectRole { IdSubjectRole = 1, IdSubject = 10, RoleName = "RoleA",
                        DateOfInvolvement = new DateOnly(2026, 1, 1) },
                    new SubjectRole { IdSubjectRole = 2, IdSubject = 10, RoleName = "RoleB",
                        DateOfInvolvement = new DateOnly(2026, 5, 1) },
                    new SubjectRole { IdSubjectRole = 3, IdSubject = 20, RoleName = "RoleC",
                        DateOfInvolvement = new DateOnly(2026, 3, 1) }
                }
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            var service = new IncidentService(context);

            // Act
            var result = service.GetFullIncidentDetails(1);

            // Assert
            result.Should().NotBeNull();
            result!.SubjectRoles.Should().HaveCount(2);
            result.SubjectRoles.Should().Contain(sr => sr.IdSubjectRole == 2);
            result.SubjectRoles.Should().Contain(sr => sr.IdSubjectRole == 3);
            result.SubjectRoles.Should().NotContain(sr => sr.IdSubjectRole == 1);
        }

        [Fact]
        public void UpdateIncident_ShouldModifyIncidentAndLocationData()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var contextSetup = CreateDbContext(dbName))
            {
                var incidentSetup = new Incident 
                { 
                    IdIncident = 55, 
                    IdResponseTeam = 2, 
                    TypeOfIncident = "UpdateTestType" 
                };
                var locationSetup = new Location 
                {
                    IdLocation = 88, 
                    Region = "TestRegion", 
                    Settlement = "TestSettlement" 
                };

                contextSetup.Incidents.Add(incidentSetup);
                contextSetup.Locations.Add(locationSetup);
                contextSetup.SaveChanges();
            }

            using var context = CreateDbContext(dbName);
            var service = new IncidentService(context);

            var incidentToUpdate = context.Incidents.Find(55)!;
            var locationToUpdate = context.Locations.Find(88)!;

            incidentToUpdate.IdResponseTeam = 77;

            // Act
            service.UpdateIncident(incidentToUpdate, locationToUpdate);

            // Assert
            using var assertContext = CreateDbContext(dbName);
            var updatedIncident = assertContext.Incidents.Find(55);

            updatedIncident.Should().NotBeNull();
            updatedIncident!.IdResponseTeam.Should().Be(77);
        }

        [Fact]
        public void DeleteIncident_ShouldCascadeRemoveAllRelatedEntities()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using var context = CreateDbContext(dbName);

            var incident = new Incident
            {
                IdIncident = 1,
                TypeOfIncident = "TestType",
                IncidentLocations = new List<IncidentLocation> { new IncidentLocation { IdIncidentLocation = 1 } },
                SubjectRoles = new List<SubjectRole> { new SubjectRole { IdSubjectRole = 1, RoleName = "Role" } },
                Decision = new Decision
                {
                    IdDecision = 1,
                    LegalBasis = "Basis",
                    DecisionType = "Type",
                    CriminalCase = new CriminalCase { Qualification = "TestQualification" },
                    TerritorialTransfer = new TerritorialTransfer { Division = "TestDivision" }
                }
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            var service = new IncidentService(context);

            // Act
            service.DeleteIncident(1);

            // Assert
            using var assertContext = CreateDbContext(dbName);

            assertContext.Incidents.Find(1).Should().BeNull();
            assertContext.IncidentLocations.Should().BeEmpty();
            assertContext.SubjectRoles.Should().BeEmpty();
            assertContext.Decisions.Should().BeEmpty();
            assertContext.CriminalCases.Should().BeEmpty();
            assertContext.TerritorialTransfers.Should().BeEmpty();
        }
    }
}
