using System.ComponentModel.DataAnnotations;

namespace Dz03._04._2024.Annotations {
    public class FutureDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value != null) {
                if ((DateTime)value > DateTime.Now) return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success!;
        }
    }
}
