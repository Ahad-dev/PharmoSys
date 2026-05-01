using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Models
{
    class Supplier
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
