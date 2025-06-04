namespace InventoryService.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
