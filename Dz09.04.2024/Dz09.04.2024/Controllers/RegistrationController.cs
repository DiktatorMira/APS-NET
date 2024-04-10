using Dz09._04._2024.Models;
using Dz09._04._2024.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dz09._04._2024.Controllers {
    public class RegistrationController : Controller {
        private readonly IRepository repository;
        private readonly ICryptography cryptography;
        public RegistrationController(IRepository rep, ICryptography crypt) {
            repository = rep;
            cryptography = crypt;
        }
        public IActionResult Register() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register reg) {
            if (ModelState.IsValid) {
                if (await repository.IsLoginTakenAsync(reg.Login!)) {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует!");
                    return View(reg);
                } else if (reg.Password!.Length < 6) {
                    ModelState.AddModelError("Password", "Пароль должен содержать не менее 6 символов!");
                    return View(reg);
                } else if (!(reg.Password.Any(char.IsUpper) && reg.Password.Any(char.IsDigit))) {
                    ModelState.AddModelError("Password", "Пароль должен содержать хотя бы одну заглавную букву и одну цифру!");
                    return View(reg);
                }

                string salt = cryptography.GenerateSalt();
                string hashedPassword = cryptography.HashPassword(reg.Password, salt);
                var user = new User {
                    Login = reg.Login,
                    FullName = reg.FullName,
                    Password = hashedPassword,
                    Salt = salt
                };
                await repository.AddUserAsync(user);

                HttpContext.Session.SetString("Fullname", user.FullName ?? "");
                HttpContext.Session.SetString("Login", user.Login ?? "");
                return Redirect("/Message/Index");
            }
            return View(reg);
        }
        public IActionResult ToLogin() { return Redirect("/Authentication/Login"); }
    }
}
