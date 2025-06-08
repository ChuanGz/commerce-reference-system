using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProductRepository
    {
        private readonly ProductDbContext _context = context;

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products.ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Products.FindAsync([id], cancellationToken);
        }

        public async Task<Product?> GetBySKUAsync(
            string sku,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Products.FirstOrDefaultAsync(
                p => p.SKU == sku,
                cancellationToken
            );
        }

        public async Task<List<Product>> GetByCategoryAsync(
            string category,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .Products.Where(p => p.Category == category)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetActiveProductsAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Products.Where(p => p.IsActive).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken = default
        )
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await GetByIdAsync(id, cancellationToken);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> AnyAsync(
            Expression<Func<Product, bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Products.AnyAsync(predicate, cancellationToken);
        }
    }
}
