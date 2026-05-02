using PharmoSys.Commands;
using PharmoSys.Core.Models;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PharmoSys.ViewModels.Products
{
    internal class ProductViewModel : BaseViewModel
    {
        private readonly ProductService _service;

        private ObservableCollection<Product> _allProducts;
        public ObservableCollection<Product> Products { get; set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterProducts();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand RefreshCommand { get; }

        public ProductViewModel()
        {
            _service = new ProductService();
            _allProducts = new ObservableCollection<Product>();
            Products = new ObservableCollection<Product>();

            AddProductCommand    = new RelayCommand((Func<object, Task>)(async _ => await OpenAddDialogAsync()));
            EditProductCommand   = new RelayCommand((Func<object, Task>)(async _ => await OpenEditDialogAsync()), _ => SelectedProduct != null);
            DeleteProductCommand = new RelayCommand((Func<object, Task>)(async _ => await DeleteProductAsync()), _ => SelectedProduct != null);
            RefreshCommand       = new RelayCommand((Func<object, Task>)(async _ => await LoadProductsAsync()));

            // Load on startup — already on UI thread
            _ = LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            IsLoading = true;
            try
            {
                // GetAllProductsAsync uses EF Core which runs async — await it directly on UI thread
                var list = await _service.GetAllProductsAsync();
                _allProducts.Clear();
                foreach (var p in list) _allProducts.Add(p);
                FilterProducts();
            }
            finally { IsLoading = false; }
        }

        private void FilterProducts()
        {
            Products.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allProducts
                : _allProducts.Where(p =>
                    p.Name.ToLower().Contains(SearchText.ToLower()) ||
                    (p.Category?.ToLower().Contains(SearchText.ToLower()) ?? false));

            foreach (var p in filtered) Products.Add(p);
        }

        private async Task OpenAddDialogAsync()
        {
            var dialog = new Views.Products.ProductFormDialog();
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                await LoadProductsAsync(); // Immediate refresh — no Task.Run
            }
        }

        private async Task OpenEditDialogAsync()
        {
            if (SelectedProduct == null) return;
            var dialog = new Views.Products.ProductFormDialog(SelectedProduct);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                await LoadProductsAsync(); // Immediate refresh — no Task.Run
            }
        }

        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null) return;
            var result = MessageBox.Show(
                $"Delete '{SelectedProduct.Name}'? This cannot be undone.",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _service.DeleteProductAsync(SelectedProduct.ProductId);
                await LoadProductsAsync();
                MessageBox.Show("Product deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

