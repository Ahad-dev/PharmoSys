using PharmoSys.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmoSys.Core.Interfaces
{
    public interface ISaleRepository
    {
        Task<bool> CreateSaleTransactionAsync(SaleEntity sale, List<SaleItemEntity> saleItems, List<StockHistoryEntity> stockHistories);
    }
}
