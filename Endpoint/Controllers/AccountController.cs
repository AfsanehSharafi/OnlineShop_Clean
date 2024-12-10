using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
