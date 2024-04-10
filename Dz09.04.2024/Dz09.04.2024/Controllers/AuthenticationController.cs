using Dz09._04._2024.Models;
using Dz09._04._2024.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dz09._04._2024.Controllers {
    public class AuthenticationController : Controller
    {
        private readonly IRepository repository;
        private readonly ICryptography cryprography;
        public AuthenticationController(IRepository rep, ICryptography crypt) {
            repository = rep;
            cryprography = crypt;
        }
        public IActionResult Login() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Logon logon) {
            if (ModelState.IsValid)  {
                var user = await repository.GetUserByLoginAsync(logon.Login!);
                if (user == null) {
                    ModelState.AddModelError("Login", "Неверный логин или пароль!");
                    return View(logon);
                }
                var hashedPassword = cryprography.HashPassword(logon.Password!, user.Salt!);
                if (user.Password != hashedPassword) {
                    ModelState.AddModelError("Login", "Неверный логин или пароль!");
                    return View(logon);
                }
                HttpContext.Session.SetString("Fullname", user.FullName!);
                HttpContext.Session.SetString("Login", user.Login!);
                return Redirect("/Message/Index");
            }
            return View(logon);
        }
        public IActionResult Continue() {
            HttpContext.Session.SetString("Login", "Guest");
            return Redirect("/Message/Index");
        }
        public IActionResult ToRegister() { return Redirect("/Registration/Register"); }
    }
}
