using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicPortal.Models{
    public class FilterVM {
        public SelectList Genres { get; } // список жанров
        public SelectList Performers { get; } // список авторов
        public int SelectedGenre { get; } // выбранный жанр
        public int SelectedPerformer { get; } // выбранный автор
        public FilterVM(IEnumerable<Genre> genres, IEnumerable<Performer> performers, int genre, int performer) {
            Genres = new SelectList(genres, "Id", "Name", genre);
            Performers = new SelectList(performers, "Id", "FullName", performer);
            SelectedGenre = genre;
            SelectedPerformer = performer;
        }
    }
    public class PageVM {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public PageVM(int count, int pageNumber, int pageSize) {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
    public class IndexVM {
        public IEnumerable<Song>? Songs { get; set; }
        public PageVM? PageVM { get; }
        public FilterVM? FilterVM { get; }
        public SortVM? SortVM { get; }
        public IndexVM(IEnumerable<Song> songs, PageVM pageVM,
            FilterVM filterVM, SortVM sortVM) {
            Songs = songs;
            PageVM = pageVM;
            FilterVM = filterVM;
            SortVM = sortVM;
        }
    }
}