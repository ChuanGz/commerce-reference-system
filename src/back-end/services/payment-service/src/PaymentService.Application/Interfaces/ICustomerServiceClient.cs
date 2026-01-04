namespace PaymentService.Application.Interfaces {
    public interface ICustomerServiceClient {
        Task<CustomerInfo?> GetCustomerAsync(
            Guid customerId,
            CancellationToken cancellationToken = default
        );
        Task<bool> ValidateCustomerAsync(
            Guid customerId,
            CancellationToken cancellationToken = default
        );
    }

    public class CustomerInfo {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
