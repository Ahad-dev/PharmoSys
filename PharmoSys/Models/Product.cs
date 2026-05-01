using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Models
{
    class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<SaleItem> SaleItems { get; set; }
        public ICollection<StockHistory> StockHistories { get; set; }

    }
}
