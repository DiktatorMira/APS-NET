using Dz09._04._2024.Models;
using Dz09._04._2024.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dz09._04._2024.Controllers {
    public class MessageController : Controller {
        private readonly IRepository repository;
        private readonly ICryptography cryprography;
        public MessageController(IRepository rep, ICryptography crypt) {
            repository = rep;
            cryprography = crypt;
        }
        //Login
        public IActionResult Login() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Logon logon) {
            if (ModelState.IsValid) {
                var user = await repository.GetUserByLoginAsync(logon.Login!);
                if (user == null) return Json(new { success = false, errors = "Неверный логин или пароль!" });
                var hashedPassword = cryprography.HashPassword(logon.Password!, user.Salt!);
                if (user.Password != hashedPassword) return Json(new { success = false, errors = "Неверный логин или пароль!" });
                HttpContext.Session.SetString("Fullname", user.FullName!);
                HttpContext.Session.SetString("Login", user.Login!);
                return Json("Вы авторизовались!");
            }
            return Problem("Форма не валидна!");
        }
        public IActionResult Continue() {
            HttpContext.Session.SetString("Login", "Guest");
            return Redirect("/Message/Index");
        }
        //Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register reg)
        {
            if (ModelState.IsValid) {
                if (await repository.IsLoginTakenAsync(reg.Login!))
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует!");
                    return View(reg);
                }
                else if (reg.Password!.Length < 6)
                {
                    ModelState.AddModelError("Password", "Пароль должен содержать не менее 6 символов!");
                    return View(reg);
                }
                else if (!(reg.Password.Any(char.IsUpper) && reg.Password.Any(char.IsDigit)))
                {
                    ModelState.AddModelError("Password", "Пароль должен содержать хотя бы одну заглавную букву и одну цифру!");
                    return View(reg);
                }

                string salt = cryprography.GenerateSalt();
                string hashedPassword = cryprography.HashPassword(reg.Password, salt);
                var user = new User
                {
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
        //Index
        public async Task<IActionResult> Index([FromServices] IRepository rep) {
            //if (HttpContext.Session.GetString("Login") != null) {
                var messages = await rep.GetAllMessagesAsync();
                return View(new CombinedMessages{ Messages = messages });
            //} else return Redirect("/Authentication/Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(CombinedMessages combinedMessages) {
            if (ModelState.IsValid) {
                var login = HttpContext.Session.GetString("Login");
                if (login != null) {
                    var user = await repository.GetUserByLoginAsync(login);
                    if (user != null) {
                        var message = combinedMessages.MessageModel;
                        message!.UserId = user.Id;
                        message.Date = DateTime.Now;
                        await repository.AddMessageAsync(message);
                    }
                }
                return Redirect("/Message/Index");
            } else {
                var messages = await repository.GetAllMessagesAsync();
                return View(combinedMessages);
            }
        }
        public IActionResult Logout()  {
            HttpContext.Session.Clear();
            return Redirect("/Authentication/Login");
        }
    }
}
