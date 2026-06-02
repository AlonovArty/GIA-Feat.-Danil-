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

namespace Exam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        List<Models.Order> _orders;
        public Orders()
        {
            InitializeComponent();
            LoadInterfaceByRoles();
            LoadInterface();
        }
        private void LoadInterface()
        {
            using var context = new Context.AppDbContext();
            _orders = context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Include(o => o.PickupPoint)
                .ToList();
            foreach (var order in _orders)
            {
                OrdersList.Children.Add(new Elements.Order(order));
            }
        }
        private void LoadInterfaceByRoles()
        {
            if (MainWindow.User.Role.Id == 3)
            {
                OpenAddOrderPageBtn.Visibility = Visibility.Visible;
            }
        }   

        private void OpenProductsPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Products());
        }

        private void OpenAddOrderPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new SaveOrder());
        }
    }
}
