namespace OrderService.Application.Interfaces;

public interface IInventoryServiceClient
{
    Task<bool> CheckStockAvailabilityAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    );
    Task<bool> ReserveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    );
    Task<bool> ReleaseStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default
    );
    Task<InventoryInfo?> GetInventoryAsync(
        Guid productId,
        CancellationToken cancellationToken = default
    );
}

public class InventoryInfo
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public string Location { get; set; } = string.Empty;
}
