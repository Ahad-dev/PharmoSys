using PharmoSys.Core.Models;
using PharmoSys.Data.Entities;

namespace PharmoSys.Core.Mappers
{
    public static class StockHistoryMapper
    {
        public static StockHistory ToModel(this StockHistoryEntity entity)
        {
            if (entity == null) return null;

            return new StockHistory
            {
                StockId = entity.StockId,
                ProductId = entity.ProductId,
                ProductName = entity.Product?.Name ?? "Unknown Product",
                ChangeType = entity.ChangeType,
                Quantity = entity.Quantity,
                ChangeDate = entity.ChangeDate
            };
        }
    }
}
