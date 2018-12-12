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

namespace task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ContactBase cb;
        public MainWindow()
        {
            InitializeComponent();
            cb = new ContactBase();
            grid.ItemsSource = cb;

        }
        public void close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void new_user(object sender, RoutedEventArgs e)
        {
            UserInfo uinf = new UserInfo();
            uinf.Owner = this;
            uinf.Show();
        }
        public void grid_click(object sender, MouseButtonEventArgs e)
        {
            Contact c = grid.SelectedItem as Contact;
            UserInfo uinf = new UserInfo(c);
            uinf.Owner = this;
            uinf.Show();
        }
    }
}
