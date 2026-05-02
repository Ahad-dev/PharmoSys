using PharmoSys.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmoSys.Core.Interfaces
{
    public interface ISupplierRepository
    {
        Task<List<SupplierEntity>> GetAllSuppliersAsync();
        Task<SupplierEntity?> GetSupplierByIdAsync(int id);
        Task AddSupplierAsync(SupplierEntity supplier);
        Task UpdateSupplierAsync(SupplierEntity supplier);
        Task DeleteSupplierAsync(int id);
    }
}
