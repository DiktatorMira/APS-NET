using AutoMapper;
using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Repositories;

namespace MusicPortal.BLL.Services {
    public interface IGenreService {
        Task<IEnumerable<GenreDTO>> GetGenres();
        Task<GenreDTO> GetGenreById(int genreId);
        Task<GenreDTO> GetGenreByName(string name);
        Task AddGenre(GenreDTO model);
        void UpdateGenre(GenreDTO model);
        Task DeleteGenre(int genreId);
        Task Save();
    }
    public class GenreService : IGenreService {
        ISaveUnit db { get; set; }
        public GenreService(ISaveUnit su) => db = su;
        public async Task<IEnumerable<GenreDTO>> GetGenres() {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Genre, GenreDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDTO>>(await db.Genres.GetAll());
        }
        public async Task<GenreDTO> GetGenreById(int genreId) {
            var genre = await db.Genres.GetById(genreId);
            if (genre == null) throw new ValidationException("Неверный жанр!", "");
            return new GenreDTO {
                Id = genre.Id,
                Name = genre.Name
            };
        }
        public async Task<GenreDTO> GetGenreByName(string name) {
            var genre = await db.Genres.GetByStr(name);
            if (genre == null) throw new ValidationException("Неверный жанр!", "");
            return new GenreDTO {
                Id = genre.Id,
                Name = genre.Name
            };
        }
        public async Task AddGenre(GenreDTO model) {
            await db.Genres.Add(new Genre {
                Id = model.Id,
                Name = model.Name,
            });
        }
        public void UpdateGenre(GenreDTO model) {
            db.Genres.Update(new Genre {
                Id = model.Id,
                Name = model.Name
            });
        }
        public async Task DeleteGenre(int genreId) => await db.Genres.Delete(genreId);
        public async Task Save() => await db.Save();
    }
}