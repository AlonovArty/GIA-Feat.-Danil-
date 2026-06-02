using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Exam.Elements
{
    public partial class Order : UserControl
    {
        Models.Order _order;
        public Order(Models.Order order)
        {
            InitializeComponent();
            _order = order;
            LoadInterfaceByRoles();
            LoadInterface();
        }

        private void LoadInterface()
        {
            Article.Content = $"Артикул: {_order.Id}";
            Status.Content = $"Статус заказа: {_order.OrderStatus?.Name}";
            PickupPoint.Content = $"Адрес пункта выдачи: {_order.PickupPoint?.Address}";
            OrderDate.Content = $"Дата заказа: {_order.OrderDate}";
            DeliveryDate.Content = $"Дата доставки: {_order.DeliveryDate}";
        }

        private void LoadInterfaceByRoles()
        {
            if (MainWindow.User.Role.Id == 3)
            {
                EditBtn.Visibility = Visibility.Visible;
                DeleteBtn.Visibility = Visibility.Visible;
            }
        }

        private void Edit(object sender, RoutedEventArgs e) =>
            MainWindow.Main.MainFrame.Navigate(new Pages.SaveOrder(_order));

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить заказ?", "Предупреждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            using var context = new Context.AppDbContext();
            context.Orders.Where(o => o.Id == _order.Id).ExecuteDelete();
            MainWindow.Main.MainFrame.Navigate(new Pages.Orders());
        }
    }
}
