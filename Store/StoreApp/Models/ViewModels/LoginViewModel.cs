using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.ViewModels
{
    public class LoginViewModel
    {
        private string? _returnUrl;
        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }

        public string ReturnUrl {
            get{
               if(_returnUrl is null){
                return "/";
               }
              else{
                return _returnUrl;
              }
            }
            set => _returnUrl = value;
         }
    }
} 