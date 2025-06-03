using InventoryService.Domain.Entities;

namespace InventoryService.Application.Queries;

public record GetInventoryByProductIdQuery(Guid ProductId) : IRequest<Inventory?>;
