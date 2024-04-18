using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Services {
    public interface ISongsRepository {
        Task<IEnumerable<Song>> GetSongs();
        IQueryable<Song> GetQuerySongs();
        Task<IEnumerable<Genre>> GetGenres();
        Task<IEnumerable<Performer>> GetPerformers();
        Task<Song> GetSongById(int songId);
        Task<Genre> GetGenreById(int genreId);
        Task<Genre> GetGenreByName(string name);
        Task<Performer> GetPerformerById(int performerId);
        Task<Performer> GetPerformerByFullName(string fullName);
        Task AddSong(Song song);
        Task AddGenre(Genre genre);
        Task AddPerformer(Performer performer);
        void DeleteSong(Song song);
        void DeleteGenre(Genre genre);
        void DeletePerformer(Performer performer);
        Task SaveDb();
    }
    public class SongsRepository : ISongsRepository {
        private readonly Context db;
        public SongsRepository(Context context) => db = context;
        public async Task<IEnumerable<Song>> GetSongs() => await db.Songs.ToListAsync();
        public IQueryable<Song> GetQuerySongs() => db.Songs;
        public async Task<IEnumerable<Genre>> GetGenres() => await db.Genres.ToListAsync(); 
        public async Task<IEnumerable<Performer>> GetPerformers() => await db.Performers.ToListAsync();
        public async Task<Song> GetSongById(int songId) => await db.Songs.FirstOrDefaultAsync(s => s.Id == songId);
        public async Task<Genre> GetGenreById(int genreId) => await db.Genres.FirstOrDefaultAsync(g => g.Id == genreId);
        public async Task<Genre> GetGenreByName(string name) => await db.Genres.FirstOrDefaultAsync(g => g.Name == name);
        public async Task<Performer> GetPerformerById(int performerId) => await db.Performers.FirstOrDefaultAsync(p => p.Id == performerId);
        public async Task<Performer> GetPerformerByFullName(string fullName) => await db.Performers.FirstOrDefaultAsync(p => p.FullName == fullName);
        public async Task AddSong(Song song) => await db.Songs.AddAsync(song);
        public async Task AddGenre(Genre genre) => await db.Genres.AddAsync(genre);
        public async Task AddPerformer(Performer performer) => await db.Performers.AddAsync(performer);
        public void DeleteSong(Song song) => db.Songs.Remove(song);
        public void DeleteGenre(Genre genre) => db.Genres.Remove(genre);
        public void DeletePerformer(Performer performer) => db.Performers.Remove(performer);
        public async Task SaveDb() => await db.SaveChangesAsync();
    }
}