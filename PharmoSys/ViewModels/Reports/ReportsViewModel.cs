using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using PharmoSys.Commands;
using PharmoSys.Services;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PharmoSys.ViewModels.Reports
{
    public class TopProductModel
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }

    internal class ReportsViewModel : BaseViewModel
    {
        private readonly ReportService _reportService;

        private DateTime _startDate = DateTime.Today.AddDays(-30);
        public DateTime StartDate
        {
            get => _startDate;
            set { SetProperty(ref _startDate, value); _ = LoadReportDataAsync(); }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set { SetProperty(ref _endDate, value); _ = LoadReportDataAsync(); }
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        private int _totalSalesCount;
        public int TotalSalesCount
        {
            get => _totalSalesCount;
            set => SetProperty(ref _totalSalesCount, value);
        }

        public ObservableCollection<ISeries> SalesChartSeries { get; set; }
        public ObservableCollection<Axis> XAxes { get; set; }

        public ObservableCollection<TopProductModel> TopProducts { get; set; }

        public ICommand RefreshCommand { get; }

        public ReportsViewModel()
        {
            _reportService = new ReportService();
            TopProducts = new ObservableCollection<TopProductModel>();
            SalesChartSeries = new ObservableCollection<ISeries>();
            XAxes = new ObservableCollection<Axis> { new Axis { Labels = new string[] { } } };

            RefreshCommand = new RelayCommand((System.Func<object, Task>)(async _ => await LoadReportDataAsync()));

            _ = LoadReportDataAsync();
        }

        private async Task LoadReportDataAsync()
        {
            if (StartDate > EndDate) return;

            // 1. Get Sales
            var sales = await _reportService.GetSalesByDateRangeAsync(StartDate, EndDate);
            TotalRevenue = sales.Sum(s => s.FinalAmount);
            TotalSalesCount = sales.Count;

            // 2. Prepare Chart Data (Group by Date)
            var groupedSales = sales
                .GroupBy(s => s.SaleDate.Date)
                .Select(g => new { Date = g.Key, Total = (double)g.Sum(s => s.FinalAmount) })
                .OrderBy(g => g.Date)
                .ToList();

            var dates = groupedSales.Select(g => g.Date.ToString("MMM dd")).ToArray();
            var values = groupedSales.Select(g => g.Total).ToArray();

            SalesChartSeries.Clear();
            if (values.Any())
            {
                SalesChartSeries.Add(new LineSeries<double>
                {
                    Values = values,
                    Name = "Revenue",
                    Fill = new SolidColorPaint(SKColors.LightSeaGreen.WithAlpha(50)),
                    Stroke = new SolidColorPaint(SKColors.Teal) { StrokeThickness = 3 },
                    GeometrySize = 10,
                    GeometryStroke = new SolidColorPaint(SKColors.Teal) { StrokeThickness = 3 }
                });

                XAxes[0].Labels = dates;
            }
            else
            {
                 XAxes[0].Labels = new string[] { "No Data" };
            }

            // 3. Top Products
            var top = await _reportService.GetTopSellingProductsAsync(StartDate, EndDate);
            TopProducts.Clear();
            foreach (var kvp in top)
            {
                TopProducts.Add(new TopProductModel { ProductName = kvp.Key, TotalSold = kvp.Value });
            }
        }
    }
}
