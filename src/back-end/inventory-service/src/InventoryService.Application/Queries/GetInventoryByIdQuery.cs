using InventoryService.Domain.Entities;

namespace InventoryService.Application.Queries;

public record GetInventoryByIdQuery(Guid Id) : IRequest<Inventory?>;
