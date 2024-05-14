using Dz13._05._024.Models;
using Microsoft.EntityFrameworkCore;

namespace Dz13._05._024 {
    public interface IRepository {
        Task<IList<Films>> GetFilms();
        Task<Films> GetFilm(int id);
        bool IsTitleExists(string title);
        Task Add(Films item);
        void Update(Films item);
        Task Delete(int id);
        Task Save();
    }
    public class Repository : IRepository {
        private readonly Context db;
        public Repository(Context context) => db = context;
        public async Task<IList<Films>> GetFilms() => await db.Films.ToListAsync();
        public async Task<Films> GetFilm(int id) => await db.Films.FindAsync(id);
        public bool IsTitleExists(string title) => db.Films.Any(f => f.Title == title);
        public async Task Add(Films film) => await db.Films.AddAsync(film);
        public void Update(Films film) => db.Entry(film).State = EntityState.Modified;
        public async Task Delete(int id) => db.Films.Remove(await GetFilm(id));
        public async Task Save() => await db.SaveChangesAsync();
    }
}