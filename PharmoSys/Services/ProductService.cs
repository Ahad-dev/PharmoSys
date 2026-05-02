using PharmoSys.Core.Interfaces;
using PharmoSys.Core.Models;
using PharmoSys.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmoSys.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService()
        {
            _repo = new ProductRepository();
        }

        public Task<List<Product>> GetAllProductsAsync() => _repo.GetAllAsync();

        public Task<List<Supplier>> GetAllSuppliersAsync() => _repo.GetAllSuppliersAsync();

        public async Task AddProductAsync(Product product)
        {
            await _repo.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _repo.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _repo.DeleteAsync(productId);
        }
    }
}
