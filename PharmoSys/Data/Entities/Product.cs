using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    public class ProductEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int SupplierId { get; set; }
        public virtual SupplierEntity Supplier { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<SaleItemEntity> SaleItems { get; set; }
        public virtual ICollection<StockHistoryEntity> StockHistories { get; set; }

    }
}
