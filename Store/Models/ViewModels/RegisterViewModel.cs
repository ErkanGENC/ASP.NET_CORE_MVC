using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
} 