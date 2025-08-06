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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.LogOut();
        }

        private void Add_Book_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.addCatalogItemScreen();
        }
        
        private void Edit_Book_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.CatalogRegularAccess(); 
        }

        private void Messages_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.AllLibrarySystemMessages(); 
        }

        private void New_Member_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.AddMember();
        }

        private void Return_Book_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.ReturnItemsScreen();
        }

        private void Borrow_Book_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.CatalogAdminBorrowingAccess();
        }

        void Edit_Member_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.AllUserAccounts();
        }
    }
}
