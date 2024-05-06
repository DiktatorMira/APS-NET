using Dz06._05._2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dz06._05._2024.Controllers {
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase {
        private readonly Context db;
        public UsersController(Context context) => db = context;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() => await db.Users.ToListAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound(); 
            return user;
        }
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<User>> EditUser(User user) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await db.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}