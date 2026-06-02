using Exam.Context;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Exam.Elements
{
    public partial class Product : UserControl
    {
        Models.Product _product;
        public Product(Models.Product product)
        {
            InitializeComponent();
            _product = product;
            LoadInterfaceByRoles();
            if (product is not null)
            {
                SetData();
                SetPrice();
            }
        }

        private void SetData()
        {
            Image.Source = MainWindow.LoadImg(_product.Image);
            CategoryAndName.Content = $"{_product.Category?.Name} | {_product.Name}";
            Description.Content = $"Описание товара: {_product.Description}";
            Manufacturer.Content = $"Производитель: {_product.Manufacturer?.Name}";
            Provider.Content = $"Поставщик: {_product.Provider?.Name}";
            Price.Text = $"Цена: {_product.Price}";
            Unit.Content = $"Единица измерения: {_product.Unit?.Name}";
            Quantity.Content = $"Количество на складе: {_product.QuantityStock}";
            Discount.Content = $"Скидка: {_product.Discount}%";
            BaseGrid.Background = _product.QuantityStock <= 0 ? Brushes.LightBlue :
                                  _product.Discount > 15 ? (Brush)new BrushConverter().ConvertFromString("#2E8B57")! :
                                  Brushes.Transparent;
        }

        private void LoadInterfaceByRoles()
        {
            if (MainWindow.User.Role.Id == 3)
            {
                EditBtn.Visibility = Visibility.Visible;
                DeleteBtn.Visibility = Visibility.Visible;
            }
        }

        private void SetPrice()
        {
            if (_product.Discount <= 0)
                return;
            var discountPrice = _product.Price - (_product.Price / 100 * _product.Discount);

            var oldPrice = new Run(_product.Price.ToString())
            {
                TextDecorations = TextDecorations.Strikethrough,
                Foreground = Brushes.Red,
            };
            Price.Text = "";
            Price.Inlines.Add("Цена: ");
            Price.Inlines.Add(oldPrice);
            Price.Inlines.Add(" " + discountPrice.ToString());
        }

        private void Edit(object sender, RoutedEventArgs e) =>
            MainWindow.Main.MainFrame.Navigate(new Pages.SaveProduct(_product));

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить товар?", "Предупреждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            using var context = new AppDbContext();
            context.Products.Where(p => p.Id == _product.Id).ExecuteDelete();
            MainWindow.Main.MainFrame.Navigate(new Pages.Products());
        }
    }
}
