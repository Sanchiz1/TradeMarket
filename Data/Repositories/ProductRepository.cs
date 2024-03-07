using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TradeMarketDbContext _context;
        public ProductRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _context.Products.Include(c => c.Category)
                .Include(c => c.ReceiptDetails)
                .ThenInclude(r => r.Receipt)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FirstAsync(c => c.Id == id);
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Products.Include(c => c.Category)
                .Include(c => c.ReceiptDetails)
                .ThenInclude(r => r.Receipt)
                .FirstAsync(c => c.Id == id);
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);
        }

        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _context.Products.FirstAsync(c => c.Id == id);

            _context.Products.Remove(entity);
        }
    }
}
