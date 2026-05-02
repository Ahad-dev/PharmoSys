using Microsoft.EntityFrameworkCore;
using PharmoSys.Core.Interfaces;
using PharmoSys.Core.Mappers;
using PharmoSys.Core.Models;
using PharmoSys.Data.Context;
using PharmoSys.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmoSys.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PharmoSysDbContext _db;

        public ProductRepository()
        {
            _db = new PharmoSysDbContext();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var entities = await _db.Products
                .AsNoTracking()
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .ToListAsync();
            return entities.Select(e => e.ToModel()).ToList();
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            var entity = await _db.Products
                .AsNoTracking()
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            return entity?.ToModel();
        }

        public async Task AddAsync(Product product)
        {
            var entity = new ProductEntity
            {
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ExpiryDate = product.ExpiryDate,
                SupplierId = product.SupplierId
            };
            _db.Products.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var entity = await _db.Products.FindAsync(product.ProductId);
            if (entity == null) return;

            entity.Name = product.Name;
            entity.Category = product.Category;
            entity.Price = product.Price;
            entity.StockQuantity = product.StockQuantity;
            entity.ExpiryDate = product.ExpiryDate;
            entity.SupplierId = product.SupplierId;

            _db.Products.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var entity = await _db.Products.FindAsync(productId);
            if (entity != null)
            {
                _db.Products.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            var suppliers = await _db.Suppliers.ToListAsync();
            return suppliers.Select(s => s.ToModel()).ToList();
        }
    }
}
