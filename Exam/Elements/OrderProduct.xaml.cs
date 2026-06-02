using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Exam.Elements
{
    public partial class OrderProduct : UserControl
    {
        public Models.OrderProduct _orderProduct;
        public OrderProduct(Models.OrderProduct orderProduct = null)
        {
            InitializeComponent();
            _orderProduct = orderProduct;
            using var context = new Context.AppDbContext();
            Products.ItemsSource = context.Products.ToList();
            if (orderProduct is not null)
            {
                Products.SelectedValue = orderProduct.ProductId;
                Quantity.Text = orderProduct.Quantity.ToString();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Panel panel)
                panel.Children.Remove(this);
        }
    }
}
