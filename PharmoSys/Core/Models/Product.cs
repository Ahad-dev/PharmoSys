using System;

namespace PharmoSys.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        // Computed / Business logic
        public bool IsLowStock => StockQuantity < 10;
        public bool IsExpiringSoon => ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now.AddDays(30);
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now;

        public string StockStatus => IsLowStock ? "⚠ Low Stock" : "✔ In Stock";
        public string ExpiryStatus => IsExpired ? "🔴 Expired" : IsExpiringSoon ? "🟡 Expiring Soon" : "🟢 Good";
    }
}

