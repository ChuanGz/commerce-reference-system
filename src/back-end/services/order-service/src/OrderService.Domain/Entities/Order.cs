using System.Linq;

namespace OrderService.Domain.Entities {
    public class Order {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();

        public void RecalculateTotalAmount() {
            TotalAmount = OrderItems.Sum(item => item.Quantity * item.UnitPrice);
        }
    }
}
