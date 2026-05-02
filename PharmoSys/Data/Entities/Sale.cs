using System;
using System.Collections.Generic;
using System.Net.ServerSentEvents;
using System.Text;

namespace PharmoSys.Data.Entities
{
    class SaleEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int SaleId { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        public ICollection<SaleItemEntity> SaleItems { get; set; }
    }
}
