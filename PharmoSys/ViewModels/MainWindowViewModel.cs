using PharmoSys.ViewModels.Dashboard;
using PharmoSys.ViewModels.POS;
using PharmoSys.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace PharmoSys.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { SetProperty(ref _currentView, value); }
        }
        public ICommand ShowDashboardCommand { get; set; }
        public ICommand ShowPOSCommand { get; set; }
        public ICommand ShowProductsCommand { get; set; }
        
        public MainWindowViewModel()
        {
            ShowDashboardCommand = new Commands.RelayCommand(_ => ShowDashboard());
            ShowPOSCommand = new Commands.RelayCommand(_ => ShowPOS());
            ShowProductsCommand = new Commands.RelayCommand(_ => ShowProducts());

            // Set default view
            CurrentView = new DashboardViewModel();
        }

        private void ShowDashboard()
        {
            CurrentView = new DashboardViewModel();
        }

        private void ShowPOS()
        {
            CurrentView = new POSViewModel();
        }

        private void ShowProducts()
        {
            CurrentView = new ProductViewModel();
        }
    }
}