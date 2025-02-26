using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Adres satırı 1 zorunludur")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }

        [Required(ErrorMessage = "Şehir alanı zorunludur")]
        public string? City { get; set; }

        [Required(ErrorMessage = "İlçe alanı zorunludur")]
        public string? District { get; set; }

        [Required(ErrorMessage = "Posta kodu zorunludur")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string? Email { get; set; }

        public bool GiftWrap { get; set; }
        public decimal? OrderTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string OrderStatus { get; set; } = "Beklemede";
        public string? PaymentMethod { get; set; }
        public bool IsPaid { get; set; } = false;

        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
    }
} 