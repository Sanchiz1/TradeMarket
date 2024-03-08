using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private readonly TradeMarketDbContext _context;
        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await _context.ReceiptsDetails.ToListAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await _context.ReceiptsDetails.Include(c => c.Receipt)
                .Include(r => r.Product)
                .ThenInclude(r => r.Category)
                .ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await _context.ReceiptsDetails.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await _context.ReceiptsDetails.AddAsync(entity);
        }

        public void Update(ReceiptDetail entity)
        {
            _context.ReceiptsDetails.Update(entity);
        }

        public void Delete(ReceiptDetail entity)
        {
            _context.ReceiptsDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _context.ReceiptsDetails.FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) return;

            _context.ReceiptsDetails.Remove(entity);
        }
    }
}
