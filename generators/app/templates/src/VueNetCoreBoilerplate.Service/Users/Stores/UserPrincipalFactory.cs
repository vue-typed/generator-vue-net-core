using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using <%= name %>.Domain.Models;
using <%= name %>.Repository;
using <%= name %>.Service.Users.Dto;
using System.Linq;

namespace <%= name %>.Service.Users.Stores {
    public class UserPrincipalFactory : IUserClaimsPrincipalFactory<ApplicationUser> {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepo _repo;

        public UserPrincipalFactory(UserManager<ApplicationUser>  userManager, IRepo repo) {
            _userManager = userManager;
            _repo = repo;
        }

        public Task<ClaimsPrincipal> CreateAsync(ApplicationUser user) {

            return Task.Run(() => {
                var u = _repo.Read<User>(x => x.Profile).Single(x => x.UserName == user.UserName);

                var identity = new ClaimsIdentity("Microsoft.AspNet.Identity.Application");
                identity.AddClaim(new Claim(ClaimTypes.Sid, u.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, u.UserName));
                identity.AddClaim(new Claim("fullName", u.Profile.FullName));

                var roles = _userManager.GetRolesAsync(Mapper.Map<ApplicationUser>(u)).Result;
                foreach (var role in roles) {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var principal = new ClaimsPrincipal(identity);

                return principal;
            });

        }
    }
}