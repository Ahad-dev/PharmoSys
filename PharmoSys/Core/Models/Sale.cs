using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Core.Models
{
    public class Sale
    {
        public List<CartItem> Items { get; set; }

        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }

        public decimal FinalAmount => TotalAmount - Discount + Tax;
    }
}
