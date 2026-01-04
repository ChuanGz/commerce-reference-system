namespace InventoryService.Application.Commands {
    public record CreateInventoryCommand(Guid ProductId, int Quantity, string Location)
        : IRequest<Guid>;
}
