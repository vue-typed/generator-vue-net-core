using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueNetCoreBoilerplate.JWT;
using VueNetCoreBoilerplate.Service.Users;
using VueNetCoreBoilerplate.Service.Users.Dto;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VueNetCoreBoilerplate.Controllers {
    [Route("api/[controller]")]
    public class AccountController : Controller {
        private readonly TokenProvider _tokenProvider;
        private readonly IUserService _userService;

        public AccountController(TokenProvider tokenProvider, IUserService userService) {
            _tokenProvider = tokenProvider;
            _userService = userService;
        }

        [Route("")]
        [Authorize]
        [HttpGet]
        public string Get() {
            return $"Hey {User.Identity.Name}, now you see me!";
        }


        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto data) {
            try {
                var user = await _userService.SignUp(data);
                return Json(await CreateToken(user));
            }
            catch (ValidationException ex) {
                return BadRequest(ex.Message);
            }
        }


        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInDto data) {
            try {
                var user = await _userService.SignIn(data);
                return Json(await CreateToken(user));
            }
            catch (ValidationException ex) {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [Route("refresh-token")]
        [HttpGet]
        public async Task<object> RefreshToken() {            
            return Json(await CreateToken(await _userService.RefreshUser(User.Identity.Name)));
        }


        private async Task<object> CreateToken(ClaimsPrincipal principal) {
            return await _tokenProvider.GenerateTokenAsync(principal);
        }


        [Route("createsuperadmin")]
        [HttpPost]
        public async Task<IActionResult> CreateSuperAdmin([FromBody] SignUpDto data) {
            await _userService.SignUp(data, new[] {Roles.SUPER_ADMIN});
            return Ok();
        }

        [Route("addrole")]
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto data) {
            await _userService.AddRole(data);
            return Ok();
        }

        [Authorize]
        [Route("profile")]
        [HttpGet]
        public async Task<UserProfileDto> GetProfile() {            
            return await _userService.GetUserProfile(Guid.Parse(User.Claims.Single(x=>x.Type == ClaimTypes.Sid).Value));
        }

        [Authorize]
        [Route("profile")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto profile) {
            await _userService.UpdateUserProfile(Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.Sid).Value), profile);
            return Ok();
        }
    }
}