using AutoMapper;
using MusicPortal.BLL.DTO;
using MusicPortal.DAL.Repositories;
using MusicPortal.DAL.Entities;
using System.Numerics;

namespace MusicPortal.BLL.Services {
    public interface ISongService {
        Task<IEnumerable<SongDTO>> GetSongs();
        Task<SongDTO> GetSongById(int songId);
        Task AddSong(SongDTO model);
        Task DeleteSong(SongDTO model);
        Task Save();
    }
    public class SongService : ISongService {
        ISaveUnit db { get; set; }
        public SongService(ISaveUnit su) => db = su;
        public async Task<IEnumerable<SongDTO>> GetSongs() {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Song, SongDTO>()
            .ForMember("User", opt => opt.MapFrom(u => u.User!.Login)).ForMember("Genre", opt => opt.MapFrom(g => g.Genre!.Name)).ForMember("Performer", opt => opt.MapFrom(p => p.Performer!.FullName)));
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<Song>, IEnumerable<SongDTO>>(await db.Songs.GetAll());
        } 
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