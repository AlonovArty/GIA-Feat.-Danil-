using Exam.Context;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace Exam.Pages
{
    public partial class SaveOrder : Page
    {
        Models.Order _order;
        public SaveOrder(Models.Order order = null)
        {
            InitializeComponent();
            _order = order;
            LoadBaseInterface();
            if (order is not null)
                LoadInterface();
        }
        private void LoadBaseInterface()
        {
            using var context = new Context.AppDbContext();
            var pickupPoints = context.PickupPoints.ToList();
            var orderStatuses = context.OrderStatuses.ToList();
            var products = context.Products.ToList();
            var clients = context.Users.ToList();
            PickupPoints.ItemsSource = pickupPoints;
            Statuses.ItemsSource = orderStatuses;
            Clients.ItemsSource = clients;
        }
        private void LoadInterface()
        {
            Statuses.SelectedValue = _order.OrderStatusId;
            PickupPoints.SelectedValue = _order.PickupPointId;
            Clients.SelectedValue = _order.UserId;
            OrderDate.SelectedDate = _order.OrderDate.ToDateTime(TimeOnly.MinValue);
            DeliveryDate.SelectedDate = _order.DeliveryDate.ToDateTime(TimeOnly.MinValue);

            foreach(var orderProduct in _order.OrderProducts)
            {
                OrderProducts.Children.Add(new Elements.OrderProduct(orderProduct));
            }
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            OrderProducts.Children.Add(new Elements.OrderProduct());
        }


        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new AppDbContext();

                List<Models.OrderProduct> orderProducts = new();
                foreach (var orderProductsChildren in OrderProducts.Children)
                    if (orderProductsChildren is Elements.OrderProduct orderProduct)
                        orderProducts.Add(new Models.OrderProduct()
                        {
                            ProductId = (int)orderProduct.Products.SelectedValue,
                            Quantity = int.Parse(orderProduct.Quantity.Text)
                        });

                if (_order is null)
                {
                    Models.Order order = new()
                    {
                        OrderDate = DateOnly.FromDateTime(OrderDate.SelectedDate.Value),
                        DeliveryDate = DateOnly.FromDateTime(DeliveryDate.SelectedDate.Value),
                        Code = new Random().Next(0, 999),
                        OrderStatusId = (int)Statuses.SelectedValue,
                        PickupPointId = (int)PickupPoints.SelectedValue,
                        UserId = (int)Clients.SelectedValue,
                        OrderProducts = orderProducts
                    };
                    context.Orders.Add(order);
                }
                else
                {
                    var order = context.Orders
                        .Include(o => o.OrderProducts)
                        .FirstOrDefault(o => o.Id == _order.Id);
                    if (order is null)
                    {
                        MessageBox.Show("Заказ не найден");
                        return;
                    }

                    order.OrderDate = DateOnly.FromDateTime(OrderDate.SelectedDate.Value);
                    order.DeliveryDate = DateOnly.FromDateTime(DeliveryDate.SelectedDate.Value);
                    order.Code = new Random().Next(0, 999);
                    order.OrderStatusId = (int)Statuses.SelectedValue;
                    order.PickupPointId = (int)PickupPoints.SelectedValue;
                    order.UserId = (int)Clients.SelectedValue;
                    order.OrderProducts.Clear();
                    order.OrderProducts = orderProducts;
                }
                context.SaveChanges();

                MainWindow.Main.MainFrame.Navigate(new Orders());
            }
            catch 
            {
                MessageBox.Show($"Заполните все поля!","Предупреждение",MessageBoxButton.OK);
            }
        }

        private void OpenProductsPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Products());
        }
    }
}
