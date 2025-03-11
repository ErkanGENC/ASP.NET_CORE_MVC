using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models.ViewModels
{
    public class LoginViewModel
    {
        private string? _returnUrl;
        [Required(ErrorMessage = "Kullanıcı adı veya e-posta adresi zorunludur.")]
        [Display(Name = "Kullanıcı Adı / E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
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