using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dz13._05._024.Models;
using Microsoft.AspNetCore.Hosting;

namespace Dz13._05._024.Pages {
    public class CreateModel : PageModel {
        private readonly Context db;
        private readonly IWebHostEnvironment baseDirectory;
        [BindProperty]
        public Films Film { get; set; } = default!;
        public CreateModel(Context context, IWebHostEnvironment web){
            db = context;
            baseDirectory = web;
        }
        public IActionResult OnGet() => Page();
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile) {
            if (!ModelState.IsValid && uploadedFile == null) return Page();
            if (db.Films.Any(f => f.Title == Film.Title)) {
                ModelState.AddModelError("Title", "Такое название фильма уже есть!");
                return Page();
            }
            string uploadsFolder = Path.Combine(baseDirectory.WebRootPath, "Images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (System.IO.File.Exists(filePath)) {
                ModelState.AddModelError("PosterPath", "Файл с таким именем уже существует");
                return Page();
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create)) await uploadedFile.CopyToAsync(fileStream);
            Film.PosterPath = "/Images/" + uniqueFileName; // Путь к сохранённому файлу
            db.Films.Add(Film);
            await db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}