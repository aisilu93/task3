using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Linq;

namespace task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ContactBase cb;
        public ICollectionView Contacts { get; set; }

        public string FilterString
        {
            get { return FilterString; }
            set
            {
                FilterString = value;
                Contacts.Refresh();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            cb = new ContactBase();
            prepare_view();
            CreatePresets();
            // Превращаем событие изменения текста в последовательность строк
            var textchanges = Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                h => searchstring.TextChanged += h,
                h => searchstring.TextChanged -= h
                ).Select(x => ((TextBox)x.Sender).Text);

            var processedTextChanges =
                textchanges
                    // Стартуем поиск неактивности 300 мс
                    // За счет Throttle обработка попадает в фоновый поток
                    .Throttle(TimeSpan.FromMilliseconds(300))
                    // Отбрасываем повторяющийся текст (например опечатались, но быстро стерли опечатку)
                    .DistinctUntilChanged();

            processedTextChanges
                // применяем функцию поиска x => Search(x)
                // Превращая последовательсность строк в последовательность 
                // последовательностей списков результатов
                .Select(Search)
                // При получении следующей строки отписываемся от предыдущего и 
                // подписываемся на новый
                .Switch()
                .ObserveOn(System.Reactive.Concurrency.DispatcherScheduler.Current)
                .Subscribe(OnSearchResult);

            //var results = from searchTerm in processedTextChanges
            //              from result in Search(searchTerm)..TakeUntil(textchanges)
            //              select result;
            //results.ObserveOn(DispatcherScheduler.Current).Subscribe(OnSearchResult);
        }
        public void prepare_view()
        {
            this.Contacts = CollectionViewSource.GetDefaultView(cb.contacts);
            grid.ItemsSource = Contacts;
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
            if(r.Text == "All") Contacts.Filter = w => ((Contact)w).m_name.Contains("");
            else Contacts.Filter = w => (((Contact)w).m_name.Contains(r.Text) || ((Contact)w).m_name.Contains(r.Text.ToLower()));
            e.Handled = true;
        }
        private void OnSearchResult(List<Contact> list)
        {
            grid.ItemsSource = list;
        }

        public IObservable<List<Contact>> Search(string filter)
        {
            //if (filter.Length <= 3) 
            //    Thread.Sleep(5000);
            var filteredList = cb.contacts.Where(x =>   x.m_name.ToLower().Contains(filter.ToLower()) || 
                                                        x.m_workphone.ToLower().Contains(filter.ToLower()) ||
                                                        x.m_homephone.ToLower().Contains(filter.ToLower()) ||
                                                        x.m_skype.ToLower().Contains(filter.ToLower()) ||
                                                        x.m_birthday.ToLower().Contains(filter.ToLower()) ||
                                                        x.m_comment.ToLower().Contains(filter.ToLower())
                                                        ).ToList();
            return Observable.Return(filteredList);
        }
    }
 }
