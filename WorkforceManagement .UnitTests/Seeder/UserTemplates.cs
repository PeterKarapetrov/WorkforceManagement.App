using WorkforceManagement.Common;
using WorkforceManagement.Data.Entities;
using WorkforceManagement.Models.DTO.Request.User;

namespace WorkforceManagement.UnitTests.Seeder
{
    public class UserTemplates
    {
        public static User GetUser(string seed)
        {
            var user = new User
            {
                UserName = $"user-{seed}",
            };
            return user;
        }

        public static UserCreateRequest GetUserCreateRequest(string seed = "", int teamId = 0)
        {
            var user = new UserCreateRequest
            {
                Username = $"Test-{seed}",
                Password = $"Test-{seed}",
                Email = $"Test-{seed}@{GlobalConstants.Host}",
                TeamId = teamId
            };
            return user;
        }
    }
}
