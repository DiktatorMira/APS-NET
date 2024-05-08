using WebApiServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dz06._05._2024.Controllers {
    [ApiController]
    [Route("api/Genres")]
    public class GenresController : ControllerBase {
        private readonly Context db;
        public GenresController(Context context) => db = context;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres() => await db.Genres.ToListAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var genre = await db.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null) return NotFound();
            return new ObjectResult(genre);
        }
        [HttpPost]
        public async Task<ActionResult<Genre>> AddGenre(Genre genre) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Genres.Add(genre);
            await db.SaveChangesAsync();
            return Ok(genre);
        }
        [HttpPut]
        public async Task<ActionResult<Genre>> EditGenre(Genre genre) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Entry(genre).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeleteGenre(int id) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var genre = await db.Genres.SingleOrDefaultAsync(m => m.Id == id);
            if (genre == null) return NotFound();
            db.Genres.Remove(genre);
            await db.SaveChangesAsync();
            return Ok(genre);
        }
    }
}