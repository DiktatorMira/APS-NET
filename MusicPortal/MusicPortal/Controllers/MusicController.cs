using Microsoft.AspNetCore.Mvc;
using MusicPortal.Services;
using Microsoft.AspNetCore.Http;
using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Controllers {
    public class MusicController : Controller {
        private readonly IUsersRepository usersRep;
        private readonly ISongsRepository songsRep;
        public MusicController(IUsersRepository urep, ISongsRepository srep) {
            usersRep = urep;
            songsRep = srep;
        }
        public async Task<IActionResult> Index(int genre = 0, int performer = 0, int page = 1,
            SortState sortOrder = SortState.TitleAsc) {
            if (HttpContext.Session.GetString("Login") != null) {
                IQueryable<Song> songs = songsRep.GetQuerySongs();
                // фильтрация
                if (genre != 0) songs = songs.Where(s => s.GenreId == genre);
                if (performer != 0) songs = songs.Where(s => s.ArtistId == performer);

                // сортировка
                songs = sortOrder switch {
                    SortState.TitleDesc => songs.OrderByDescending(s => s.Title),
                    SortState.TitleAsc => songs.OrderBy(s => s.Title),
                    SortState.GenreDesc => songs.OrderByDescending(s => s.Genre!.Name),
                    SortState.GenreAsc => songs.OrderBy(s => s.Genre!.Name),
                    SortState.PerformerDesc => songs.OrderByDescending(s => s.Performer!.FullName),
                    SortState.PerformerAsc => songs.OrderBy(s => s.Performer!.FullName),
                    _ => songs.OrderBy(s => s.Title),
                };

                // пагинация
                var count = await songs.CountAsync();
                var items = await songs.Skip((page - 1) * 6).Take(6).ToListAsync();

                // формируем модель представления
                return View(new IndexVM(
                    items,
                    new PageVM(count, page, 6),
                    new FilterVM(await songsRep.GetGenres(), await songsRep.GetPerformers(), genre, performer),
                    new SortVM(sortOrder)
                ));
            }
            else return Redirect("/Authorization/Login");
        }
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return Redirect("/Authorization/Login");
        }
        public async Task<IActionResult> DeleteSong(int songId) {
            songsRep.DeleteSong(await songsRep.GetSongById(songId));
            await songsRep.SaveDb();
            return RedirectToAction("Index");
        }
        // Users.cshtml
        public async Task<IActionResult> ToUsers() => View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        public async Task<IActionResult> Authorize(int userId) {
            var user = await usersRep.GetUserById(userId);
            user.IsAuthorized = !user.IsAuthorized;
            await usersRep.SaveDb();
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        public async Task<IActionResult> DeleteUser(int userId) {
            usersRep.DeleteUser(await usersRep.GetUserById(userId));
            await usersRep.SaveDb();
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        // Genres.cshtml
        public async Task<IActionResult> ToGenres() => View("~/Views/Music/Genres.cshtml", await songsRep.GetGenres());
        public async Task<IActionResult> DeleteGenre(int genreId) {
            songsRep.DeleteGenre(await songsRep.GetGenreById(genreId));
            await songsRep.SaveDb();
            return View("~/Views/Music/Genres.cshtml", await songsRep.GetGenres());
        }
        // AddGenre.cshtml
        public IActionResult ToAddGenre() => View("~/Views/Music/AddGenre.cshtml");
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
        public async Task<IActionResult> ToEditGenre(int genreId) => View("~/Views/Music/EditGenre.cshtml", await songsRep.GetGenreById(genreId));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGenre(Genre model) {
            if (ModelState.IsValid) {
                var genre = await songsRep.GetGenreById(model.Id);
                genre.Name = model.Name;
                await songsRep.SaveDb();
                return RedirectToAction("ToGenres");
            }
            return View("~/Views/Music/EditGenre.cshtml", model);
        }
        // Performers.cshtml
        public async Task<IActionResult> ToPerformers() => View("~/Views/Music/Performers.cshtml", await songsRep.GetPerformers());
        public async Task<IActionResult> DeletePerformer(int performerId) {
            songsRep.DeletePerformer(await songsRep.GetPerformerById(performerId));
            await songsRep.SaveDb();
            return View("~/Views/Music/Performers.cshtml", await songsRep.GetPerformers());
        }
        // AddPerformer.cshtml
        public IActionResult ToAddPerformer() => View("~/Views/Music/AddPerformer.cshtml");
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
        public async Task<IActionResult> ToEditPerformer(int performerId) => View("~/Views/Music/EditPerformer.cshtml", await songsRep.GetPerformerById(performerId));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPerformer(Performer model) {
            if (ModelState.IsValid) {
                var performer = await songsRep.GetPerformerById(model.Id);
                performer.FullName = model.FullName;
                await songsRep.SaveDb();
                return RedirectToAction("ToPerformers");
            }
            return View("~/Views/Music/EditPerformer.cshtml", model);
        }
        // AddSong.cshtml
        public async Task<IActionResult> ToAddSong() {
            ViewBag.Genres = await songsRep.GetGenres();
            ViewBag.Performers = await songsRep.GetPerformers();
            return View("~/Views/Music/AddSong.cshtml"); 
        }
        public async Task<IActionResult> AddSong(Song model) {
            if (ModelState.IsValid && !string.IsNullOrEmpty(HttpContext.Session.GetString("Login"))) {

                var user = await usersRep.GetUserByLogin(HttpContext.Session.GetString("Login")!);
                var genre = await songsRep.GetGenreByName(model.Genre!.Name!);
                var performer = await songsRep.GetPerformerByFullName(model.Performer!.FullName!);

                await songsRep.AddSong(new Song {
                    Title = model.Title,
                    UserId = user!.Id,
                    GenreId = genre!.Id,
                    ArtistId = performer!.Id,
                    User = user,
                    Genre = genre,
                    Performer = performer
                });
                await songsRep.SaveDb();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Title", "Ошибка добавления песни!");
            ViewBag.Genres = await songsRep.GetGenres();
            ViewBag.Performers = await songsRep.GetPerformers();
            return View("~/Views/Music/AddSong.cshtml", model);
        }
        // EditSong.cshtml
        public async Task<IActionResult> ToEditSong(int songId) {
            ViewBag.Genres = await songsRep.GetGenres();
            ViewBag.Performers = await songsRep.GetPerformers();
            return View("~/Views/Music/EditSong.cshtml", await songsRep.GetSongById(songId));
        }
        public async Task<IActionResult> EditSong(Song model) {
            if (ModelState.IsValid) {
                var song = await songsRep.GetSongById(model.Id);
                var genre = await songsRep.GetGenreByName(model.Genre!.Name!);
                var performer = await songsRep.GetPerformerByFullName(model.Performer!.FullName!);

                song.Title = model.Title;
                song.GenreId = genre!.Id;
                song.ArtistId = genre.Id;
                song.Genre = genre;
                song.Performer = performer;
                await songsRep.SaveDb();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Title", "Ошибка изменения песни!");
            ViewBag.Genres = await songsRep.GetGenres();
            ViewBag.Performers = await songsRep.GetPerformers();
            return View("~/Views/Music/AddSong.cshtml", model);
        }
    }
}