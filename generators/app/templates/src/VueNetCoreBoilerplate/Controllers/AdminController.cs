using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using <%= name %>.Service.Users;

namespace <%= name %>.Controllers {
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.SUPER_ADMIN)]
    public class AdminController : Controller {
        [Route("")]
        [HttpGet]
        public async Task<string> Index() {
            return await Task.Run(() => $"Now you see me.");
        }
    }
}