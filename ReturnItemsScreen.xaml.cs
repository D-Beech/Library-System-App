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
    /// Interaction logic for WithdrawnItemScreen.xaml
    /// </summary>
    public partial class ReturnItemsScreen : UserControl
    {
        int itemId;
        public ReturnItemsScreen()
        {
            InitializeComponent();
        }

        void Go_Back_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.AdminInitial();
        }
        private void ReturnItem()
        {
            Borrowed b = DatabaseManager.makeBorrowedItemFromDB(itemId);
            DatabaseManager.AddToChanges(new Returned(b));
            DatabaseManager.CommitAllChanges();
        }

        bool InputCanConcertToInt()
        {
            return DatabaseManager.ValidNumberString(BorrowedIdBox.Text);
        }

        void AcceptInput()
        {
            ReturnItem();
            BorrowedIdBox.Text = $"Item Returned";
        }
         
        bool IdExists()
        {
            if(DatabaseManager.CheckBorrowedIdExists(itemId))
            {
                return true;
            }
            BorrowedIdBox.Text = "That id is not in use";
            return false;
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if(InputCanConcertToInt())
            {
                itemId = Convert.ToInt32(BorrowedIdBox.Text);
                if (IdExists())
                {
                    AcceptInput();
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
