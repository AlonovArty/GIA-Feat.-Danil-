using Exam.Context;
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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private async void OpenProductsPage(object sender, RoutedEventArgs e)
        {
            using var context = new AppDbContext();
            var existedUser = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == Login.Text && u.Password == Password.Text);
            if (existedUser is null)
            {
                MessageBox.Show("Неверный логин или пароль!");
                return;
            }
            MainWindow.User = existedUser;
            MainWindow.Main.MainFrame.Navigate(new Products());
        }
    }
}
