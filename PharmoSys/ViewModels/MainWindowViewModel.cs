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
    internal class MainWindowViewModel:INotifyPropertyChanged
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}