using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Services;

namespace MusicPortal.Controllers {
    public class RegistrationController : Controller {
        private readonly IRepository repository;
        private readonly ICryptography cryptography;
        public RegistrationController(IRepository rep, ICryptography crypt) {
            repository = rep;
            cryptography = crypt;
        }
        public IActionResult Register() { return View("~/Views/Music/Register.cshtml"); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register reg) {
            if (ModelState.IsValid) {
                if (await repository.IsLoginTaken(reg.Login!)) {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует!");
                    return View("~/Views/Music/Register.cshtml", reg);
                } else if (reg.Password!.Length < 6) {
                    ModelState.AddModelError("Password", "Пароль должен содержать не менее 6 символов!");
                    return View("~/Views/Music/Register.cshtml", reg);
                } else if (!(reg.Password.Any(char.IsUpper) && reg.Password.Any(char.IsDigit))) {
                    ModelState.AddModelError("Password", "Пароль должен содержать хотя бы одну заглавную букву и одну цифру!");
                    return View("~/Views/Music/Register.cshtml", reg);
                }

                string salt = cryptography.GenerateSalt();
                string hashedPassword = cryptography.HashPassword(reg.Password, salt);

                var user = new User {
                    Login = reg.Login,
                    FullName = reg.FullName,
                    Password = hashedPassword,
                    Salt = salt,
                };
                if(reg.Login == "Admin") user.IsAuthorized = true;
                await repository.AddUser(user);
                await repository.SaveDb();

                HttpContext.Session.SetString("Authorization", user.IsAuthorized.ToString());
                HttpContext.Session.SetString("Login", user.Login!);
                return Redirect("/Main/Index");
            }
            return View("~/Views/Music/Register.cshtml", reg);
        }
        public IActionResult ToLogin() { return Redirect("/Authorization/Login"); }
    }
}
