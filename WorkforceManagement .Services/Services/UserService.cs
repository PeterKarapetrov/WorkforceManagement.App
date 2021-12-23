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

        public async Task<bool> AssignUserAsAdminAsync(string username)
        {
            User user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
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

        public async Task<UserResponse> DeleteUserAsync(string userId)
        {
            User userToDelete = await _applicationDbContext.Users.FindAsync(userId);

            if (userToDelete == null)
            {
                return null;
            }

            userToDelete.IsDeleted = true;
            userToDelete.DeletedOn = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync();

            return new UserResponse 
            {
                Id  = userToDelete.Id,
                Username = userToDelete.UserName,
                Email = userToDelete.Email 
            };
        }

        public async Task<ICollection<UserResponse>> GetAllAsync()
        {
            ICollection<UserResponse> usersList = await _applicationDbContext.Users
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email
                })
                .ToListAsync();

            return usersList;
        }

        public Task<User> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }

        public async Task<User> GetUserById(string requesterUserId)
        {
            return await _userManager.FindByIdAsync(requesterUserId);
        }

        public async Task<UserResponse> UpdateUserAsync(string id, UserUpdateRequest inputDto)
        {
            User user = await _applicationDbContext.Users.FindAsync(id);

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

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new UserResponse
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email
                };
            }

            return null;
        }

        public async Task<bool> UserIsAdmin(User user)
        {
            return await _userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}