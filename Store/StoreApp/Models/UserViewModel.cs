using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        
        [Display(Name = "E-posta")]
        public string Email { get; set; }
        
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "E-posta Onaylı")]
        public bool EmailConfirmed { get; set; }
        
        [Display(Name = "Roller")]
        public List<string> Roles { get; set; } = new List<string>();

        public bool IsAdmin { get; set; }
    }

    public class UserEditViewModel
    {
        public UserEditViewModel()
        {
            UserRoles = new List<string>();
            AllRoles = new List<string>();
        }

        public string Id { get; set; }
        
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }
        
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "E-posta Onaylı")]
        public bool EmailConfirmed { get; set; }
        
        [Display(Name = "Kullanıcı Rolleri")]
        public List<string> UserRoles { get; set; }
        
        [Display(Name = "Tüm Roller")]
        public List<string> AllRoles { get; set; }

        public bool IsAdmin { get; set; }
    }
} 