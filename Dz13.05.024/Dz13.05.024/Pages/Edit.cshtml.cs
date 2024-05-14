using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dz13._05._024.Models;

namespace Dz13._05._024.Pages {
    public class EditModel : PageModel {
        private readonly IRepository rep;
        private readonly IWebHostEnvironment baseDirectory;
        [BindProperty]
        public Films? Film { get; set; } = default!;
        public EditModel(IRepository r, IWebHostEnvironment web) {
            rep = r;
            baseDirectory = web;
        }
        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) return NotFound();
            Film = await rep.GetFilm((int)id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile uploadedFile) {
            if (!ModelState.IsValid && uploadedFile == null) return Page();
            if (rep.IsTitleExists(Film!.Title!)) {
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
            Film!.PosterPath = "/Images/" + uniqueFileName;
            rep.Update(Film!);
            await rep.Save();
            return RedirectToPage("./Index");
        }
    }
}