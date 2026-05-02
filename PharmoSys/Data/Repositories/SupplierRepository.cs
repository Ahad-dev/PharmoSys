using Microsoft.EntityFrameworkCore;
using PharmoSys.Core.Interfaces;
using PharmoSys.Data.Context;
using PharmoSys.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmoSys.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        public async Task<List<SupplierEntity>> GetAllSuppliersAsync()
        {
            using var _db = new PharmoSysDbContext();
            return await _db.Suppliers.AsNoTracking().ToListAsync();
        }

        public async Task<SupplierEntity?> GetSupplierByIdAsync(int id)
        {
            using var _db = new PharmoSysDbContext();
            return await _db.Suppliers.FindAsync(id);
        }

        public async Task AddSupplierAsync(SupplierEntity supplier)
        {
            using var _db = new PharmoSysDbContext();
            _db.Suppliers.Add(supplier);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(SupplierEntity supplier)
        {
            using var _db = new PharmoSysDbContext();
            _db.Suppliers.Update(supplier);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            using var _db = new PharmoSysDbContext();
            var supplier = await _db.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _db.Suppliers.Remove(supplier);
                await _db.SaveChangesAsync();
            }
        }
    }
}
