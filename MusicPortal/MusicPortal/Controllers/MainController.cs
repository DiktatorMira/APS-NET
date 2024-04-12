using Microsoft.AspNetCore.Mvc;
using MusicPortal.Services;
using Microsoft.AspNetCore.Http;
using MusicPortal.Models;

namespace MusicPortal.Controllers {
    public class MainController : Controller {
        private readonly IRepository repository;
        public MainController(IRepository rep) => repository = rep;
        public IActionResult Index() {
            if (HttpContext.Session.GetString("Login") != null) return View("~/Views/Music/Index.cshtml");
            else return Redirect("/Authorization/Login");
        }
        public IActionResult ToLogin() { return Redirect("/Authorization/Login"); }
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return Redirect("/Authorization/Login");
        }
        public async Task<IActionResult> ToUsers() {
            return View("~/Views/Music/Users.cshtml", await repository.GetUsers());
        }
        public async Task<IActionResult> Authorize(int userId) {
            var user = await repository.GetUserById(userId);
            if (user != null) {
                user.IsAuthorized = !user.IsAuthorized;
                await repository.SaveDb();
            }
            return View("~/Views/Music/Users.cshtml", await repository.GetUsers());
        }
        public async Task<IActionResult> DeleteUser(int userId) {
            var user = await repository.GetUserById(userId);
            if (user != null) {
                repository.DeleteUser(user);
                await repository.SaveDb();
            }
            return View("~/Views/Music/Users.cshtml", await repository.GetUsers());
        }
    }
}
