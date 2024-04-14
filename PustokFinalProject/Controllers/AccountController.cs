using Microsoft.AspNetCore.Mvc;
using PustokFinalProject.ViewModels;

namespace PustokFinalProject.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVm registerVm)
        {
            return View();
        }
    }
}
