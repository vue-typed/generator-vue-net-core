using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using <%= name %>.Domain.Models;
using <%= name %>.Repository;
using <%= name %>.Service.Users.Dto;
using System.Linq;

namespace <%= name %>.Service.Users.Stores {
    public class RoleStore : IRoleStore<ApplicationRole> {
        private readonly IRepo _repo;

        public RoleStore(IRepo repo) {
            _repo = repo;
        }

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    trx.AddAsync(Mapper.Map<Role>(role));
                    trx.SaveAndCommit(cancellationToken);
                    return IdentityResult.Success;
                }
            }, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var match = trx.Track<Role>().FirstOrDefault(r => r.Id == role.Id);
                    if (match == null) return IdentityResult.Failed();

                    match.Name = role.Name;
                    trx.SaveAndCommit(cancellationToken);
                    return IdentityResult.Success;
                }
            }, cancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken) {
            return Task.Run(() => {
                using (var trx = _repo.Transaction()) {
                    var match = trx.Track<Role>().FirstOrDefault(r => r.Id == role.Id);
                    if (match == null) return IdentityResult.Failed();

                    trx.Remove(match);
                    trx.SaveAndCommit(cancellationToken);
                    return IdentityResult.Success;
                }
            }, cancellationToken);
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var id = Guid.Parse(roleId);
                var role = _repo.Read<Role>().FirstOrDefault(r => r.Id == id);

                return Mapper.Map<ApplicationRole>(role);
            }, cancellationToken);
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
            return Task.Run(() => {
                var role = _repo.Read<Role>().FirstOrDefault(r => r.Name == normalizedRoleName);
                return Mapper.Map<ApplicationRole>(role);
            }, cancellationToken);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken) {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken) {
            return Task.FromResult(role.Name);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken) {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken) {
            role.Name = roleName;

            return Task.FromResult(true);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName,
            CancellationToken cancellationToken) {
            role.Name = normalizedName;

            return Task.FromResult(true);
        }

        public void Dispose() {
        }
    }
}