using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    public class StockHistoryEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int StockId { get; set; }

        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }

        public string ChangeType { get; set; } // SALE / RESTOCK

        public int Quantity { get; set; }

        public DateTime ChangeDate { get; set; } = DateTime.Now;
    }
}
