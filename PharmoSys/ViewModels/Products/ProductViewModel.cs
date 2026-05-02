using PharmoSys.Commands;
using PharmoSys.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using PharmoSys.Core.Models;


namespace PharmoSys.ViewModels.Products
{
    internal class ProductViewModel:BaseViewModel
    {
        public ObservableCollection<Product> Products { get; set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public ICommand AddProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }

        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>();

            LoadProducts();

            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            UpdateProductCommand = new RelayCommand(UpdateProduct);
        }

        private void LoadProducts()
        {
            Products.Add(new Product
            {
                Name = "Paracetamol",
                Price = 20,
                Stock = 100
            });
        }

        private void AddProduct(object obj)
        {
            Products.Add(new Product
            {
                Name = "New Medicine",
                Price = 100,
                Stock = 50
            });
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedProduct != null)
            {
                Products.Remove(SelectedProduct);
            }
        }

        private void UpdateProduct(object obj)
        {
            // update logic later via service
        }
    }
}
