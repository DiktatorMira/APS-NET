using Microsoft.AspNetCore.Mvc;
using MusicPortal.BLL.Services;
using MusicPortal.Models;
using MusicPortal.Services;

namespace MusicPortal.Controllers {
    public class AuthorizationController : Controller {
        private readonly IUserService usersRep;
        private readonly ICryptographyService cryprography;
        public AuthorizationController(IUserService urep, ICryptographyService crypt) {
            usersRep = urep;
            cryprography = crypt;
        }
        public IActionResult Login() { return View("~/Views/Music/Login.cshtml"); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Logon logon) {
            if (ModelState.IsValid) {
                var user = await usersRep.GetUserByLogin(logon.Login!);
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
                return Redirect("/Music/Index");
            }
            return View("~/Views/Music/Login.cshtml", logon);
        }
        public IActionResult ToRegister() { return Redirect("/Registration/Register"); }
    }
}
