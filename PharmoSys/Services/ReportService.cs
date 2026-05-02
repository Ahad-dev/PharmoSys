using Microsoft.EntityFrameworkCore;
using PharmoSys.Data.Context;
using PharmoSys.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmoSys.Services
{
    public class ReportService
    {
        public async Task<decimal> GetTotalSalesTodayAsync()
        {
            using var db = new PharmoSysDbContext();
            var today = DateTime.Today;
            return await db.Sales
                .Where(s => s.SaleDate >= today && s.SaleDate < today.AddDays(1))
                .SumAsync(s => s.FinalAmount);
        }

        public async Task<int> GetTotalOrdersTodayAsync()
        {
            using var db = new PharmoSysDbContext();
            var today = DateTime.Today;
            return await db.Sales
                .Where(s => s.SaleDate >= today && s.SaleDate < today.AddDays(1))
                .CountAsync();
        }

        public async Task<List<ProductEntity>> GetLowStockProductsAsync(int threshold = 10)
        {
            using var db = new PharmoSysDbContext();
            return await db.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<ProductEntity>> GetExpiringProductsAsync(int daysWarning = 30)
        {
            using var db = new PharmoSysDbContext();
            var limitDate = DateTime.Today.AddDays(daysWarning);
            return await db.Products
                .AsNoTracking()
                .Where(p => p.ExpiryDate <= limitDate)
                .OrderBy(p => p.ExpiryDate)
                .Take(10)
                .ToListAsync();
        }

        // For Reports View
        public async Task<List<SaleEntity>> GetSalesByDateRangeAsync(DateTime start, DateTime end)
        {
            using var db = new PharmoSysDbContext();
            return await db.Sales
                .AsNoTracking()
                .Where(s => s.SaleDate >= start && s.SaleDate <= end)
                .OrderBy(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetTopSellingProductsAsync(DateTime start, DateTime end, int count = 5)
        {
            using var db = new PharmoSysDbContext();
            var query = await db.SaleItems
                .Include(si => si.Product)
                .Where(si => si.Sale.SaleDate >= start && si.Sale.SaleDate <= end)
                .GroupBy(si => si.Product.Name)
                .Select(g => new { ProductName = g.Key, TotalSold = g.Sum(si => si.Quantity) })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .ToListAsync();

            return query.ToDictionary(x => x.ProductName, x => x.TotalSold);
        }

        public async Task<List<SaleEntity>> GetDetailedSalesAsync(DateTime start, DateTime end)
        {
            using var db = new PharmoSysDbContext();
            return await db.Sales
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .AsNoTracking()
                .Where(s => s.SaleDate >= start && s.SaleDate <= end)
                .OrderBy(s => s.SaleDate)
                .ToListAsync();
        }
    }
}
