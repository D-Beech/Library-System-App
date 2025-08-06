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
    /// Catalog ItemCards are generated dynamically from the database (CatalogItems Table).
    /// There is a button the size of the card, it's destitnation differs depending on the user (member or admin) if admin the access method from the admin menu.
    /// </summary>
    public partial class ItemCard : UserControl
    {
        Book book;
        static public bool borrowing = false;

        delegate void ClickHandler(Book book);
        event ClickHandler ButtonClickEvent;

        public ItemCard()
        {
            InitializeComponent();
        }

        public ItemCard(Book _book)
        {
            InitializeComponent();
            book = _book;
            ItemAuthor.Text = book.authorFirstNames + ", " + book.authorLastNames;
            ItemTitle.Text = book.title;
            string imagePath = $"{book.imagePath}";
            ImageBox.Source = new BitmapImage(new Uri(@imagePath));
            ItemAvailable.Text = ShowAvailability();
            ItemAvailable.Foreground = AvailabilityColor();
        }

        string ShowAvailability()
        {
            return  (book.available == true) ? "Available" : "Not Available";
        }

        SolidColorBrush AvailabilityColor()
        {
            return (book.available == true) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.DarkRed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(PageLoader.isAdmin == true )
            {
                if(borrowing)
                {
                    if (book.available == false)
                    {
                        return; //Admin account cannot interact with loaned item, this prevents unavailable books being loaned.
                    }
                    ButtonClickEvent += (book) => PageLoader.BorrowBookScreen(book);
                }
                else //Admin is editing Book Details.
                {
                    ButtonClickEvent += (book) => PageLoader.updateBook(book);
                }  
            }
            else //Member account interaction.
            {
                ButtonClickEvent += (book) => PageLoader.BookScreenFromCatalog(book);
            }
            ButtonClickEvent?.Invoke(book);
        }

        private void Make_Border_Visible(object sender, MouseEventArgs e)
        {
            CardBorder.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void Make_Border_Transparent(object sender, MouseEventArgs e)
        {
            CardBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }
    }
}
