using Dz09._04._2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Dz09._04._2024.Controllers {
    public class UsersController : Controller {
        private readonly UserContext db;
        public UsersController(UserContext context) => db = context;
        public IActionResult Continue() {
            HttpContext.Session.SetString("Login", "Guest");
            return RedirectToAction("Index", "Users"); 
        }
        public IActionResult ToRegister() { return RedirectToAction("Register", "Users"); }
        public IActionResult ToLogin() { return RedirectToAction("Login", "Users"); }
        public ActionResult Login() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Logon logon) {
            if (ModelState.IsValid) {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == logon.Login);
                if (user == null) {
                    ModelState.AddModelError("Login", "Неверный логин!");
                    return View(logon);
                }

                string? salt = user.Salt;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(salt + logon.Password), hashedPasswordBytes;
                using (SHA256 sha256 = SHA256.Create()) hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                if (user.Password != hashedPassword) {
                    ModelState.AddModelError("Password", "Неверный пароль!");
                    return View(logon);
                }

                HttpContext.Session.SetString("Fullname", user.FullName!);
                HttpContext.Session.SetString("Login", user.Login!);
                return RedirectToAction("Index", "Users");
            }
            return View(logon);
        } 
        public IActionResult Register() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register reg) {
            if (ModelState.IsValid) {
                if (db.Users.Any(u => u.Login == reg.Login)) {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже есть!");
                    return View(reg);
                } else if (reg.Password!.Length < 6) {
                    ModelState.AddModelError("Password", "Пароль должен содержать не менее 6 символов!");
                    return View(reg);
                } else if (!Regex.IsMatch(reg.Password, @"^(?=.*[A-Z])(?=.*\d).+$")) {
                    ModelState.AddModelError("Password", "Пароль должен содержать хотя бы одну заглавную букву и одну цифру!");
                    return View(reg);
                }
                byte[] saltBytes = new byte[16];
                using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()) rngCsp.GetBytes(saltBytes);
                string salt = Convert.ToBase64String(saltBytes);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(salt + reg.Password), hashedPasswordBytes;
                using (SHA256 sha256 = SHA256.Create()) hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                string hash = Convert.ToBase64String(hashedPasswordBytes);

                User user = new User {
                    Login = reg.Login,
                    FullName = reg.FullName,
                    Password = hash.ToString(),
                    Salt = salt
                };
                db.Users?.AddAsync(user);
                await db.SaveChangesAsync();
                HttpContext.Session.SetString("Fullname", user.FullName!);
                HttpContext.Session.SetString("Login", user.Login!);
                return RedirectToAction("Index", "Users");
            }
            return View(reg);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(CombinedMessages combinedMessages) {
            if (ModelState.IsValid)  {
                var login = HttpContext.Session.GetString("Login");
                if (login != null) {
                    var user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
                    if (user != null) {
                        var message = combinedMessages.MessageModel;
                        message!.UserId = user.Id;
                        message.Date = DateTime.Now;
                        db.Messages.Add(message);
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "Users");
            } else return View(combinedMessages); 
        }
        public async Task<IActionResult> Index() {
            if (HttpContext.Session.GetString("Login") != null) {
                var messages = await db.Messages.Include(m => m.User).ToListAsync();
                return View(new CombinedMessages { Messages = messages });
            } else return RedirectToAction("Login", "Users");
        }
        public ActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }
    }
}
