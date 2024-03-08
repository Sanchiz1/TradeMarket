using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    public class ReceiptDetail : BaseEntity
    {
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal DiscountUnitPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public ReceiptDetail() { }
    }
}
