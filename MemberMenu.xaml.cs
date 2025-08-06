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
    /// Interaction logic for MemberMenu.xaml
    /// </summary>
    public partial class MemberMenu : UserControl
    {
        public MemberMenu(Member member)
        {
            InitializeComponent();
            PersonalizeMemberMenu(member);
        }
        public void PersonalizeMemberMenu(Member member)
        {
            NameBox.Text = "Signed In " + member.name;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.CatalogRegularAccess();
        }
        private void Sign_Out_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.LogOut();
        }
        private void My_Account_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.MyAccount();
        }

    }
}
