using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HubStoreV2.Controllers
{
    [AllowAnonymous]
    public class AuthPagesController : Controller
    {
        [HttpGet]
        [Route("Auth/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [Route("Auth/LoginDX")]
        public IActionResult LoginDX()
        {
            return View();
        }

        [HttpGet]
        [Route("Auth/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [Route("Auth/RegisterDX")]
        public IActionResult RegisterDX()
        {
            return View();
        }

        [HttpGet]
        [Route("Auth/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Route("Auth/ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("Auth/Logout")]
        public async Task<IActionResult> Logout()
        {
            // This will be handled by the API controller
            // Redirect to login page
            return RedirectToAction("LoginDX");
        }
    }
}
