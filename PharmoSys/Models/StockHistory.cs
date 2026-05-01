using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Models
{
    class StockHistory
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int StockId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ChangeType { get; set; } // SALE / RESTOCK

        public int Quantity { get; set; }

        public DateTime ChangeDate { get; set; } = DateTime.Now;
    }
}
