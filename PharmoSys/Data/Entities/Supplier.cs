using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    class SupplierEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }

        public ICollection<ProductEntity> Products { get; set; }
    }
}
