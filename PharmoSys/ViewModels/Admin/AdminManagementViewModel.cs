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

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public ICommand AddSupplierCommand { get; }
        public ICommand EditSupplierCommand { get; }
        public ICommand DeleteSupplierCommand { get; }

        public AdminManagementViewModel()
        {
            _userRepository = new UserRepository();
            _supplierRepository = new SupplierRepository();

            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            SaveSettingsCommand = new RelayCommand(_ => SaveSettings());

            AddUserCommand = new RelayCommand(async _ => await AddUserAsync());
            EditUserCommand = new RelayCommand(async obj => await EditUserAsync(obj as UserEntity));
            DeleteUserCommand = new RelayCommand(async obj => await DeleteUserAsync(obj as UserEntity));

            AddSupplierCommand = new RelayCommand(async _ => await AddSupplierAsync());
            EditSupplierCommand = new RelayCommand(async obj => await EditSupplierAsync(obj as SupplierEntity));
            DeleteSupplierCommand = new RelayCommand(async obj => await DeleteSupplierAsync(obj as SupplierEntity));

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

        private async Task AddUserAsync()
        {
            var dialog = new Views.Admin.UserFormDialog();
            if (dialog.ShowDialog() == true)
            {
                await _userRepository.AddUserAsync(dialog.UserEntity);
                await LoadDataAsync();
            }
        }

        private async Task EditUserAsync(UserEntity user)
        {
            if (user == null) return;
            var dialog = new Views.Admin.UserFormDialog(user);
            if (dialog.ShowDialog() == true)
            {
                await _userRepository.UpdateUserAsync(dialog.UserEntity);
                await LoadDataAsync();
            }
        }

        private async Task DeleteUserAsync(UserEntity user)
        {
            if (user == null) return;
            if (System.Windows.MessageBox.Show($"Are you sure you want to delete user '{user.Username}'?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
            {
                await _userRepository.DeleteUserAsync(user.UserId);
                await LoadDataAsync();
            }
        }

        private async Task AddSupplierAsync()
        {
            var dialog = new Views.Admin.SupplierFormDialog();
            if (dialog.ShowDialog() == true)
            {
                await _supplierRepository.AddSupplierAsync(dialog.Supplier);
                await LoadDataAsync();
            }
        }

        private async Task EditSupplierAsync(SupplierEntity supplier)
        {
            if (supplier == null) return;
            var dialog = new Views.Admin.SupplierFormDialog(supplier);
            if (dialog.ShowDialog() == true)
            {
                await _supplierRepository.UpdateSupplierAsync(dialog.Supplier);
                await LoadDataAsync();
            }
        }

        private async Task DeleteSupplierAsync(SupplierEntity supplier)
        {
            if (supplier == null) return;
            if (System.Windows.MessageBox.Show($"Are you sure you want to delete supplier '{supplier.SupplierName}'?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
            {
                await _supplierRepository.DeleteSupplierAsync(supplier.SupplierId);
                await LoadDataAsync();
            }
        }
    }
}
