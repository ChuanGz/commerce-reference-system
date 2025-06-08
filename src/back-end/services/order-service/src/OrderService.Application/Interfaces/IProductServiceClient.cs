namespace OrderService.Application.Interfaces;

public interface IProductServiceClient
{
    Task<ProductInfo?> GetProductAsync(
        Guid productId,
        CancellationToken cancellationToken = default
    );
    Task<bool> ValidateProductAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductInfo>> GetProductsAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken = default
    );
}

public class ProductInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
