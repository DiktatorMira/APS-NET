using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dz13._05._024.Models;

namespace Dz13._05._024.Pages {
    public class DeleteModel : PageModel {
        private readonly IRepository rep;
        private readonly IWebHostEnvironment baseDirectory;
        public Films? Film { get; set; } = default!;
        public DeleteModel(IRepository r, IWebHostEnvironment web){
            rep = r;
            baseDirectory = web;
        }
        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null)  return NotFound();
            Film = await rep.GetFilm((int)id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) return NotFound();
            var film = await rep.GetFilm((int)id);
            System.IO.File.Delete(Path.Combine(baseDirectory.WebRootPath, film.PosterPath!.TrimStart('/')));
            await rep.Delete((int)id);
            await rep.Save();
            return RedirectToPage("./Index");
        }
    }
}