﻿using Dz06._05._2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dz06._05._2024.Controllers {
    [ApiController]
    [Route("api/Performers")]
    public class PerformersController : ControllerBase {
        private readonly Context db;
        public PerformersController(Context context) => db = context;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Performer>>> GetPerformers() => await db.Performers.ToListAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<Performer>> GetPerformer(int id) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var performer = await db.Performers.SingleOrDefaultAsync(p => p.Id == id);
            if (performer == null) return NotFound();
            return new ObjectResult(performer);
        }
        [HttpPost]
        public async Task<ActionResult<Performer>> AddPerformer(Performer performer) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Performers.Add(performer);
            await db.SaveChangesAsync();
            return Ok(performer);
        }
        [HttpPut]
        public async Task<ActionResult<Performer>> EditPerformer(Performer performer) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Update(performer);
            await db.SaveChangesAsync();
            return Ok(performer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeletePerformer(int id) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var performer = await db.Performers.SingleOrDefaultAsync(m => m.Id == id);
            if (performer == null) return NotFound();
            db.Performers.Remove(performer);
            await db.SaveChangesAsync();
            return Ok(performer);
        }
    }
}