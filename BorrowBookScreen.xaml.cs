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
    /// Interaction logic for BorrowBookScreen.xaml
    /// </summary>
    public partial class BorrowBookScreen : UserControl
    {
        int itemId;
        int memberId;
        Member member;
        Book book;
       
        public BorrowBookScreen()
        {
            InitializeComponent();
        }

        public BorrowBookScreen(Book _book)
        {
            InitializeComponent();
            book = _book;
            BookIdBox.Text = book.id.ToString();
        }

        void Go_Back_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.CatalogAdminBorrowingAccess();
        }
        private void BorrowItem()
        {
            book = DatabaseManager.SearchCatalogById(itemId).Last();
            member = DatabaseManager.SearchMemberById(memberId).Last();
            DatabaseManager.AddToChanges(new Borrowed(member, book));
            DatabaseManager.CommitAllChanges();
            MemberIdBox.Text = $"Item: {itemId} ( {book.title} ) borrowed by: {member.name} succesfully";
            book.available = false;
            DatabaseManager.UpdateCatalogReservedandAvailableStatus(book);
        }

        bool InputsConvertToInt()
        {
            if (DatabaseManager.ValidNumberString(BookIdBox.Text) == false)
            {
                BookIdBox.Text = "Enter an Integer Value";
                return false;
            }
            if (DatabaseManager.ValidNumberString(MemberIdBox.Text) == false)
            {
                MemberIdBox.Text = "Enter an Integer Value";
                return false;
            }
            return true;
        }

        bool IdsExist()
        {
            if (DatabaseManager.CheckBookIdExists(itemId) == false)
            {
                BookIdBox.Text = "That id is not in use";
                return false;
            }
            if (DatabaseManager.CheckMemberIdExists(memberId) == false)
            {
                MemberIdBox.Text = "That id is not in use";
                return false;
            }
            return true;
        }

        void AcceptInputValues()
        {
            itemId = Convert.ToInt32(BookIdBox.Text);
            memberId = Convert.ToInt32(MemberIdBox.Text);
        }

        void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (InputsConvertToInt())
            {
                AcceptInputValues();
                if (IdsExist())
                {
                    BorrowItem();
                }
            }
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
                Submit_Button_Click(sender, e);
            }
            TextBox t = (TextBox)sender;
            if (t.Text == "")
            {
                ResetTextBox(t);
            }
        }
    }
}
