using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Context;
using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories {
    public class GenreRepository : IRepository<Genre> {
        private MainContext db;
        public GenreRepository(MainContext context) => db = context;
        public async Task<IEnumerable<Genre>> GetAll() => await db.Genres.ToListAsync();
        public IQueryable<Genre> GetQuery() => db.Genres;
        public async Task<Genre> GetById(int genreId) => await db.Genres.FirstOrDefaultAsync(g => g.Id == genreId);
        public async Task<Genre> GetByStr(string name) => await db.Genres.FirstOrDefaultAsync(g => g.Name == name);
        public async Task<bool> IsStr(string name) => await db.Genres.AnyAsync(g => g.Name == name);
        public async Task Add(Genre genre) => await db.Genres.AddAsync(genre);
        public void Delete(Genre genre) => db.Genres.Remove(genre);
    }
}