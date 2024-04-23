using Microsoft.AspNetCore.Mvc.Rendering;
using MusicPortal.BLL.DTO;

namespace MusicPortal.Models {
    public class FilterVM {
        public SelectList Genres { get; } // список жанров
        public SelectList Performers { get; } // список авторов
        public int SelectedGenre { get; } // выбранный жанр
        public int SelectedPerformer { get; } // выбранный автор
        public FilterVM(IEnumerable<GenreDTO> genres, IEnumerable<PerformerDTO> performers, int genre, int performer) {
            Genres = new SelectList(genres, "Id", "Name", genre);
            Performers = new SelectList(performers, "Id", "FullName", performer);
            SelectedGenre = genre;
            SelectedPerformer = performer;
        }
    }
}