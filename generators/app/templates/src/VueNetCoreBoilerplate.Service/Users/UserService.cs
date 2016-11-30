using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using <%= name %>.Domain.Models;
using <%= name %>.Repository;
using <%= name %>.Service.Users.Dto;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace <%= name %>.Service.Users {
    public class UserService : IUserService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepo _repo;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IRepo repo) {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _repo = repo;
        }
      
        public async Task AddRole(AddRoleDto data) {
            var user = _userManager.FindByNameAsync(data.UserName).Result;
            if (user == null)
                throw new ValidationException($"User: {data.UserName} is not found.");


            await EnsureRoleIsExists(data.RoleName);
            await _userManager.AddToRoleAsync(user, data.RoleName);
        }

        public async Task<ClaimsPrincipal> SignUp(SignUpDto data, string[] roles = null) {

            if (data.Password != data.PasswordConfirm)
                throw new ValidationException("Password confirmation do not match.");

            var user = new ApplicationUser { UserName = data.UserName };
            var result = await _userManager.CreateAsync(user, data.Password);

            if (!result.Succeeded)
                throw new ValidationException(string.Join(", ", result.Errors.Select(x => x.Description)));

            using (var trx = _repo.Transaction()) {
                var profile = new UserProfile {
                    UserId = user.Id,
                    FullName = data.FullName
                };
                await trx.AddAsync(profile);
                await trx.SaveAndCommit(CancellationToken.None);
            }

            if (roles != null && roles.Any()) {
                try {
                    foreach (var role in roles) {
                        await EnsureRoleIsExists(role);
                    }
                    result = await _userManager.AddToRolesAsync(user, roles);
                    if (!result.Succeeded) {
                        throw new ValidationException(string.Join(", ", result.Errors.Select(x => x.Description)));
                    }
                }
                catch (System.Exception) {
                    await _userManager.DeleteAsync(user);
                    throw;
                }
            }

            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        public async Task<ClaimsPrincipal> SignIn(SignInDto data) {
            var user = new ApplicationUser { UserName = data.UserName };
            var result = await _signInManager.CheckPasswordSignInAsync(user, data.Password, false);

            if (!result.Succeeded)
                throw new ValidationException("Invalid login attempt.");

            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        public async Task<ClaimsPrincipal> RefreshUser(string userName) {
            var user = await _userManager.FindByNameAsync(userName);
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        private async Task EnsureRoleIsExists(string roleName) {
                        
            var tRoles = typeof(Roles).GetFields().Select(p => p.Name).ToList();
            if (!tRoles.Contains(roleName.ToUpper()))
                throw new ValidationException($"Invalid role name: {roleName}.");

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) {
                var createRoleResult = await _roleManager.CreateAsync(new ApplicationRole {
                    Name = roleName
                });
                if (!createRoleResult.Succeeded)
                    throw new ValidationException(string.Join(", ", createRoleResult.Errors.Select(x => x.Description)));
            }

        }

        public async Task<UserProfileDto> GetUserProfile(Guid userId) {
            var up = await _repo.Read<UserProfile>().FirstOrDefaultAsync(x => x.UserId == userId);
            return Mapper.Map<UserProfileDto>(up);
        }

        public async Task UpdateUserProfile(Guid userId, UserProfileDto profile) {
            using (var trx = _repo.Transaction()) {
                var up = await trx.Track<UserProfile>().FirstOrDefaultAsync(x => x.UserId == userId);
                if (up == null) throw new ValidationException("User profile is not found.");

                up.FullName = profile.FullName;
                up.Dob = profile.Dob;
                up.Gender = profile.Gender;
                up.Email = profile.Email;

                await trx.SaveAndCommit();
            }
        }
    }
}