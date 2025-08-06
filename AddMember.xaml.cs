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

//This is my final submission.

namespace FinalProject
{
    public partial class AddMember : UserControl
    {
        Member existingAccount;
        bool editing;
        public AddMember()
        {
            editing = false;
            InitializeComponent();
        }

        public AddMember(Member member)
        {
            editing = true;
            existingAccount = member;
            InitializeComponent();
            ShowEditingScreen();
            FillDetails();
            usernameBox.IsEnabled = false;
        }

        void ShowEditingScreen()
        {
            MemberHeader.Text = "Edit Member Account";
            Submit_Button.Content = "Update Details";
        }

        void FillDetails()
        {
            NameBox.Text = existingAccount.name;
            DOBBox.Text = existingAccount.DOB;
            usernameBox.Text = existingAccount.username;
            passwordBox.Text = existingAccount.password;
        }

        bool usernameAvailable()
        {
            if(editing)
            {
                return true;
            }
            List<Member> members = DatabaseManager.LoadMembers(); //I would replace this with an SQL search query if I had time.
            foreach (Member M in members)
            {
                if (usernameBox.Text == M.username)
                {
                    Submit_Button.Content = "That username is already in use";
                    return false;
                }
            }
            return true;
        }

        void registerAccount()
        {
            Member member = new Member(NameBox.Text, DOBBox.Text, usernameBox.Text, passwordBox.Text);
            DatabaseManager.AddToChanges(member);
            DatabaseManager.CommitAllChanges();
        }

        void EditDetails()
        {
            existingAccount.name = NameBox.Text;
            existingAccount.DOB = DOBBox.Text;
            existingAccount.password = passwordBox.Text;
            DatabaseManager.UpdateMemberDetails(existingAccount);
        }

        bool detailsExist()
        {
            TextBox[] textBoxes = { NameBox, DOBBox, usernameBox, passwordBox };
            foreach(TextBox t in textBoxes)
            {
                if (t.Text == "")
                {
                    Submit_Button.Content = "Member information is missing, fill in all fields";
                    return false;
                }
            }
            return true;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(detailsExist())
            {
                if(usernameAvailable())
                {
                    if(editing)
                    {
                        EditDetails();
                        Submit_Button.Content = "Member information Updated";
                        return;
                    }
                    Submit_Button.Content = "Account Added";
                    registerAccount();
                }
            }
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.AdminInitial();
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

        private void Key_Up(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submit_Click(sender, e);
            }
            TextBox t = (TextBox)sender;
            if (t.Text == "")
            {
                ResetTextBox(t);
            }
        }
    }
}
