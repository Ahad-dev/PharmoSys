using System;
using System.Collections.Generic;
using System.Text;
using PharmoSys.Utils;

namespace PharmoSys.ViewModels.Dashboard
{
    internal class DashboardViewModel:BaseViewModel
    {
        private decimal _totalSalesToday;
        public decimal TotalSalesToday
        {
            get => _totalSalesToday;
            set
            {
                _totalSalesToday = value;
                OnPropertyChanged(nameof(TotalSalesToday));
            }
        }

        private int _lowStockItems;
        public int LowStockItems
        {
            get => _lowStockItems;
            set
            {
                _lowStockItems = value;
                OnPropertyChanged(nameof(LowStockItems));
            }
        }

        public DashboardViewModel()
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // dummy data (later come from service)
            TotalSalesToday = 5000;
            LowStockItems = 12;
        }
    }
}
