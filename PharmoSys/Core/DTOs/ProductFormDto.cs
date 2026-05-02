using System;

namespace PharmoSys.Core.DTOs
{
    public class ProductFormDto
    {
        public int ProductId { get; set; }   // 0 for new
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int SupplierId { get; set; }
    }
}
