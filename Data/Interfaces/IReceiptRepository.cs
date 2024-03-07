using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IReceiptRepository : IRepository<Receipt>
    {
        Task<decimal> ToPayAsync(int receiptId);
        Task<IEnumerable<Receipt>> GetAllWithDetailsAsync();

        Task<Receipt> GetByIdWithDetailsAsync(int id);
    }
}
