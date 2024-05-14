using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dz13._05._024.Models;

namespace Dz13._05._024.Pages {
    public class DetailsModel : PageModel {
        private readonly IRepository rep;
        public Films? Film { get; set; } = default!;
        public DetailsModel(IRepository r) => rep = r;
        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) return NotFound();
            Film = await rep.GetFilm((int)id);
            return Page();
        }
    }
}