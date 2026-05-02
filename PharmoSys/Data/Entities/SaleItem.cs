using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    class SaleItemEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int SaleItemId { get; set; }

        public int SaleId { get; set; }
        public SaleEntity Sale { get; set; }

        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
