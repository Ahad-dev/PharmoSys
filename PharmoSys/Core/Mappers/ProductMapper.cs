using PharmoSys.Core.Models;
using PharmoSys.Data.Entities;

namespace PharmoSys.Core.Mappers
{
    public static class ProductMapper
    {
        public static Product ToModel(this ProductEntity entity)
        {
            if (entity == null) return null;
            return new Product
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Category = entity.Category,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity,
                ExpiryDate = entity.ExpiryDate,
                SupplierId = entity.SupplierId,
                SupplierName = entity.Supplier?.SupplierName ?? "N/A"
            };
        }

        public static ProductEntity ToEntity(this Product model)
        {
            if (model == null) return null;
            return new ProductEntity
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Category = model.Category,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                ExpiryDate = model.ExpiryDate,
                SupplierId = model.SupplierId
            };
        }
    }
}
