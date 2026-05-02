using System;
using System.Collections.Generic;
using System.Text;

namespace PharmoSys.Data.Entities
{
    public class SupplierEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
