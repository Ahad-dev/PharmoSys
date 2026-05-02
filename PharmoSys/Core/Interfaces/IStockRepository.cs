using PharmoSys.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmoSys.Core.Interfaces
{
    public interface IStockRepository
    {
        Task<List<StockHistoryEntity>> GetStockHistoriesAsync();
        Task<bool> AddStockAsync(int productId, int quantityToAdd);
    }
}
