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
using System.Windows.Shapes;

namespace task3
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        public Contact contact, contact_copy;
        bool new_contact;
        public UserInfo()
        {
            Initialize();
            new_contact = true;
            contact = new Contact();
            base.DataContext = contact;
        }
        public UserInfo(Contact c)
        {
            Initialize();
            new_contact = false;
            contact_copy = (Contact)c.Clone();
            base.DataContext=c;
            contact = c;
        }
        private void Initialize()
        {
            InitializeComponent();
            CommandBinding commandBinding = new CommandBinding();
            commandBinding.Command = ApplicationCommands.Close;
            commandBinding.Executed += CloseCmd_Executed;
            uinf_close.CommandBindings.Add(commandBinding);

            CommandBinding commandBinding1 = new CommandBinding();
            commandBinding1.Command = ApplicationCommands.Save;
            commandBinding1.Executed += SaveCmd_Executed;
            uinf_save.CommandBindings.Add(commandBinding1);

            CommandBinding commandBindingDelete = new CommandBinding();
            commandBindingDelete.Command = ApplicationCommands.Delete;
            commandBindingDelete.Executed += DeleteCmd_Executed;
            uinf_delete.CommandBindings.Add(commandBindingDelete);
            
        }
        private void CloseCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(!new_contact) contact = contact_copy;
            this.Close();
        }
        private void SaveCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = this.Owner as MainWindow;
            if (new_contact) mw.cb.Add(contact);
            this.Close();
        }
        private void DeleteCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = this.Owner as MainWindow;
            mw.cb.Delete(contact);
            this.Close();
        }
    }

}
