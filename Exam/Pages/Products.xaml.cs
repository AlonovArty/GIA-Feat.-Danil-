using Exam.Context;
using Exam.Elements;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Exam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        List<Models.Product> _products;
        public Products()
        {
            InitializeComponent();
            LoadInterfaceByRoles();
            LoadInterface();
        }

        private void LoadInterface()
        {
            using var context = new AppDbContext();
            _products = context.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Provider)
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .ToList();

            foreach (var product in _products)
            {
                ProductsList.Children.Add(new Elements.Product(product));
            }
            UserName.Content = MainWindow.User.FIO;
            var manufacturers = context.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturer() { Id = 0, Name = "Все поставщики" });
            Manufacturers.ItemsSource = manufacturers;
        }

        private void LoadInterfaceByRoles()
        {
            if(MainWindow.User.Role.Id == 3)
            {
                OpenAddProductPageBtn.Visibility = Visibility.Visible;
                OpenOrdersPageBtn.Visibility = Visibility.Visible;
            }
            if (MainWindow.User.Role.Id == 2)
            {
                OpenOrdersPageBtn.Visibility = Visibility.Visible;
            }
        }

        private void OpenAuthorizationPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Authorization());
        }

        private void OpenOrdersPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Orders());
        }

        private void OpenAddProductPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new SaveProduct());
        }

        private void SortProducts()
        {
            if (Search == null || Manufacturers == null || OrderBy == null) return;

            var txt = Search.Text.ToLower();
            var q = _products.Where(p => string.IsNullOrEmpty(txt)
            ||p.Name.ToLower().Contains(txt)
            || p.Manufacturer.Name.ToLower().Contains(txt) 
            || p.Provider.Name.ToLower().Contains(txt));

            if (Manufacturers.SelectedIndex > 0)
                q = q.Where(p => p.ManufacturerId == (int)Manufacturers.SelectedValue);

            q = OrderBy.SelectedIndex == 1 ? q.OrderBy(p => p.QuantityStock) : OrderBy.SelectedIndex == 2 ?
                q.OrderByDescending(p => p.QuantityStock) : q;

            ProductsList.Children.Clear();
            foreach (var p in q) ProductsList.Children.Add(new Elements.Product(p));
        }

        private void SortProducts(object sender, TextChangedEventArgs e) =>
            SortProducts();

        private void SortProducts(object sender, SelectionChangedEventArgs e) =>
            SortProducts();
    }
}
