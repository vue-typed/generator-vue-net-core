using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using <%= name %>.Domain.Models;
using <%= name %>.Repository;
using <%= name %>.Service.Users.Dto;
using System.Linq;

namespace <%= name %>.Service.Users.Stores {
    public class UserStore : IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser> {
        private readonly IRepo _repo;

        public UserStore(IRepo repo) {
            _repo = repo;
        }

        public void Dispose() {
            _repo.Dispose();
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName,
            CancellationToken cancellationToken) {
            user.UserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var u = Mapper.Map<User>(user);
                    trx.AddAsync(u);
                    trx.SaveAndCommit(cancellationToken);
                    user.Id = u.Id;
                    return IdentityResult.Success;
                }
            }, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var u = trx.FindAsync<User>(user.Id, cancellationToken).Result;
                    u.UserName = user.UserName;
                    u.PasswordHash = user.PasswordHash;
                    trx.SaveAndCommit(cancellationToken);
                    return IdentityResult.Success;
                }
            }, cancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var u = trx.FindAsync<User>(user.Id, cancellationToken).Result;
                    trx.Remove(u);
                    trx.SaveAndCommit(cancellationToken);
                    return IdentityResult.Success;
                }
            }, cancellationToken);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var id = new Guid(userId);
                var u = _repo.FindAsync<User>(id, cancellationToken).Result;
                return Mapper.Map<ApplicationUser>(u);
            }, cancellationToken);
        }


        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var u = _repo.Read<User>().FirstOrDefault(x => x.UserName.ToUpper() == normalizedUserName);
                return u == null ? default(ApplicationUser) : Mapper.Map<ApplicationUser>(u);
            }, cancellationToken);
        }


        /// ============================================= IUserPasswordStore Implementations ============================================= ///
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken) {
            // ADDPASS: 2
            // REG: 4
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.Run(() => {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var u = _repo.Read<User>().FirstOrDefault(x => x.UserName == user.UserName);
                return u?.PasswordHash;
            }, cancellationToken);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }


        /// ============================================= IUserRoleStore Implementations ============================================= ///
        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var userToAdd = trx.FindAsync<User>(user.Id, cancellationToken).Result;
                    var roleToAdd = trx.Track<Role>().SingleOrDefault(x => x.Name == roleName.Trim().ToUpper());
                    if (roleToAdd == null)
                        throw new InvalidOperationException($"Role [{roleName}] is not found.");

                    var userRole = new UserRole {
                        User = userToAdd,
                        Role = roleToAdd
                    };
                    trx.AddAsync(userRole);
                    trx.SaveAndCommit(cancellationToken);
                }
            }, cancellationToken);
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var userRole =
                        trx.Track<UserRole>().FirstOrDefault(x => x.UserId == user.Id && x.Role.Name == roleName);
                    if (userRole != null) {
                        trx.Remove(userRole);
                        trx.SaveAndCommit(cancellationToken);
                    }
                }
            }, cancellationToken);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var roles =
                    _repo.Read<UserRole>()
                        .Where(x => x.User.UserName == user.UserName.ToUpper().Trim())
                        .Select(x => x.Role.Name)
                        .ToList();

                return (IList<string>) roles;
            }, cancellationToken);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken) {
            return
                Task.Run(
                    () => {
                        return
                            _repo.Read<UserRole>()
                                .Any(x => x.UserId == user.Id && x.Role.Name == roleName.ToUpper().Trim());
                    },
                    cancellationToken);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var users = _repo.Read<User>().Where(x => x.UserRoles.Any(r => r.Role.Name == roleName)).ToList();
                return Mapper.Map<IList<ApplicationUser>>(users);
            }, cancellationToken);
        }
    }
}