using PharmoSys.Data.Entities;
using System.Windows;

namespace PharmoSys.Views.Admin
{
    public partial class SupplierFormDialog : Window
    {
        public SupplierEntity Supplier { get; private set; }

        public SupplierFormDialog(SupplierEntity supplier = null)
        {
            InitializeComponent();
            if (supplier != null)
            {
                Supplier = supplier;
                txtName.Text = supplier.SupplierName;
                txtContact.Text = supplier.Contact;
                txtAddress.Text = supplier.Address;
            }
            else
            {
                Supplier = new SupplierEntity();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Supplier Name is required.");
                return;
            }
            Supplier.SupplierName = txtName.Text;
            Supplier.Contact = txtContact.Text;
            Supplier.Address = txtAddress.Text;
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
