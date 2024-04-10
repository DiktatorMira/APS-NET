using Dz09._04._2024.Models;
using Dz09._04._2024.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dz09._04._2024.Controllers {
    public class MessageController : Controller {
        private readonly IRepository repository;
        public MessageController(IRepository rep) => repository = rep;
        public async Task<IActionResult> Index([FromServices] IRepository rep) {
            if (HttpContext.Session.GetString("Login") != null) {
                var messages = await rep.GetAllMessagesAsync();
                return View(new CombinedMessages{ Messages = messages });
            } else return Redirect("/Authentication/Login");
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
        public IActionResult ToRegister() { return Redirect("/Registration/Register");}
        public IActionResult ToLogin() { return Redirect("/Authentication/Login"); }
        public IActionResult Logout()  {
            HttpContext.Session.Clear();
            return Redirect("/Authentication/Login");
        }
    }
}
