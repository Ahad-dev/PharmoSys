using PharmoSys.Commands;
using PharmoSys.Core.Models;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PharmoSys.ViewModels.Stock
{
    internal class StockViewModel : BaseViewModel
    {
        private readonly StockService _stockService;

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<StockHistory> StockHistories { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddStockCommand { get; }

        public StockViewModel()
        {
            _stockService = new StockService();
            Products = new ObservableCollection<Product>();
            StockHistories = new ObservableCollection<StockHistory>();

            RefreshCommand = new RelayCommand((System.Func<object, Task>)(async _ => await LoadDataAsync()));
            AddStockCommand = new RelayCommand((System.Func<object, Task>)(async _ => await OpenRestockDialogAsync()));

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var products = await _stockService.GetAllProductsAsync();
                var histories = await _stockService.GetStockHistoriesAsync();

                Products.Clear();
                foreach (var p in products) Products.Add(p);

                StockHistories.Clear();
                foreach (var h in histories) StockHistories.Add(h);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OpenRestockDialogAsync()
        {
            var dialog = new Views.Stock.RestockDialog();
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                await LoadDataAsync();
            }
        }
    }
}
