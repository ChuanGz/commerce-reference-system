namespace InventoryService.Application.Commands;

public record ReleaseReservedInventoryCommand(Guid ProductId, int Quantity) : IRequest<Unit>;
