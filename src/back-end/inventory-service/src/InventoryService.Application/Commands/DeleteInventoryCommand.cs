namespace InventoryService.Application.Commands;

public record DeleteInventoryCommand(Guid Id) : IRequest<Unit>;
