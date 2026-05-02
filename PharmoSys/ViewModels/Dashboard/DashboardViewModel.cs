using PharmoSys.Core.Models;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PharmoSys.ViewModels.Dashboard
{
    internal class DashboardViewModel : BaseViewModel
    {
        private readonly ReportService _reportService;

        private decimal _totalSalesToday;
        public decimal TotalSalesToday
        {
            get => _totalSalesToday;
            set => SetProperty(ref _totalSalesToday, value);
        }

        private int _totalOrdersToday;
        public int TotalOrdersToday
        {
            get => _totalOrdersToday;
            set => SetProperty(ref _totalOrdersToday, value);
        }

        private int _lowStockItems;
        public int LowStockItems
        {
            get => _lowStockItems;
            set => SetProperty(ref _lowStockItems, value);
        }

        private int _expiringItems;
        public int ExpiringItems
        {
            get => _expiringItems;
            set => SetProperty(ref _expiringItems, value);
        }

        public ObservableCollection<Product> LowStockProducts { get; set; } = new();
        public ObservableCollection<Product> ExpiringProducts { get; set; } = new();

        public DashboardViewModel()
        {
            _reportService = new ReportService();
            _ = LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            TotalSalesToday = await _reportService.GetTotalSalesTodayAsync();
            TotalOrdersToday = await _reportService.GetTotalOrdersTodayAsync();
            
            var lowStock = await _reportService.GetLowStockProductsAsync();
            LowStockItems = lowStock.Count;
            foreach (var p in lowStock)
            {
                LowStockProducts.Add(new Product { Name = p.Name, StockQuantity = p.StockQuantity });
            }

            var expiring = await _reportService.GetExpiringProductsAsync();
            ExpiringItems = expiring.Count;
            foreach (var p in expiring)
            {
                ExpiringProducts.Add(new Product { Name = p.Name, ExpiryDate = p.ExpiryDate });
            }
        }
    }
}
