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
    /// Interaction logic for Book_Screen.xaml
    /// Only one Book Screen can be open at a time. This prevents books being reserved on parallel open screens with out of date reservation status data.
    /// </summary>
    public partial class Book_Screen : UserControl
    {
        Book book; 
        Member member;
        bool fromCatalog;
             
      /*  public Book_Screen(Book _book, Member _member) //make priavte
        {
            book = _book;
            member = _member;
            InitializeComponent();
            LoadDetails();
            InitializeButton();
        }
*/
        public Book_Screen(Book _book, Member _member, bool _fromCatalog = false) //make priavte
        {
            book = _book;
            member = _member;
            fromCatalog = _fromCatalog;
            InitializeComponent();
            LoadDetails();
            InitializeButton();
        }

        void InitializeButton()
        {
            if (book.reserved == true)
            {
                DisableBtn();
            }
        }

        private void LoadDetails()
        {
            TitleBlock.Text = book.title;
            AuthorBlock.Text = book.authorLastNames + ", " + book.authorFirstNames;
            DescriptionBlock.Text = book.description;
            YearBlock.Text = Convert.ToString(book.year);
            GenreBlock.Text = Book.Genres[Convert.ToInt32(book.genre)] + ", " + Book.Genres[Convert.ToInt32(book.genre2)];
            ReservationBlock.Text = DisplayAvailablity();
            string imagePath = $"{book.imagePath}";
            ImageBox.Source = new BitmapImage(new Uri(@imagePath));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(fromCatalog)
            {
                PageLoader.CatalogRegularAccess();
                return;
            }
            PageLoader.MyAccount();

        }

        private void CommitReservation()
        {
            DatabaseManager.AddToChanges(new Reserved(member, book));
            DatabaseManager.UpdateCatalogReservedandAvailableStatus(book);
            DatabaseManager.CommitAllChanges();
        }

         string ExpectedAvailability()
        {
            DateTime dateBorrowed;
            Borrowed b = DatabaseManager.SearchBorrowedById(DatabaseManager.TABLE.BORROWED, DatabaseManager.COLUMN.BOOKID, book.id).Last();

            if (!DateTime.TryParse(b.dt, out dateBorrowed))
            {
                return "Due Date Unknown";
            }

            return $"Not Available (Expected Return:{dateBorrowed.AddDays(14).ToString()} ) ";
        }

        string DisplayAvailablity()
        {
           if (book.available)
           {
                return "This item is Available";
           }
           else if(book.reserved)
           {
                return "This item is Reserved";
           }
           else
           {
                return ExpectedAvailability();
           }
        }

        void DisableBtn()
        {
            Reserved r = DatabaseManager.SearchReservedById(DatabaseManager.TABLE.RESERVED, DatabaseManager.COLUMN.BOOKID, book.id).Last();
            if (r.memberId == member.id)
            {
                ReservationToggle.Content = "You have this Item Reserved";
            }
            else
            {
                ReservationToggle.Content = "This Item is already Reserved";
            }
            ReservationToggle.IsEnabled = false;
            ReservationToggle.Background = new SolidColorBrush(Colors.Gray);
        }

        void Reserve_Click(object sender, RoutedEventArgs e)
        {
             CommitReservation();
             DisplayAvailablity();
             DisableBtn();
        }
    } 
}
