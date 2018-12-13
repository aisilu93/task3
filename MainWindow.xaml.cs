using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private ICollectionView defaultView;
        public string FilterString
        {
            get { return FilterString; }
            set
            {
                FilterString = value;
                defaultView.Refresh();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            cb = new ContactBase();
            this.defaultView = CollectionViewSource.GetDefaultView(cb.contacts);
            grid.ItemsSource = defaultView;
            CreatePresets();
        }
        public ICollectionView Contacts
        {
            get { return defaultView; }
        }
        public void close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private bool ContactsFilter(object item)
        {
            Contact c = item as Contact;
            return c.m_name.Contains(FilterString);
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
        public void CreatePresets()
        {
            presets.Inlines.Clear();
            Run linkText = new Run("All");
            UriBuilder uri = new UriBuilder("foo");
            Hyperlink hlink = new Hyperlink(linkText);
            hlink.NavigateUri = uri.Uri;
            presets.Inlines.Add("   ");
            presets.Inlines.Add(hlink);
            hlink.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(Hyperlink_RequestNavigate);
            SortedSet<char> s = new SortedSet<char>();
            foreach (Contact item in cb.contacts)
            {
                if (item.m_name.Length > 0) s.Add(item.m_name.ToUpper()[0]);
            }
            foreach (char ch in s)
            {
                linkText = new Run(ch.ToString());
                uri = new UriBuilder("foo");
                hlink = new Hyperlink(linkText);
                hlink.NavigateUri = uri.Uri;
                hlink.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(Hyperlink_RequestNavigate);
                presets.Inlines.Add("   ");
                presets.Inlines.Add(hlink);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Run r = link.Inlines.FirstInline as Run;
            if(r.Text == "All") defaultView.Filter = w => ((Contact)w).m_name.Contains("");
            else defaultView.Filter = w => (((Contact)w).m_name.Contains(r.Text) || ((Contact)w).m_name.Contains(r.Text.ToLower()));
            e.Handled = true;
        }

    }
 }
