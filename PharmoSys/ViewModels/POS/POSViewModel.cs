using PharmoSys.Commands;
using PharmoSys.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using PharmoSys.Core.Models;

namespace PharmoSys.ViewModels.POS
{
    internal class POSViewModel:BaseViewModel
    {
        public ObservableCollection<CartItem> CartItems { get; set; }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public ICommand AddToCartCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }

        public POSViewModel()
        {
            CartItems = new ObservableCollection<CartItem>();

            AddToCartCommand = new RelayCommand(AddToCart);
            RemoveItemCommand = new RelayCommand(RemoveItem);
            CheckoutCommand = new RelayCommand(Checkout);
        }

        private void AddToCart(object obj)
        {
            var item = new CartItem
            {
                Name = "Panadol",
                Price = 50,
                Quantity = 1
            };

            CartItems.Add(item);
            CalculateTotal();
        }

        private void RemoveItem(object obj)
        {
            if (obj is CartItem item)
            {
                CartItems.Remove(item);
                CalculateTotal();
            }
        }

        private void Checkout(object obj)
        {
            // call service later
            CartItems.Clear();
            TotalAmount = 0;
        }

        private void CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in CartItems)
            {
                total += item.Subtotal;
            }

            TotalAmount = total;
        }
    }
}
