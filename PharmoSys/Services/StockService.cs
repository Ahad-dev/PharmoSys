using PharmoSys.Core.Interfaces;
using PharmoSys.Core.Mappers;
using PharmoSys.Core.Models;
using PharmoSys.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmoSys.Services
{
    public class StockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;

        public StockService()
        {
            _stockRepository = new StockRepository();
            _productRepository = new ProductRepository();
        }

        public async Task<List<StockHistory>> GetStockHistoriesAsync()
        {
            var entities = await _stockRepository.GetStockHistoriesAsync();
            return entities.Select(e => e.ToModel()).ToList();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<bool> AddStockAsync(int productId, int quantityToAdd)
        {
            if (quantityToAdd <= 0) return false;
            return await _stockRepository.AddStockAsync(productId, quantityToAdd);
        }
    }
}
