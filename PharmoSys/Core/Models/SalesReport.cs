using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Core.Models
{
    public class SalesReport
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
    }
}
