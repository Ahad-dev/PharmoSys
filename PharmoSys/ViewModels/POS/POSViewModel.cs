using PharmoSys.Commands;
using PharmoSys.Core.Models;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PharmoSys.ViewModels.POS
{
    internal class POSViewModel : BaseViewModel
    {
        private readonly ProductService _productService;

        // --- Product catalogue ---
        private ObservableCollection<Product> _allProducts;
        public ObservableCollection<Product> FilteredProducts { get; set; }

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
            set { SetProperty(ref _searchText, value); FilterProducts(); }
        }

        // --- Cart ---
        public ObservableCollection<CartItem> CartItems { get; set; }

        private CartItem _selectedCartItem;
        public CartItem SelectedCartItem
        {
            get => _selectedCartItem;
            set => SetProperty(ref _selectedCartItem, value);
        }

        // --- Totals ---
        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }

        public string TotalDisplay => $"PKR {TotalAmount:N2}";

        // --- Commands ---
        public ICommand AddToCartCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand ClearCartCommand { get; }

        public POSViewModel()
        {
            _productService = new ProductService();
            _allProducts = new ObservableCollection<Product>();
            FilteredProducts = new ObservableCollection<Product>();
            CartItems = new ObservableCollection<CartItem>();

            AddToCartCommand = new RelayCommand(_ => AddToCart(), _ => SelectedProduct != null);
            RemoveItemCommand = new RelayCommand(_ => RemoveItem(), _ => SelectedCartItem != null);
            CheckoutCommand = new RelayCommand(_ => Checkout(), _ => CartItems.Count > 0);
            ClearCartCommand = new RelayCommand(_ => ClearCart(), _ => CartItems.Count > 0);

            Task.Run(async () => await LoadProductsAsync());
        }

        private async Task LoadProductsAsync()
        {
            var list = await _productService.GetAllProductsAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                _allProducts.Clear();
                foreach (var p in list) _allProducts.Add(p);
                FilterProducts();
            });
        }

        private void FilterProducts()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FilteredProducts.Clear();
                var filtered = string.IsNullOrWhiteSpace(SearchText)
                    ? _allProducts
                    : _allProducts.Where(p => p.Name.ToLower().Contains(SearchText.ToLower())
                                           || (p.Category?.ToLower().Contains(SearchText.ToLower()) ?? false));
                foreach (var p in filtered) FilteredProducts.Add(p);
            });
        }

        private void AddToCart()
        {
            if (SelectedProduct == null) return;

            // If already in cart → increase quantity
            var existing = CartItems.FirstOrDefault(c => c.ProductId == SelectedProduct.ProductId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    ProductId = SelectedProduct.ProductId,
                    Name = SelectedProduct.Name,
                    Price = SelectedProduct.Price,
                    Quantity = 1
                });
            }
            CalculateTotal();
        }

        private void RemoveItem()
        {
            if (SelectedCartItem == null) return;
            CartItems.Remove(SelectedCartItem);
            SelectedCartItem = null;
            CalculateTotal();
        }

        private void Checkout()
        {
            if (CartItems.Count == 0) return;

            var confirm = MessageBox.Show(
                $"Confirm checkout?\n\nItems: {CartItems.Count}\nTotal: PKR {TotalAmount:N2}",
                "Checkout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                // TODO: Save sale to DB in Phase 4
                MessageBox.Show($"✅ Sale completed!\nTotal Collected: PKR {TotalAmount:N2}",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearCart();
            }
        }

        private void ClearCart()
        {
            CartItems.Clear();
            TotalAmount = 0;
            OnPropertyChanged(nameof(TotalDisplay));
        }

        private void CalculateTotal()
        {
            TotalAmount = CartItems.Sum(c => c.Subtotal);
            OnPropertyChanged(nameof(TotalDisplay));
        }

        /// <summary>Called by the View after inline quantity edit to refresh the total.</summary>
        public void RecalculateTotal() => CalculateTotal();
    }
}

