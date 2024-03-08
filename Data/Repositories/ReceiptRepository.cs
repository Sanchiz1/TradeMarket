using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly TradeMarketDbContext _context;
        public ReceiptRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await _context.Receipts.ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await _context.Receipts.Include(c => c.Customer)
                .ThenInclude(c => c.Person)
                .Include(c => c.ReceiptDetails)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.Category)
                .ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await _context.Receipts.FirstAsync(c => c.Id == id);
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Receipts.Include(c => c.Customer)
                .Include(c => c.ReceiptDetails)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<decimal> ToPayAsync(int receiptId)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(Receipt entity)
        {
            await _context.Receipts.AddAsync(entity);
        }

        public void Update(Receipt entity)
        {
            _context.Receipts.Update(entity);
        }

        public void Delete(Receipt entity)
        {
            _context.Receipts.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _context.Receipts.FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) return;

            _context.Receipts.Remove(entity);
        }
    }
}
