using System.Text.Json.Serialization;
using StoreApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Entities.Models;

namespace StoreApp.Infrastructure.Extensions
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }

        public override void UpdateQuantity(Product product, int quantity)
        {
            base.UpdateQuantity(product, quantity);
            Session?.SetJson("Cart", this);
        }

        public static SessionCart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
    }

    public static class SessionExtensions
    {
        private static JsonSerializerOptions GetJsonOptions() => new()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            IncludeFields = true
        };

        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value, GetJsonOptions()));
        }

        public static T? GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return string.IsNullOrEmpty(sessionData) 
                ? default(T) 
                : JsonSerializer.Deserialize<T>(sessionData, GetJsonOptions());
        }
    }
} 