using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Models
{
    class SaleItem
    {
        public int SaleItemId { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
