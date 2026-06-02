using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam
{
    public partial class MainWindow : Window
    {
        //
        public static Models.User User;
        public static MainWindow Main;
        public MainWindow()
        {
            InitializeComponent();
            Main = this;
            MainFrame.Navigate(new Pages.Authorization());
        }

        public static string GetImagePathByFileName(string fileName) =>
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);

        public static BitmapImage LoadImg(string name)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = new Uri(GetImagePathByFileName(string.IsNullOrEmpty(name) ? "picture.png" : name));
            bmp.EndInit();
            return bmp;
        }
    }
}