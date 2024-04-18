namespace MusicPortal.Models {
    public enum SortState  {
        TitleAsc,    // по названию песни по возрастанию
        TitleDesc,   // по названию песни по убыванию
        GenreAsc, // по названию жанра по возрастанию
        GenreDesc,    // по названию жанра по убыванию
        PerformerAsc, // по фио автора по возрастанию
        PerformerDesc,    // по фио автора по убыванию
    }
    public class SortVM {
        public SortState TitleSort { get; set; } // значение для сортировки по названию песни
        public SortState GenreSort { get; set; } // значение для сортировки по названию жанра
        public SortState PerformerSort { get; set; } // значение для сортировки по фио автора
        public SortState Current { get; set; } // значение свойства, выбранного для сортировки
        public bool Up { get; set; } // Сортировка по возрастанию или убыванию
        public SortVM(SortState sortOrder) {
            TitleSort = SortState.TitleAsc;
            GenreSort = SortState.GenreAsc;
            PerformerSort = SortState.PerformerAsc;

            TitleSort = sortOrder == SortState.TitleAsc ? SortState.TitleDesc : SortState.TitleAsc;
            GenreSort = sortOrder == SortState.GenreAsc ? SortState.GenreDesc : SortState.GenreAsc;
            PerformerSort = sortOrder == SortState.PerformerAsc ? SortState.PerformerDesc : SortState.PerformerDesc;
            Current = sortOrder;
        }
    }
}