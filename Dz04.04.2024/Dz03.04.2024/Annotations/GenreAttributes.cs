using System.ComponentModel.DataAnnotations;

namespace Dz03._04._2024.Annotations {
    public class GenreAttribute : ValidationAttribute {
        private static string[]? genres;
        public GenreAttribute(string[] Genres) => genres = Genres;
        public override bool IsValid(object? value) {
            if (value != null) {
                for (int i = 0; i < genres!.Length; i++) {
                    if (value.ToString() == genres[i]) return false;
                }
            }
            return true;
        }
    }
}
