using PharmoSys.ViewModels.Auth;
using System.Windows;
using System.Windows.Controls;

namespace PharmoSys.Views.Auth
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            var vm = new LoginViewModel();
            vm.CloseAction = new System.Action(this.Close);
            DataContext = vm;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
