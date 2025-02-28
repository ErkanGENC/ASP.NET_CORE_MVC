using Entities.Models;

namespace StoreApp.Infrastructure.Extensions
{
    public static class OrderExtensions
    {
        public static string GetStatusBadgeClass(this Order order)
        {
            return order.OrderStatus switch
            {
                "Beklemede" => "warning",
                "Onaylandı" => "info",
                "Hazırlanıyor" => "primary",
                "Kargoda" => "success",
                "Tamamlandı" => "success",
                "İptal Edildi" => "danger",
                _ => "secondary"
            };
        }

        public static string FormatOrderNumber(this Order order)
        {
            return $"#{order.OrderId:D6}";
        }

        public static string FormatDateTime(this Order order)
        {
            return order.OrderDate.ToString("dd.MM.yyyy HH:mm");
        }

        public static string FormatPrice(this decimal price)
        {
            return $"₺{price:N2}";
        }

        public static string FormatPrice(this decimal? price)
        {
            return price.HasValue ? $"₺{price.Value:N2}" : "₺0.00";
        }

        public static string GetFullName(this Order order)
        {
            return $"{order.Name} {order.LastName}";
        }

        public static string GetFullAddress(this Order order)
        {
            var address = order.Address1;
            if (!string.IsNullOrEmpty(order.Address2))
                address += $", {order.Address2}";
            address += $", {order.District}/{order.City}";
            if (!string.IsNullOrEmpty(order.PostalCode))
                address += $" - {order.PostalCode}";
            return address;
        }
    }
} 