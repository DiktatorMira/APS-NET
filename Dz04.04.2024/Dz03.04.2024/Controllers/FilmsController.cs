using Dz03._04._2024.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Threading.Tasks;

namespace Dz03._04._2024.Controllers {
    public class FilmsController : Controller {
        FilmsContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FilmsController(FilmsContext db, IWebHostEnvironment webHostEnvironment){
            this.db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index() { return View(await db.films.ToListAsync()); }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile uploadedFile, [Bind("Id,Title,Director,Genre,Date,PosterPath,Description")] Films film) {
            if (db.films.Any(f => f.Title == film.Title)) ModelState.AddModelError("Title", "Такое название фильма уже есть!");
            if (ModelState.IsValid && uploadedFile != null) {
                // Сохранение загруженного файла
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                if (System.IO.File.Exists(filePath)) ModelState.AddModelError("PosterPath", "Файл с таким именем уже существует");

                using (var fileStream = new FileStream(filePath, FileMode.Create)) await uploadedFile.CopyToAsync(fileStream);
                film.PosterPath = "/Images/" + uniqueFileName; // Путь к сохранённому файлу

                db.Add(film);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else {
                ModelState.AddModelError("", "Форма не валидная!");
                return View(film);
            }
        }
        public async Task<IActionResult> Edit(int? id, IFormFile uploadedFile) {
            if (id == null) return NotFound();
            var film = await db.films.FindAsync(id);
            if (film == null) return NotFound();
            if (db.films.Any(f => f.Title == film.Title)) ModelState.AddModelError("Title", "Такое название фильма уже есть!");
            if (ModelState.IsValid) {
                if (uploadedFile != null) {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    if (System.IO.File.Exists(filePath)) ModelState.AddModelError("PosterPath", "Файл с таким именем уже существует");

                    using (var fileStream = new FileStream(filePath, FileMode.Create)) await uploadedFile.CopyToAsync(fileStream);
                    film.PosterPath = "/Images/" + uniqueFileName; // Обновляем путь к изображению
                }
                TryUpdateModelAsync(film); // Попробуем обновить модель с данными из запроса
                db.SaveChanges(); // Сохраняем все изменения в базе данных
                return RedirectToAction("Index"); // Редирект на страницу со списком фильмов
            }
            else {
                ModelState.AddModelError("", "Форма не валидная!");
                return View(film);
            }
        }
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();
            var film = await db.films.FirstOrDefaultAsync(f => f.Id == id);
            if (film == null) return NotFound();
            return View(film);
        }
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();
            var film = await db.films.FirstOrDefaultAsync(f => f.Id == id);
            if (film == null) return NotFound();
            return View(film);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var film = await db.films.FindAsync(id);
            if (film != null) db.films.Remove(film);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
