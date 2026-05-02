using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }

        // UI / Business logic fields
        public bool IsLowStock => Stock < 10;
        public bool IsExpired => ExpiryDate < DateTime.Now;
    }
}
