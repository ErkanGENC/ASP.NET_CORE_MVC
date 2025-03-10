using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
} 