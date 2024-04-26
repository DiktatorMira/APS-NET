using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;
using MusicPortal.BLL.Services;
using MusicPortal.BLL.DTO;

namespace MusicPortal.Controllers {
    public class MusicController : Controller {
        private readonly IUserService usersRep;
        private readonly ISongService songsRep;
        private readonly IGenreService genreRep;
        private readonly IPerformerService performerRep;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILangService langService;
        public MusicController(IUserService urep, ISongService srep, IGenreService grep, IPerformerService prep, IWebHostEnvironment host, ILangService lServ) {
            usersRep = urep;
            songsRep = srep;
            genreRep = grep;
            performerRep = prep;
            webHostEnvironment = host;
            langService = lServ;
        }
        public async Task<IActionResult> Index(int genre = 0, int performer = 0, int page = 1,
            SortState sortOrder = SortState.TitleAsc) {
            if (HttpContext.Session.GetString("Login") != null) {
                var songs = await songsRep.GetSongs();
                // фильтрация
                if (genre != 0) songs = songs.Where(s => s.GenreId == genre);
                if (performer != 0) songs = songs.Where(s => s.ArtistId == performer);


                songs = sortOrder switch {
                    SortState.TitleDesc => songs.OrderByDescending(s => s.Title).ToList(),
                    SortState.TitleAsc => songs.OrderBy(s => s.Title).ToList(),
                    SortState.GenreDesc => songs.OrderByDescending(s => s.Genre).ToList(),
                    SortState.GenreAsc => songs.OrderBy(s => s.Genre).ToList(),
                    SortState.PerformerDesc => songs.OrderByDescending(s => s.Performer).ToList(),
                    SortState.PerformerAsc => songs.OrderBy(s => s.Performer).ToList(),
                    _ => songs.OrderBy(s => s.Title).ToList(),
                };

                // пагинация
                var count = songs.Count();
                var items = songs.Skip((page - 1) * 6).Take(6);

                // формируем модель представления
                HttpContext.Session.SetString("path", Request.Path);
                ViewBag.Languages = langService.languageList();
                return View(new IndexVM(
                    items,
                    new PageVM(count, page, 6),
                    new FilterVM(await genreRep.GetGenres(), await performerRep.GetPerformers(), genre, performer),
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
            var song = await songsRep.GetSongById(songId);
            string oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, song.Path!.TrimStart('/'));
            System.IO.File.Delete(oldFilePath);
            await songsRep.DeleteSong(songId);
            return RedirectToAction("Index");
        }
        public ActionResult ChangeCulture(string lang) {
            string? returnUrl = HttpContext.Session.GetString("path") ?? "/Club/Index";
            List<string> cultures = langService.languageList().Select(t => t.ShortName).ToList()!;
            if (!cultures.Contains(lang)) lang = "ru";
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10); // срок хранения куки - 10 дней
            Response.Cookies.Append("lang", lang, option); // создание куки
            return Redirect(returnUrl);
        }
        // Users.cshtml
        public async Task<IActionResult> ToUsers() => View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        public async Task<IActionResult> Authorize(int userId) {
            var user = await usersRep.GetUserById(userId);
            user.IsAuthorized = !user.IsAuthorized;
            usersRep.UpdateUser(user);
            await usersRep.Save();
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        public async Task<IActionResult> DeleteUser(int userId) {
            usersRep?.DeleteUser(userId);
            await usersRep!.Save();
            return View("~/Views/Music/Users.cshtml", await usersRep.GetUsers());
        }
        // Genres.cshtml
        public async Task<IActionResult> ToGenres() => View("~/Views/Music/Genres.cshtml", await genreRep.GetGenres());
        public async Task<IActionResult> DeleteGenre(int genreId) {
            await genreRep.DeleteGenre(genreId);
            await genreRep.Save();
            return View("~/Views/Music/Genres.cshtml", await genreRep.GetGenres());
        }
        // AddGenre.cshtml
        public IActionResult ToAddGenre() => View("~/Views/Music/AddGenre.cshtml");
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGenre(GenreDTO model) {
            if (ModelState.IsValid) {
                await genreRep.AddGenre(model);
                await genreRep.Save();
                return RedirectToAction("ToGenres");
            }
            return View("~/Views/Music/AddGenre.cshtml", model);
        }
        //EditGenre.cshtml
        public async Task<IActionResult> ToEditGenre(int genreId) => View("~/Views/Music/EditGenre.cshtml", await genreRep.GetGenreById(genreId));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGenre(GenreDTO model) {
            if (ModelState.IsValid) {
                genreRep.UpdateGenre(model);
                await genreRep.Save();
                return RedirectToAction("ToGenres");
            }
            return View("~/Views/Music/EditGenre.cshtml", model);
        }
        // Performers.cshtml
        public async Task<IActionResult> ToPerformers() => View("~/Views/Music/Performers.cshtml", await performerRep.GetPerformers());
        public async Task<IActionResult> DeletePerformer(int performerId) {
            await performerRep.DeletePerformer(performerId);
            await performerRep.Save();
            return View("~/Views/Music/Performers.cshtml", await performerRep.GetPerformers());
        }
        // AddPerformer.cshtml
        public IActionResult ToAddPerformer() => View("~/Views/Music/AddPerformer.cshtml");
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPerformer(PerformerDTO model) {
            if (ModelState.IsValid) {
                await performerRep.AddPerformer(model);
                await performerRep.Save();
                return RedirectToAction("ToPerformers");
            }
            return View("~/Views/Music/AddPerformer.cshtml", model);
        }
        // EditPerformer.cshtml
        public async Task<IActionResult> ToEditPerformer(int performerId) => View("~/Views/Music/EditPerformer.cshtml", await performerRep.GetPerformerById(performerId));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPerformer(PerformerDTO model) {
            if (ModelState.IsValid) {
                performerRep.UpdatePerformer(model);
                await performerRep.Save();
                return RedirectToAction("ToPerformers");
            }
            return View("~/Views/Music/EditPerformer.cshtml", model);
        }
        // AddSong.cshtml
        public async Task<IActionResult> ToAddSong() {
            ViewBag.Genres = await genreRep.GetGenres();
            ViewBag.Performers = await performerRep.GetPerformers();
            return View("~/Views/Music/AddSong.cshtml"); 
        }
        public async Task<IActionResult> AddSong(IFormFile uploadedFile, SongDTO model) {
            if (ModelState.IsValid && !string.IsNullOrEmpty(HttpContext.Session.GetString("Login"))) {
                if(uploadedFile == null)  ModelState.AddModelError("Path", Resources.Resource.ChooseFile);
                else {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "musics");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    if (System.IO.File.Exists(filePath)) ModelState.AddModelError("Path", Resources.Resource.FileExist);
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) await uploadedFile.CopyToAsync(fileStream);

                    var user = await usersRep.GetUserByLogin(HttpContext.Session.GetString("Login")!);
                    var genre = await genreRep.GetGenreByName(model.Genre!);
                    var performer = await performerRep.GetPerformerByFullName(model.Performer!);

                    await songsRep.AddSong(new SongDTO {
                        Title = model.Title,
                        Path = "/musics/" + uniqueFileName,
                        UserId = user!.Id,
                        GenreId = genre!.Id,
                        ArtistId = performer!.Id,
                        User = user.Login,
                        Genre = genre.Name,
                        Performer = performer.FullName
                    });
                    await songsRep.Save();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Genres = await genreRep.GetGenres();
            ViewBag.Performers = await performerRep.GetPerformers();
            return View("~/Views/Music/AddSong.cshtml", model);
        }
        // EditSong.cshtml
        public async Task<IActionResult> ToEditSong(int songId) {
            ViewBag.Genres = await genreRep.GetGenres();
            ViewBag.Performers = await performerRep.GetPerformers();
            return View("~/Views/Music/EditSong.cshtml", await songsRep.GetSongById(songId));
        }
        public async Task<IActionResult> EditSong(IFormFile uploadedFile, SongDTO model) {
            if (ModelState.IsValid) {
                var song = await songsRep.GetSongById(model.Id);
                var genre = await genreRep.GetGenreByName(model.Genre!);
                var performer = await performerRep.GetPerformerByFullName(model.Performer!);

                song.Title = model.Title;
                song.GenreId = genre!.Id;
                song.ArtistId = genre.Id;
                song.Genre = genre.Name;
                song.Performer = performer.FullName;

                if (uploadedFile != null) {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "musics");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) await uploadedFile.CopyToAsync(fileStream);

                    string oldFilePath = Path.Combine(webHostEnvironment.WebRootPath, song.Path!.TrimStart('/'));
                    System.IO.File.Delete(oldFilePath);

                    song.Path = "/musics/" + uniqueFileName;
                } else song.Path = song.Path;

                songsRep.UpdateSong(song);
                await songsRep.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Genres = await genreRep.GetGenres();
            ViewBag.Performers = await performerRep.GetPerformers();
            return View("~/Views/Music/EditSong.cshtml", model);
        }
    }
}