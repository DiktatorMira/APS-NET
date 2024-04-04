using Dz03._04._2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dz03._04._2024.Controllers {
    public class FilmsController : Controller {
        FilmsContext db;
        public FilmsController(FilmsContext db) => this.db = db;
        public async Task<IActionResult> Index() { return View(await db.films.ToListAsync()); }
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
