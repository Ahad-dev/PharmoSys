using PharmoSys.Core.Models;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace PharmoSys.ViewModels.Stock
{
    internal class RestockViewModel : BaseViewModel
    {
        private readonly StockService _stockService;

        public ObservableCollection<Product> Products { get; } = new();

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private int _quantityToAdd;
        public int QuantityToAdd
        {
            get => _quantityToAdd;
            set => SetProperty(ref _quantityToAdd, value);
        }

        public RestockViewModel()
        {
            _stockService = new StockService();
        }

        public async Task InitializeAsync()
        {
            var list = await _stockService.GetAllProductsAsync();
            Products.Clear();
            foreach (var p in list) Products.Add(p);
        }

        public async Task<bool> SaveAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (QuantityToAdd <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return await _stockService.AddStockAsync(SelectedProduct.ProductId, QuantityToAdd);
        }
    }
}
