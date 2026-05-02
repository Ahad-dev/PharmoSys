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
    public class StockRepository : IStockRepository
    {
        private readonly PharmoSysDbContext _db;

        public StockRepository()
        {
            _db = new PharmoSysDbContext();
        }

        public async Task<List<StockHistoryEntity>> GetStockHistoriesAsync()
        {
            return await _db.StockHistories
                .AsNoTracking()
                .Include(sh => sh.Product)
                .OrderByDescending(sh => sh.ChangeDate)
                .ToListAsync();
        }

        public async Task<bool> AddStockAsync(int productId, int quantityToAdd)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var product = await _db.Products.FindAsync(productId);
                if (product == null) return false;

                // 1. Update Product
                product.StockQuantity += quantityToAdd;
                _db.Products.Update(product);

                // 2. Add StockHistory
                var history = new StockHistoryEntity
                {
                    ProductId = productId,
                    ChangeType = "RESTOCK",
                    Quantity = quantityToAdd,
                    ChangeDate = DateTime.Now
                };
                _db.StockHistories.Add(history);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
