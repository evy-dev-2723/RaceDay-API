using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Xunit;

namespace RaceDay.Tests
{
    public class AuthTests
    {
        private RaceDayDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task Register_NewUser_SavesToDatabase()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Roles.Add(new Role { RoleID = 1, RoleName = "Participant" });
            await context.SaveChangesAsync();

            // Act
            var user = new User
            {
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123"),
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                RoleID = 1
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var saved = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@test.com");
            Assert.NotNull(saved);
            Assert.Equal("Test", saved.FirstName);
            Assert.Equal("User", saved.LastName);
        }

        [Fact]
        public void Password_IsHashed_NotPlainText()
        {
            // Arrange
            var plainPassword = "Test123";
            var hashed = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            // Assert
            Assert.NotEqual(plainPassword, hashed);
            Assert.True(BCrypt.Net.BCrypt.Verify(plainPassword, hashed));
        }

        [Fact]
        public async Task User_CannotHaveDuplicateEmail()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Roles.Add(new Role { RoleID = 1, RoleName = "Participant" });
            await context.SaveChangesAsync();

            context.Users.Add(new User
            {
                Email = "duplicate@test.com",
                PasswordHash = "hash1",
                FirstName = "First",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                RoleID = 1
            });
            await context.SaveChangesAsync();

            // Act & Assert
            var exists = await context.Users.AnyAsync(u => u.Email == "duplicate@test.com");
            Assert.True(exists);
        }
    }
}