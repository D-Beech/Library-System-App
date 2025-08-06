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
    /// Interaction logic for AllUserAccounts.xaml
    /// </summary>
    public partial class AllUserAccounts : UserControl
    {
        public AllUserAccounts()
        {
            InitializeComponent();
            LoadMembers();
        }

       void LoadMembers()
        {
            foreach(Member m in DatabaseManager.LoadMembers())
            { 
                    MessagePanel.Children.Add(new MessageCard(m));
            }
        }
        void Go_Back_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.AdminInitial();
        }
    }
}

