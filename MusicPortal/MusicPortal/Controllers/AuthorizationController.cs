using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Services;

namespace MusicPortal.Controllers {
    public class AuthorizationController : Controller {
        private readonly IRepository repository;
        private readonly ICryptography cryprography;
        public AuthorizationController(IRepository rep, ICryptography crypt) {
            repository = rep;
            cryprography = crypt;
        }
        public IActionResult Login() { return View("~/Views/Music/Login.cshtml"); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Logon logon) {
            if (ModelState.IsValid) {
                var user = await repository.GetUserByLogin(logon.Login!);
                if (user == null) {
                    ModelState.AddModelError("Login", "Неверный логин или пароль!");
                    return View("~/Views/Music/Login.cshtml", logon);
                }
                var hashedPassword = cryprography.HashPassword(logon.Password!, user.Salt!);
                if (user.Password != hashedPassword) {
                    ModelState.AddModelError("Login", "Неверный логин или пароль!");
                    return View("~/Views/Music/Login.cshtml", logon);
                }

                HttpContext.Session.SetString("Authorization", user.IsAuthorized.ToString());
                HttpContext.Session.SetString("Login", user.Login!);
                return Redirect("/Main/Index");
            }
            return View("~/Views/Music/Login.cshtml", logon);
        }
        public IActionResult ToRegister() { return Redirect("/Registration/Register"); }
    }
}
