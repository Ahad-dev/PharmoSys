using PharmoSys.ViewModels.Dashboard;
using PharmoSys.ViewModels.POS;
using PharmoSys.ViewModels.Products;
using PharmoSys.Core.Store;
using PharmoSys.Views.Auth;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PharmoSys.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private object _currentView;
        private string _currentDateTime;

        public object CurrentView
        {
            get { return _currentView; }
            set { SetProperty(ref _currentView, value); }
        }

        public string CurrentDateTime
        {
            get { return _currentDateTime; }
            set { SetProperty(ref _currentDateTime, value); }
        }

        public string LoggedInUserName => $"Logged in as: {AppSession.CurrentUser?.FullName ?? AppSession.CurrentUser?.Username} ({AppSession.CurrentUser?.Role})";

        public ICommand ShowDashboardCommand { get; set; }
        public ICommand ShowPOSCommand { get; set; }
        public ICommand ShowProductsCommand { get; set; }
        public ICommand ShowUsersCommand { get; set; }
        public ICommand ShowReportsCommand { get; set; }
        public ICommand ShowStockCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public bool IsAdmin => AppSession.IsAdmin;
        public bool IsCashier => AppSession.IsCashier;
        public bool IsManager => AppSession.IsManager;
        public bool IsAdminOrManager => IsAdmin || IsManager;
        public bool IsAdminOrCashier => IsAdmin || IsCashier;
        
        public MainWindowViewModel()
        {
            ShowDashboardCommand = new Commands.RelayCommand(_ => ShowDashboard());
            ShowPOSCommand = new Commands.RelayCommand(_ => ShowPOS());
            ShowProductsCommand = new Commands.RelayCommand(_ => ShowProducts());
            ShowUsersCommand = new Commands.RelayCommand(_ => ShowUsers());
            ShowReportsCommand = new Commands.RelayCommand(_ => ShowReports());
            ShowStockCommand = new Commands.RelayCommand(_ => ShowStock());
            LogoutCommand = new Commands.RelayCommand(_ => Logout());

            // Real-time clock setup
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (s, e) => CurrentDateTime = DateTime.Now.ToString("dd MMM yyyy  |  hh:mm:ss tt");
            timer.Start();
            CurrentDateTime = DateTime.Now.ToString("dd MMM yyyy  |  hh:mm:ss tt");

            // Set default view based on role
            if (IsCashier)
            {
                CurrentView = new POSViewModel();
            }
            else
            {
                CurrentView = new DashboardViewModel();
            }
        }

        private void ShowDashboard() => CurrentView = new DashboardViewModel();
        private void ShowPOS() => CurrentView = new POSViewModel();
        private void ShowProducts() => CurrentView = new ProductViewModel();
        private void ShowUsers() => CurrentView = new Views.Admin.AdminManagementView();
        private void ShowReports() => CurrentView = new Views.Reports.ReportsView();
        private void ShowStock() => CurrentView = new Views.Stock.StockView();

        private void Logout()
        {
            AppSession.Logout();
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                var loginView = new LoginView();
                loginView.Show();
                
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        window.Close();
                        break;
                    }
                }
            });
        }
    }
}