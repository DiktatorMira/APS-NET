using Microsoft.AspNetCore.Mvc;
using MusicPortal.Services;
using Microsoft.AspNetCore.Http;
using MusicPortal.Models;

namespace MusicPortal.Controllers {
    public class MusicController : Controller {
        private readonly IUsersRepository usersRep;
        private readonly ISongsRepository songsRep;
        public MusicController(IUsersRepository urep, ISongsRepository srep) {
            usersRep = urep;
            songsRep = srep;
        }
        public IActionResult Index() {
            if (HttpContext.Session.GetString("Login") != null) return View();
            else return Redirect("/Authorization/Login");
        }
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return Redirect("/Authorization/Login");
        }
        // Users.cshtml
        public async Task<IActionResult> ToUsers() {
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        public async Task<IActionResult> Authorize(int userId) {
            var user = await usersRep.GetUserById(userId);
            if (user != null) {
                user.IsAuthorized = !user.IsAuthorized;
                await usersRep.SaveDb();
            }
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        public async Task<IActionResult> DeleteUser(int userId) {
            var user = await usersRep.GetUserById(userId);
            if (user != null) {
                usersRep.DeleteUser(user);
                await usersRep.SaveDb();
            }
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        // Genres.cshtml
        public async Task<IActionResult> ToGenres() {
            return View("~/Views/Music/Genres.cshtml", await songsRep.GetGenres());
        }
        public async Task<IActionResult> DeleteGenre(int genreId) {
            var genre = await songsRep.GetGenreById(genreId);
            if (genre != null) {
                songsRep.DeleteGenre(genre);
                await songsRep.SaveDb();
            }
            return View("~/Views/Music/Genres.cshtml", await songsRep.GetGenres());
        }
        // AddGenre.cshtml
        public IActionResult ToAddGenre() { return View("~/Views/Music/AddGenre.cshtml"); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGenre(Genre model) {
            if (ModelState.IsValid) {
                await songsRep.AddGenre(new Genre { Name = model.Name });
                await songsRep.SaveDb();
                return RedirectToAction("ToGenres");
            }
            return View("~/Views/Music/AddGenre.cshtml", model);
        }
        //EditGenre.cshtml
        public async Task<IActionResult> ToEditGenre(int genreId) {
            var genre = await songsRep.GetGenreById(genreId);
            if (genre == null) return NotFound();
            return View("~/Views/Music/EditGenre.cshtml", genre);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGenre(Genre model) {
            if (ModelState.IsValid) {
                var genre = await songsRep.GetGenreById(model.Id);
                if (genre == null) return NotFound();
                genre.Name = model.Name;
                await songsRep.SaveDb();
                return RedirectToAction("ToGenres");
            }
            return View("~/Views/Music/EditGenre.cshtml", model);
        }
        // Performers.cshtml
        public async Task<IActionResult> ToPerformers()  {
            return View("~/Views/Music/Performers.cshtml", await songsRep.GetPerformers());
        }
        public async Task<IActionResult> DeletePerformer(int performerId) {
            var performer = await songsRep.GetPerformerById(performerId);
            if (performer != null) {
                songsRep.DeletePerformer(performer);
                await songsRep.SaveDb();
            }
            return View("~/Views/Music/Performers.cshtml", await songsRep.GetPerformers());
        }
        // AddPerformer.cshtml
        public IActionResult ToAddPerformer() { return View("~/Views/Music/AddPerformer.cshtml"); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPerformer(Performer model) {
            if (ModelState.IsValid) {
                await songsRep.AddPerformer(new Performer { FullName = model.FullName });
                await songsRep.SaveDb();
                return RedirectToAction("ToPerformers");
            }
            return View("~/Views/Music/AddPerformer.cshtml", model);
        }
        // EditPerformer.cshtml
        public async Task<IActionResult> ToEditPerformer(int performerId) {
            var performer = await songsRep.GetPerformerById(performerId);
            if (performer == null) return NotFound();
            return View("~/Views/Music/EditPerformer.cshtml", performer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPerformer(Performer model) {
            if (ModelState.IsValid) {
                var performer = await songsRep.GetPerformerById(model.Id);
                if (performer == null) return NotFound();
                performer.FullName = model.FullName;
                await songsRep.SaveDb();
                return RedirectToAction("ToPerformers");
            }
            return View("~/Views/Music/EditPerformer.cshtml", model);
        }
    }
}