using Dz03._04._2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dz03._04._2024.Controllers {
    public class FilmsController : Controller {
        FilmsContext db;
        public FilmsController(FilmsContext db) => this.db = db;
        public async Task<IActionResult> Index() {
            IEnumerable<Films> films = await db.films.ToListAsync();
            return View(films);
        }
    }
}
