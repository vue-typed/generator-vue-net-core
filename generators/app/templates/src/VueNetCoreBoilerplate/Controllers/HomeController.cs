// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using <%= name %>.Global.Options;

namespace <%= name %>.Controllers {
    
    public class HomeController : Controller {
        private readonly AppOptions _appOptions;

        public HomeController(IOptions<AppOptions> appOptions) {
            _appOptions = appOptions.Value;
        }

        public IActionResult Index() {
            ViewData["Title"] = _appOptions.Title;
            return View();
        }
    }
}
 