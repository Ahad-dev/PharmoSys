using PharmoSys.Core.Models;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace PharmoSys.ViewModels.Products
{
    internal class ProductFormViewModel : BaseViewModel
    {
        private readonly ProductService _service;
        private readonly bool _isEdit;

        // --- Form fields ---
        private int _productId;
        private string _name;
        private string _category;
        private decimal _price;
        private int _stockQuantity;
        private System.DateTime? _expiryDate;
        private int _supplierId;

        public string Name           { get => _name;          set => SetProperty(ref _name, value); }
        public string Category       { get => _category;      set => SetProperty(ref _category, value); }
        public decimal Price         { get => _price;         set => SetProperty(ref _price, value); }
        public int StockQuantity     { get => _stockQuantity; set => SetProperty(ref _stockQuantity, value); }
        public System.DateTime? ExpiryDate { get => _expiryDate; set => SetProperty(ref _expiryDate, value); }
        public int SupplierId        { get => _supplierId;    set => SetProperty(ref _supplierId, value); }

        public string FormTitle => _isEdit ? "Edit Product" : "Add New Product";

        public ObservableCollection<Supplier> Suppliers { get; } = new();

        public ProductFormViewModel()
        {
            _service = new ProductService();
            _isEdit = false;
            Task.Run(LoadSuppliersAsync);
        }

        public ProductFormViewModel(Product existing)
        {
            _service = new ProductService();
            _isEdit = true;
            _productId    = existing.ProductId;
            _name         = existing.Name;
            _category     = existing.Category;
            _price        = existing.Price;
            _stockQuantity = existing.StockQuantity;
            _expiryDate   = existing.ExpiryDate;
            _supplierId   = existing.SupplierId;
            Task.Run(LoadSuppliersAsync);
        }

        private async Task LoadSuppliersAsync()
        {
            var list = await _service.GetAllSuppliersAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Suppliers.Clear();
                foreach (var s in list) Suppliers.Add(s);
            });
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || Price <= 0 || SupplierId == 0)
            {
                MessageBox.Show("Please fill in all required fields correctly.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var product = new Product
            {
                ProductId     = _productId,
                Name          = Name,
                Category      = Category,
                Price         = Price,
                StockQuantity = StockQuantity,
                ExpiryDate    = ExpiryDate,
                SupplierId    = SupplierId
            };

            if (_isEdit)
                await _service.UpdateProductAsync(product);
            else
                await _service.AddProductAsync(product);

            return true;
        }
    }
}
