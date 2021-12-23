using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagement.Data.Database;
using WorkforceManagement.Data.Entities;
using WorkforceManagement.Models.DTO.Request.User;
using WorkforceManagement.Services.Services;
using WorkforceManagement.UnitTests.Seeder;
using Xunit;

namespace WorkforceManagement.UnitTests.ServicesTests
{
    public class UserServiceTests : IDisposable
    {
        public DbContextOptions<ApplicationDbContext> _options;
        ApplicationDbContext _dbContext;

        public UserServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb").Options;

            _dbContext = new ApplicationDbContext(_options);
        }

        [Fact]
        public async Task CreateUser_Valid_ReturnsUserResponse()
        {
            // arrange
            var model = UserTemplates.GetUserCreateRequest();

            var manager = MockUserManager(new List<User>());

            var sut = new UserService(_dbContext, manager.Object);

            // act
            var result = await sut.CreateUserAsync(model);

            // assert
            Assert.NotNull(result);
            Assert.Equal(result.Username, model.Username);
        }

        [Fact]
        public async Task CreateUser_ValidTeamId_ReturnsUserResponse()
        {
            // arrange
            var team = new Team
            {
                Title = "Test",
                Description = "Test",
                TeamLeader = new User()
            };
            await _dbContext.Teams.AddAsync(team);
            await _dbContext.SaveChangesAsync();

            var model = UserTemplates.GetUserCreateRequest();

            var manager = MockUserManager(new List<User>());
            var sut = new UserService(_dbContext, manager.Object);

            // act
            var result = await sut.CreateUserAsync(model);

            // assert
            Assert.NotNull(result);
            Assert.Equal(result.Username, model.Username);
        }

        [Fact]
        public async Task CreateUser_InValidTeamId_ReturnsNull()
        {
            // arrange
            var model = new UserCreateRequest
            {
                Username = "Test",
                Password = "Test",
                Email = "test@test.com",
                TeamId = 1
            };

            var manager = MockUserManager(new List<User>());
            var sut = new UserService(_dbContext, manager.Object);

            // act
            var result = await sut.CreateUserAsync(model);

            // assert
            Assert.Null(result);
        }

        public static Mock<UserManager<User>> MockUserManager(List<User> users)
        {
            var store = new Mock<IUserStore<User>>();
            var manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            manager.Object.UserValidators.Add(new UserValidator<User>());
            manager.Object.PasswordValidators.Add(new PasswordValidator<User>());

            manager.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            manager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback<User, string>((x, y) => users.Add(x));
            manager.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            return manager;
        }

        // ToDo
        public void Dispose()
        {
            //_dbContext.Database.EnsureDeleted();
        }
    }
}
