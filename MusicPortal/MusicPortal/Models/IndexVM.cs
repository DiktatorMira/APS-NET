using MusicPortal.BLL.DTO;

namespace MusicPortal.Models{
    public class IndexVM {
        public IEnumerable<SongDTO>? Songs { get; set; }
        public PageVM? PageVM { get; }
        public FilterVM? FilterVM { get; }
        public SortVM? SortVM { get; }
        public IndexVM(IEnumerable<SongDTO> songs, PageVM pageVM,
            FilterVM filterVM, SortVM sortVM) {
            Songs = songs;
            PageVM = pageVM;
            FilterVM = filterVM;
            SortVM = sortVM;
        }
    }
}