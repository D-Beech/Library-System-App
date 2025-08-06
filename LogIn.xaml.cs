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

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : UserControl
    {
        string adminUsername = "admin";
        string adminPassword = "admin";
        public LogIn()
        {
            InitializeComponent();
        }
        private bool CheckValidCredentials(Member account)
        {
            if (account.username == usernameBox.Text)
            {
                if (account.password == passwordBox.Text)
                {
                    return true;
                }
            }
            return false;
        }
        void adminLogIn()
        {
            if (usernameBox.Text == adminUsername && passwordBox.Text == adminPassword)
            {
                PageLoader.AdminInitial();
            }
        }

        void MemberLogin()
        {
            List<Member> members = DatabaseManager.LoadMembers();
            foreach (Member M in members)
            {
                if (CheckValidCredentials(M))
                {
                    PageLoader.LoadAccount(M);
                }
            }
        }

        void AttemptLogin()
        {
            adminLogIn();
            MemberLogin();
        }

        SolidColorBrush FilledInColor()
        {
            return new SolidColorBrush(Colors.Black);
        }

        SolidColorBrush DefaultColor()
        {
            return new SolidColorBrush(Colors.LightGray);
        }

        private void Text_Box_Entry(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (t.Tag.ToString() == t.Text)
            {
                t.Text = "";
                t.Foreground = FilledInColor();
            }
        }
        void ResetTextBox(TextBox t)
        {
            t.Text = t.Tag.ToString();
            t.Foreground = DefaultColor();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void Key_Up(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptLogin();
            }
            TextBox t = (TextBox)sender;
            if (t.Text == "")
            {
                ResetTextBox(t);
            }
        }
    }
}