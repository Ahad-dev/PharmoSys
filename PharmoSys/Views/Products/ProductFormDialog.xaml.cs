using PharmoSys.Core.Models;
using PharmoSys.ViewModels.Products;
using System.Windows;

namespace PharmoSys.Views.Products
{
    public partial class ProductFormDialog : Window
    {
        private readonly ProductFormViewModel _vm;
        private readonly int _existingSupplierId;

        public ProductFormDialog()
        {
            InitializeComponent();
            _vm = new ProductFormViewModel();
            DataContext = _vm;
            Loaded += async (s, e) => await _vm.InitializeAsync();
        }

        public ProductFormDialog(Product existing)
        {
            InitializeComponent();
            _existingSupplierId = existing.SupplierId;
            _vm = new ProductFormViewModel(existing);
            DataContext = _vm;
            Loaded += async (s, e) =>
            {
                await _vm.InitializeAsync();
                // After suppliers are loaded, set the correct one for edit
                _vm.SetSupplierById(_existingSupplierId);
            };
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool success = await _vm.SaveAsync();
            if (success)
            {
                MessageBox.Show("Product saved successfully! ✅", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

