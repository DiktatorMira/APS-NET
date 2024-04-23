using MusicPortal.DAL.Entities;

namespace MusicPortal.DAL.Repositories {
    public interface IRepository<T> where T : class {
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetQuery();
        Task<T> GetById(int id);
        Task<T> GetByStr(string value);
        Task<bool> IsStr(string value);
        Task Add(T item);
        void Delete(T item);
    }
    public interface ISaveUnit {
        IRepository<User> Users { get; }
        IRepository<Song> Songs { get; }
        IRepository<Genre> Genres { get; }
        IRepository<Performer> Performers { get; }
        Task Save();
    }
}