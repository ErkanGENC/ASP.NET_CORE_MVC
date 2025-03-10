using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Entities.Models;

namespace StoreApp.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            // Http oturumunu al
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            // Oturumdan sepeti al veya yeni oluştur
            SessionCart cart = session?.GetString("Cart") != null
                ? JsonSerializer.Deserialize<SessionCart>(session.GetString("Cart"))
                : new SessionCart();

            // Sepete session'ı bağla
            cart.Session = session;
            return cart;
        }

        // Sepeti güncelleyen metodları override ederek her değişiklikte oturumu da güncelliyoruz
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            SaveSession();
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            SaveSession();
        }

        public override void Clear()
        {
            base.Clear();
            SaveSession();
        }

        // Session'a sepeti kaydeder
        private void SaveSession()
        {
            Session.SetString("Cart", JsonSerializer.Serialize(this));
        }
    }
} 