using PharmoSys.Core.Interfaces;
using PharmoSys.Core.Models;
using PharmoSys.Core.Store;
using PharmoSys.Data.Entities;
using PharmoSys.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmoSys.Services
{
    public class SaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public SaleService()
        {
            _saleRepository = new SaleRepository();
            _productRepository = new ProductRepository();
        }

        public async Task<bool> ProcessSaleAsync(IEnumerable<CartItem> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                return false;

            // 1. Calculate Totals
            decimal totalAmount = cartItems.Sum(c => c.Subtotal);
            decimal discount = 0; // Future enhancement
            decimal tax = 0;      // Future enhancement
            decimal finalAmount = totalAmount - discount + tax;

            // 2. Validate Stock Availability
            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for {item.Name}. Available: {product?.StockQuantity ?? 0}, Requested: {item.Quantity}");
                }
            }

            // 3. Prepare Entities
            var saleEntity = new SaleEntity
            {
                UserId = AppSession.CurrentUser?.Id ?? 1, // Fallback to 1 if not logged in during testing
                TotalAmount = totalAmount,
                Discount = discount,
                Tax = tax,
                FinalAmount = finalAmount,
                PaymentMethod = "Cash", // Defaulting to Cash
                SaleDate = DateTime.Now
            };

            var saleItemEntities = new List<SaleItemEntity>();
            var stockHistories = new List<StockHistoryEntity>();

            foreach (var item in cartItems)
            {
                saleItemEntities.Add(new SaleItemEntity
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Subtotal = item.Subtotal
                });

                stockHistories.Add(new StockHistoryEntity
                {
                    ProductId = item.ProductId,
                    ChangeType = "SALE",
                    Quantity = -item.Quantity, // Negative to denote deduction
                    ChangeDate = DateTime.Now
                });
            }

            // 4. Execute Transaction
            return await _saleRepository.CreateSaleTransactionAsync(saleEntity, saleItemEntities, stockHistories);
        }
    }
}
