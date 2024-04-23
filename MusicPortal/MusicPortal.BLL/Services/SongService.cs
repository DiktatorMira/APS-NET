using AutoMapper;
using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Repositories;
using MusicPortal.DAL.Entities;

namespace MusicPortal.BLL.Services {
    public interface ISongService {
        Task<IEnumerable<SongDTO>> GetSongs();
        IQueryable<SongDTO> GetQuerySongs();
        Task<SongDTO> GetSongById(int songId);
        Task AddSong(SongDTO model);
        Task DeleteSong(SongDTO model);
        Task Save();
    }
    public class SongService : ISongService {
        ISaveUnit db { get; set; }
        public SongService(ISaveUnit su) => db = su;
        public static IMapper ConfigureMapper() {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Song, SongDTO>()
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User!.Login))
                    .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre!.Name))
                    .ForMember(dest => dest.Performer, opt => opt.MapFrom(src => src.Performer!.FullName));
            });
            return new Mapper(config);
        }
        public async Task<IEnumerable<SongDTO>> GetSongs() => ConfigureMapper().Map<IEnumerable<Song>, IEnumerable<SongDTO>>(await db.Songs.GetAll());
        public IQueryable<SongDTO> GetQuerySongs() => db.Songs.GetQuery().Select(song => ConfigureMapper().Map<SongDTO>(song));
        public async Task<SongDTO> GetSongById(int songId) {
            var song = await db.Songs.GetById(songId);
            if (song == null) throw new ValidationException("Неверная песня!", "");
            return new SongDTO {
                Id = song.Id,
                Title = song.Title,
                Path = song.Path,
                UserId = song.UserId,
                GenreId = song.GenreId,
                ArtistId = song.ArtistId,
                User = song.User?.Login,
                Genre = song.Genre?.Name,
                Performer = song.Performer?.FullName
            };
        }
        public async Task AddSong(SongDTO model) {
            await db.Songs.Add(new Song {
                Id = model.Id,
                Title = model.Title,
                Path = model.Path,
                UserId = model.UserId,
                GenreId = model.GenreId,
                ArtistId = model.ArtistId
            });
        }
        public async Task DeleteSong(SongDTO model) => db.Songs.Delete(await db.Songs.GetById(model.Id));
        public async Task Save() => await db.Save();
    }
}