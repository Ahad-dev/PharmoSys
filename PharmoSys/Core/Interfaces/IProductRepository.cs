using PharmoSys.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmoSys.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int productId);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int productId);
        Task<List<Supplier>> GetAllSuppliersAsync();
    }
}
