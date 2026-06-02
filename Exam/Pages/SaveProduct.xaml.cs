using Exam.Elements;
using Exam.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
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

namespace Exam.Pages
{
    public partial class SaveProduct : Page
    {
        Models.Product _product;
        public SaveProduct(Models.Product product = null)
        {
            InitializeComponent();
            _product = product;
            LoadBaseInterface();
            if (product is not null)
                LoadInterface();
        }

        private void LoadInterface()
        {
            if (!string.IsNullOrEmpty(_product.Image))
            {
                Image.Source = MainWindow.LoadImg(_product.Image);
                Image.Tag = _product.Image;
            }
            Article.Text = _product.Article;
            Name.Text = _product.Name;
            Description.Text = _product.Description;
            Priсe.Text = _product.Price.ToString();
            Quantity.Text = _product.QuantityStock.ToString();
            Discount.Text = _product.Discount.ToString();
            Units.SelectedValue = _product.UnitId;
            Categories.SelectedValue = _product.CategoryId;
            Manufacturers.SelectedValue = _product.ManufacturerId;
            Providers.SelectedValue = _product.ProviderId;
        }
        private void LoadBaseInterface()
        {
            using var context = new Context.AppDbContext();
            var units = context.Units.ToList();
            var categories = context.Categories.ToList();
            var manufacturers = context.Manufacturers.ToList();
            var providers = context.Providers.ToList();
            Units.ItemsSource = units;
            Categories.ItemsSource = categories;
            Manufacturers.ItemsSource = manufacturers;
            Providers.ItemsSource = providers;
        }
       

        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new Context.AppDbContext();

                if (_product is null)
                {
                    Models.Product product = new()
                    {
                        Article = Article.Text,
                        Name = Name.Text,
                        Description = Description.Text,
                        Price = int.Parse(Priсe.Text),
                        QuantityStock = int.Parse(Quantity.Text),
                        Discount = int.Parse(Discount.Text),
                        UnitId = (int)Units.SelectedValue,
                        CategoryId = (int)Categories.SelectedValue,
                        ManufacturerId = (int)Manufacturers.SelectedValue,
                        ProviderId = (int)Providers.SelectedValue,
                        Image = Image.Tag as string ?? string.Empty
                    };
                    context.Products.Add(product);
                }
                else
                {
                    var oldProduct = context.Products.Find(_product.Id);
                    if (oldProduct is null)
                    {
                        MessageBox.Show("Товар не найден");
                        return;
                    }
                    oldProduct.Article = Article.Text;
                    oldProduct.Name = Name.Text;
                    oldProduct.Description = Description.Text;
                    oldProduct.Price = int.Parse(Priсe.Text);
                    oldProduct.QuantityStock = int.Parse(Quantity.Text);
                    oldProduct.Discount = int.Parse(Discount.Text);
                    oldProduct.UnitId = (int)Units.SelectedValue;
                    oldProduct.CategoryId = (int)Categories.SelectedValue;
                    oldProduct.ManufacturerId = (int)Manufacturers.SelectedValue;
                    oldProduct.ProviderId = (int)Providers.SelectedValue;
                    oldProduct.Image = Image.Tag.ToString() ?? string.Empty;
                }
                context.SaveChanges();
                MainWindow.Main.MainFrame.Navigate(new Products());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Заполните все поля: {ex.Message}");
            }
        }
        private void SelectImage(object sender, RoutedEventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == false)
                    return;

                var imageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(ofd.FileName);
                var newImagePath = MainWindow.GetImagePathByFileName(imageName);

                File.Copy(ofd.FileName, newImagePath, true);
                if (_product is not null && !string.IsNullOrEmpty(_product.Image))
                {
                    var oldPath = MainWindow.GetImagePathByFileName(_product.Image);
                    if (File.Exists(oldPath))
                        try { File.Delete(oldPath); } catch { }
                }
                Image.Source = MainWindow.LoadImg(imageName);
                Image.Tag = imageName;
            }
            catch
            (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenProductsPage(object sender, RoutedEventArgs e) =>
            MainWindow.Main.MainFrame.Navigate(new Products());
        
    }
}
