using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly TradeMarketDbContext _context;
        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.FirstAsync(c => c.Id == id);
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await _context.ProductCategories.AddAsync(entity);
        }

        public void Update(ProductCategory entity)
        {
            _context.ProductCategories.Update(entity);
        }

        public void Delete(ProductCategory entity)
        {
            _context.ProductCategories.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _context.ProductCategories.FirstAsync(c => c.Id == id);

            _context.ProductCategories.Remove(entity);
        }
    }
}
