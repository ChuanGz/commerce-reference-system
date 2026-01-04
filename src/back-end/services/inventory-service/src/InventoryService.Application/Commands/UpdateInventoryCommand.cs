namespace InventoryService.Application.Commands {
    public record UpdateInventoryCommand(Guid Id, int Quantity, string Location) : IRequest<Unit>;
}
