using Microsoft.EntityFrameworkCore;
using PharmoSys.Core.Interfaces;
using PharmoSys.Data.Context;
using PharmoSys.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmoSys.Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly PharmoSysDbContext _db;

        public SaleRepository()
        {
            _db = new PharmoSysDbContext();
        }

        public async Task<bool> CreateSaleTransactionAsync(SaleEntity sale, List<SaleItemEntity> saleItems, List<StockHistoryEntity> stockHistories)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1. Add Sale
                _db.Sales.Add(sale);
                await _db.SaveChangesAsync(); // Saves and generates SaleId

                // 2. Link SaleItems to SaleId and Add
                foreach (var item in saleItems)
                {
                    item.SaleId = sale.SaleId;
                    _db.SaleItems.Add(item);

                    // 3. Update Product Stock
                    var product = await _db.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        _db.Products.Update(product);
                    }
                }

                // 4. Add StockHistories
                if (stockHistories != null && stockHistories.Any())
                {
                    foreach(var history in stockHistories)
                    {
                        _db.StockHistories.Add(history);
                    }
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Depending on error handling policy, log exception here
                throw new Exception("Failed to process sale transaction.", ex);
            }
        }
    }
}
