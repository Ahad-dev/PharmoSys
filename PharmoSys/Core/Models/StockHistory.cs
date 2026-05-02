using System;

namespace PharmoSys.Core.Models
{
    public class StockHistory
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ChangeType { get; set; }
        public int Quantity { get; set; }
        public DateTime ChangeDate { get; set; }

        public string FormattedDate => ChangeDate.ToString("MMM dd, yyyy HH:mm");
        public string FormattedQuantity => Quantity > 0 ? $"+{Quantity}" : Quantity.ToString();
    }
}
