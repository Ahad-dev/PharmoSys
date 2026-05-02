using PharmoSys.ViewModels.Stock;
using System.Windows;

namespace PharmoSys.Views.Stock
{
    public partial class RestockDialog : Window
    {
        private readonly RestockViewModel _vm;

        public RestockDialog()
        {
            InitializeComponent();
            _vm = new RestockViewModel();
            DataContext = _vm;
            Loaded += async (s, e) => await _vm.InitializeAsync();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool success = await _vm.SaveAsync();
            if (success)
            {
                MessageBox.Show("Stock added successfully! ✅", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
