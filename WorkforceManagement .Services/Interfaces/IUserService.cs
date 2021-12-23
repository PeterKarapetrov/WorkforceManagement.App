using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.Data.Entities;
using WorkforceManagement.Models.DTO.Request.User;
using WorkforceManagement.Models.DTO.Response.User;

namespace WorkforceManagement.Services.Contracts
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(UserCreateRequest inputDto);

        Task<UserResponse> UpdateUserAsync(string id, UserUpdateRequest inputDto);

        Task<UserResponse> DeleteUserAsync(string userId);

        Task<ICollection<UserResponse>> GetAllAsync();

        Task<User> GetCurrentUserAsync(ClaimsPrincipal user);

        Task<bool> AssignUserAsAdminAsync(string username);

        Task<User> GetUserById(string requesterUserId);

        Task<bool> UserIsAdmin(User user);
    }
}