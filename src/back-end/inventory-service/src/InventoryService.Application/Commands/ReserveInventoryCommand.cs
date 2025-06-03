namespace InventoryService.Application.Commands;

public record ReserveInventoryCommand(Guid ProductId, int Quantity) : IRequest<Unit>;
