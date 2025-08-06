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

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for UIElementSketchpad.xaml
    /// </summary>
    public partial class LibrarySystemMessages : UserControl
    {
        public LibrarySystemMessages()
        {
            InitializeComponent();
            DatabaseManager.CheckForOverdueItems();
            LoadAllMessages();
        }

        void OverdueMessages()
        {
            foreach (OverdueMessage o in DatabaseManager.LoadOverDueMessages())
            {
                MessagePanel.Children.Add(new MessageCard(o));
            }
        }

        void DueDateMessages()
        {
            foreach (DueDateMessage d in DatabaseManager.LoadDueDateMessages())
            {
                MessagePanel.Children.Add(new MessageCard(d));
            }
        }

        void ReturnedMessages()
        {
            foreach (ReturnedMessage r in DatabaseManager.LoadReturnedMessages())
            {
                MessagePanel.Children.Add(new MessageCard(r));
            }
        }

        void LoadAllMessages()
        {
            OverdueMessages();
            DueDateMessages();
            ReturnedMessages();         
        }

        void Go_Back_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.AdminInitial();
        }
    }
}
