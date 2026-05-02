using PharmoSys.Data.Entities;
using PharmoSys.Helpers;
using System.Windows;

namespace PharmoSys.Views.Admin
{
    public partial class UserFormDialog : Window
    {
        public UserEntity UserEntity { get; private set; }

        public UserFormDialog(UserEntity user = null)
        {
            InitializeComponent();
            if (user != null)
            {
                UserEntity = user;
                txtUsername.Text = user.Username;
                txtFullName.Text = user.FullName;
                txtPhone.Text = user.Phone;
                txtRoleId.Text = user.RoleId.ToString();
                txtUsername.IsEnabled = false; // Cannot change username
            }
            else
            {
                UserEntity = new UserEntity();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || 
                string.IsNullOrWhiteSpace(txtFullName.Text) || 
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtRoleId.Text))
            {
                MessageBox.Show("Please fill all required fields (Username, Full Name, Phone, Role ID).");
                return;
            }

            if (!int.TryParse(txtRoleId.Text, out int roleId))
            {
                MessageBox.Show("Role ID must be a number.");
                return;
            }

            UserEntity.Username = txtUsername.Text;
            UserEntity.FullName = txtFullName.Text;
            UserEntity.Phone = txtPhone.Text;
            UserEntity.RoleId = roleId;

            if (!string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                UserEntity.PasswordHash = SecurityHelper.HashPassword(txtPassword.Password);
            }
            else if (UserEntity.UserId == 0) // New user requires password
            {
                MessageBox.Show("Password is required for new users.");
                return;
            }

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
