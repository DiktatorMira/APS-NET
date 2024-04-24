using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Context;
using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories {
    public class SongRepository : IRepository<Song> {
        private MainContext db;
        public SongRepository(MainContext context) => db = context;
        public async Task<IEnumerable<Song>> GetAll() => await db.Songs.ToListAsync();
        public async Task<Song> GetById(int songId) => await db.Songs.FirstOrDefaultAsync(s => s.Id == songId);
        public async Task<Song> GetByStr(string title) => await db.Songs.FirstOrDefaultAsync(s => s.Title == title);
        public async Task<bool> IsStr(string title) => await db.Songs.AnyAsync(s => s.Title == title);
        public async Task Add(Song song) => await db.Songs.AddAsync(song);
        public void Delete(Song song) => db.Songs.Remove(song);
    }
}