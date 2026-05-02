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
        private string _name = string.Empty;
        private string _category = string.Empty;
        private decimal _price;
        private int _stockQuantity;
        private System.DateTime? _expiryDate;
        private Supplier _selectedSupplier;

        public string Name           { get => _name;          set => SetProperty(ref _name, value); }
        public string Category       { get => _category;      set => SetProperty(ref _category, value); }
        public decimal Price         { get => _price;         set => SetProperty(ref _price, value); }
        public int StockQuantity     { get => _stockQuantity; set => SetProperty(ref _stockQuantity, value); }
        public System.DateTime? ExpiryDate { get => _expiryDate; set => SetProperty(ref _expiryDate, value); }

        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set => SetProperty(ref _selectedSupplier, value);
        }

        public string FormTitle => _isEdit ? "✏ Edit Product" : "➕ Add New Product";

        public ObservableCollection<Supplier> Suppliers { get; } = new();

        // --- For Add ---
        public ProductFormViewModel()
        {
            _service = new ProductService();
            _isEdit = false;
        }

        // --- For Edit ---
        public ProductFormViewModel(Product existing)
        {
            _service = new ProductService();
            _isEdit = true;
            _productId     = existing.ProductId;
            _name          = existing.Name;
            _category      = existing.Category ?? string.Empty;
            _price         = existing.Price;
            _stockQuantity = existing.StockQuantity;
            _expiryDate    = existing.ExpiryDate;
            // SelectedSupplier is set after suppliers load in InitializeAsync
        }

        /// <summary>Called by the dialog's Loaded event — ensures suppliers are ready before user interacts.</summary>
        public async Task InitializeAsync()
        {
            var list = await _service.GetAllSuppliersAsync();
            Suppliers.Clear();
            foreach (var s in list) Suppliers.Add(s);

            // Restore selected supplier for edit mode
            if (_isEdit)
            {
                foreach (var s in Suppliers)
                {
                    if (s.SupplierId == _productId)  // _productId holds the old SupplierId temporarily
                    {
                        SelectedSupplier = s;
                        break;
                    }
                }
            }
        }

        /// <summary>Called by ProductRepository when it fetches the full product including SupplierId.</summary>
        public void SetSupplierById(int supplierId)
        {
            foreach (var s in Suppliers)
            {
                if (s.SupplierId == supplierId)
                {
                    SelectedSupplier = s;
                    return;
                }
            }
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Product Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Price <= 0)
            {
                MessageBox.Show("Price must be greater than 0.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (SelectedSupplier == null)
            {
                MessageBox.Show("Please select a Supplier.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                SupplierId    = SelectedSupplier.SupplierId
            };

            if (_isEdit)
                await _service.UpdateProductAsync(product);
            else
                await _service.AddProductAsync(product);

            return true;
        }
    }
}

