using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dz13._05._024.Models;

namespace Dz13._05._024.Pages {
    public class DeleteModel : PageModel {
        private readonly Context db;
        public DeleteModel(Context context) => db = context;
        public Films? Film { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null)  return NotFound();
            Film = await db.Films.FirstOrDefaultAsync(m => m.Id == id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) return NotFound();
            Film = await db.Films.FindAsync(id);
            db.Films.Remove(Film!);
            await db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}