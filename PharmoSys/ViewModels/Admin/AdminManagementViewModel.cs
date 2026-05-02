using PharmoSys.Commands;
using PharmoSys.Core.Interfaces;
using PharmoSys.Data.Entities;
using PharmoSys.Data.Repositories;
using PharmoSys.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PharmoSys.ViewModels.Admin
{
    public class AdminManagementViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;

        // Settings
        private string _storeName;
        public string StoreName
        {
            get => _storeName;
            set => SetProperty(ref _storeName, value);
        }

        private string _storeAddress;
        public string StoreAddress
        {
            get => _storeAddress;
            set => SetProperty(ref _storeAddress, value);
        }

        private string _storeContact;
        public string StoreContact
        {
            get => _storeContact;
            set => SetProperty(ref _storeContact, value);
        }

        public ObservableCollection<UserEntity> Users { get; set; } = new();
        public ObservableCollection<SupplierEntity> Suppliers { get; set; } = new();

        public ICommand LoadDataCommand { get; }
        public ICommand SaveSettingsCommand { get; }

        public AdminManagementViewModel()
        {
            _userRepository = new UserRepository();
            _supplierRepository = new SupplierRepository();

            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            SaveSettingsCommand = new RelayCommand(_ => SaveSettings());

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // Load Users
            var users = await _userRepository.GetAllUsersAsync();
            Users.Clear();
            foreach (var u in users) Users.Add(u);

            // Load Suppliers
            var suppliers = await _supplierRepository.GetAllSuppliersAsync();
            Suppliers.Clear();
            foreach (var s in suppliers) Suppliers.Add(s);

            // Load Settings
            var settings = SettingsService.LoadSettings();
            StoreName = settings.StoreName;
            StoreAddress = settings.StoreAddress;
            StoreContact = settings.StoreContact;
        }

        private void SaveSettings()
        {
            var settings = new StoreSettings
            {
                StoreName = this.StoreName,
                StoreAddress = this.StoreAddress,
                StoreContact = this.StoreContact
            };
            SettingsService.SaveSettings(settings);
            System.Windows.MessageBox.Show("Settings saved successfully!", "Settings", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
