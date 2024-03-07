using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext _context;
        public ICustomerRepository CustomerRepository { get; private set; }

        public IPersonRepository PersonRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public IProductCategoryRepository ProductCategoryRepository { get; private set; }

        public IReceiptRepository ReceiptRepository { get; private set; }

        public IReceiptDetailRepository ReceiptDetailRepository { get; private set; }

        public UnitOfWork(TradeMarketDbContext context)
        {
            _context = context;
            CustomerRepository = new CustomerRepository(_context);
            ProductRepository = new ProductRepository(_context);
            ProductCategoryRepository = new ProductCategoryRepository(_context);
            ReceiptRepository = new ReceiptRepository(_context);
            ReceiptDetailRepository = new ReceiptDetailRepository(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
