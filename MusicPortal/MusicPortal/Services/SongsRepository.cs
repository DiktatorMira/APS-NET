using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Services {
    public interface ISongsRepository {
        Task<IEnumerable<Genre>> GetGenres();
        Task<IEnumerable<Performer>> GetPerformers();
        Task<Genre> GetGenreById(int genreId);
        Task<Performer> GetPerformerById(int performerId);
        Task AddGenre(Genre genre);
        Task AddPerformer(Performer performer);
        void DeleteGenre(Genre genre);
        void DeletePerformer(Performer performer);
        Task SaveDb();
    }
    public class SongsRepository : ISongsRepository {
        private readonly Context db;
        public SongsRepository(Context context) => db = context;
        public async Task<IEnumerable<Genre>> GetGenres() { return await db.Genres.ToListAsync(); }
        public async Task<IEnumerable<Performer>> GetPerformers() { return await db.Performers.ToListAsync(); }
        public async Task<Genre> GetGenreById(int genreId) {
            return await db.Genres.FirstOrDefaultAsync(g => g.Id == genreId);
        }
        public async Task<Performer> GetPerformerById(int performerId) {
            return await db.Performers.FirstOrDefaultAsync(p => p.Id == performerId);
        }
        public async Task AddGenre(Genre genre) => await db.Genres.AddAsync(genre);
        public async Task AddPerformer(Performer performer) => await db.Performers.AddAsync(performer);
        public void DeleteGenre(Genre genre) => db.Genres.Remove(genre);
        public void DeletePerformer(Performer performer) => db.Performers.Remove(performer);
        public async Task SaveDb() => await db.SaveChangesAsync();
    }
}