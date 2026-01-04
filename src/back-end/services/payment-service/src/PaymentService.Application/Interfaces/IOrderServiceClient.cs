namespace PaymentService.Application.Interfaces {
    public interface IOrderServiceClient {
        Task<OrderInfo?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<bool> ValidateOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<bool> UpdateOrderPaymentStatusAsync(
            Guid orderId,
            string paymentStatus,
            CancellationToken cancellationToken = default
        );
    }

    public class OrderInfo {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
}
