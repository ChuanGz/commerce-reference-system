using InventoryService.Domain.Entities;

namespace InventoryService.Application.Queries
{
    public record GetAllInventoryQuery() : IRequest<List<Inventory>>;
}
