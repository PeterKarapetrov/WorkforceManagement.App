using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.Data.Database;
using WorkforceManagement.Data.Entities;
using WorkforceManagement.Models.DTO.Request.User;
using WorkforceManagement.Models.DTO.Response.User;
using WorkforceManagement.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkforceManagement.Common;
using WorkforceManagement.Services.Interfaces;

namespace WorkforceManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext applicationDbContext, UserManager<User> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public async Task<bool> AssignUserAsAdmin(string username)
        {
            User user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false;
            }
            await _userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            return true;
        }

        // ToDo Add check for team by providet Id if any
        public async Task<UserResponse> CreateUserAsync(UserCreateRequest inputDto)
        {
            User user = new User
            {
                UserName = inputDto.Username,
                EmailConfirmed = true,
                Email = inputDto.Email,
            };

            if (inputDto.TeamId != 0)
            {
                Team team = await _applicationDbContext.Teams.FindAsync(inputDto.TeamId);

                if (team != null)
                {
                    user.Teams.Add(team);
                }
            }

            var result = await _userManager.CreateAsync(user, inputDto.Password);

            if (result.Succeeded)
            {
                UserResponse userResponse = new UserResponse
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                };

                return userResponse;
            }

            return null;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            User user = await _applicationDbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            List<UserResponse> result = await _applicationDbContext.Users
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email
                }).ToListAsync();

            return result;

        }

        public Task<User> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }

        public async Task<User> GetUserById(string requesterUserId)
        {
            return await _userManager.FindByIdAsync(requesterUserId);
        }

        public async Task<UserResponse> UpdateUserAsync(UserUpdateRequest inputDto)
        {
            User user = await _applicationDbContext.Users.FindAsync(inputDto.UserId);
            if (user == null)
            {
                return null;
            }

            if (inputDto.TeamId != 0)
            {
                Team team = await _applicationDbContext.Teams.FindAsync(inputDto.TeamId);
                if (team == null)
                {
                    return null;
                }
                user.Teams.Add(team);
            }

            user.UserName = inputDto.Username;
            user.Email = inputDto.Email;

            await _userManager.UpdateAsync(user);

            UserResponse result = new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            };
            return result;
        }

        public async Task<bool> UserIsAdmin(User user)
        {
            return await _userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}