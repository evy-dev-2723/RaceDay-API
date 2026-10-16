using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Xunit;

namespace RaceDay.Tests
{
    public class EventTests
    {
        private RaceDayDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task Organiser_CanCreateEvent()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Roles.Add(new Role { RoleID = 1, RoleName = "Organiser" });
            context.Users.Add(new User
            {
                UserID = 1,
                Email = "organiser@test.com",
                PasswordHash = "hash",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 1, 1),
                RoleID = 1
            });
            await context.SaveChangesAsync();

            // Act
            var newEvent = new Event
            {
                OrganiserID = 1,
                EventName = "Cape Town Marathon",
                EventDate = new DateTime(2026, 12, 15),
                Location = "Cape Town",
                DistanceKm = 42.2m,
                IsActive = true
            };
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            // Assert
            var saved = await context.Events.FirstOrDefaultAsync(e => e.EventName == "Cape Town Marathon");
            Assert.NotNull(saved);
            Assert.Equal(42.2m, saved.DistanceKm);
            Assert.Equal("Cape Town", saved.Location);
        }

        [Fact]
        public async Task Event_IsLinkedToOrganiser()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Roles.Add(new Role { RoleID = 1, RoleName = "Organiser" });
            context.Users.Add(new User
            {
                UserID = 1,
                Email = "organiser@test.com",
                PasswordHash = "hash",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 1, 1),
                RoleID = 1
            });
            await context.SaveChangesAsync();

            // Act
            var newEvent = new Event
            {
                OrganiserID = 1,
                EventName = "Test Event",
                EventDate = new DateTime(2026, 12, 15),
                Location = "Durban",
                DistanceKm = 10.0m
            };
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            // Assert
            var savedEvent = await context.Events
                .Include(e => e.Organiser)
                .FirstOrDefaultAsync(e => e.EventID == newEvent.EventID);

            Assert.NotNull(savedEvent);
            Assert.NotNull(savedEvent.Organiser);
            Assert.Equal("John", savedEvent.Organiser.FirstName);
        }

        [Fact]
        public async Task MultipleEvents_CanBeCreatedBySameOrganiser()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Roles.Add(new Role { RoleID = 1, RoleName = "Organiser" });
            context.Users.Add(new User
            {
                UserID = 1,
                Email = "organiser@test.com",
                PasswordHash = "hash",
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 1, 1),
                RoleID = 1
            });
            await context.SaveChangesAsync();

            // Act
            context.Events.AddRange(
                new Event { OrganiserID = 1, EventName = "Event 1", EventDate = new DateTime(2026, 1, 1), Location = "A", DistanceKm = 5 },
                new Event { OrganiserID = 1, EventName = "Event 2", EventDate = new DateTime(2026, 2, 1), Location = "B", DistanceKm = 10 }
            );
            await context.SaveChangesAsync();

            // Assert
            var count = await context.Events.CountAsync(e => e.OrganiserID == 1);
            Assert.Equal(2, count);
        }
    }
}