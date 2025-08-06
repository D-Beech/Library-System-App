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
    /// Interaction logic for MyAccount.xaml
    /// </summary>
    public partial class MyAccount : UserControl
    {
        List<Reserved> reservedItems;
        List<Book> catalog;
        Member member;
        bool showMessages = false;

        public MyAccount(Member _member)
        {
            member = _member;
            InitializeComponent();
            PersonalizeMyAccount(member);
            InitializeLists();
            initializeHeaders();
            FillInfo();
        }

        void InitializeLists() //These lists are used to perform id searches. Now that I understand how to implement SQL queries I would prefer to use that method but I have run out of time.
        {
            reservedItems = DatabaseManager.LoadReservedItems();
            reservedItems = findAccociated();
            catalog = DatabaseManager.LoadBooks();
        }

        void initializeHeaders() 
        {
            TotalReservedHeader.Text = "Total Items on Reserved: " + DatabaseManager.SearchReservedById(DatabaseManager.TABLE.RESERVED, DatabaseManager.COLUMN.MEMBERID, member.id).Count.ToString();
            TotalBorrowingHeader.Text = "Total Items Currently Borrowing: " + DatabaseManager.SearchBorrowedById(DatabaseManager.TABLE.BORROWED, DatabaseManager.COLUMN.MEMBERID, member.id).Count.ToString();
            TotalOverdueHeader.Text = "Total Overdue Items: " + DatabaseManager.SearchOverdueById(DatabaseManager.TABLE.OVERDUE, DatabaseManager.COLUMN.MEMBERID, member.id).Count.ToString();
            TotalReturnedHeader.Text = "Total Returned Items: " + DatabaseManager.SearchReturnedById(DatabaseManager.TABLE.RETURNED, DatabaseManager.COLUMN.MEMBERID, member.id).Count.ToString();
        }

        public void PersonalizeMyAccount(Member member)
        {
            NameHeader.Text = PageLoader.loggedIn.name;
            AgeHeader.Text = member.DOB;
        }

        void FillInfo()
        {
            ActiveItemPanel.Children.Clear();

            ActiveItemPanel.Children.Add(TotalBorrowingHeader);
            FillBorrowedItems();

            ActiveItemPanel.Children.Add(TotalReservedHeader);
            FillReservedItems();

            ActiveItemPanel.Children.Add(TotalOverdueHeader);
            FillOverdueItems();

            ActiveItemPanel.Children.Add(TotalReturnedHeader);
            FillReturnedItems();
        }

        void FillOverDue()  //These functions are id searches.They have a high perfomace and memory cost comapred to an sql serach, if I had more time I would replace them with those.
        {
            foreach (OverdueMessage o in DatabaseManager.LoadOverDueMessages())
            {
                if (o.memberId == member.id)
                {
                    ActiveItemPanel.Children.Add(new MessageCard(o));
                }
            }
        }

        void FillDueDate()
        {
            foreach (DueDateMessage d in DatabaseManager.LoadDueDateMessages())
            {
                if (d.memberId == member.id)
                {
                    ActiveItemPanel.Children.Add(new MessageCard(d));
                }
            }
        }

        void FillReturned()
        {
            foreach (ReturnedMessage r in DatabaseManager.LoadReturnedMessages())
            {
                if (r.memberId == member.id)
                {
                    ActiveItemPanel.Children.Add(new MessageCard(r));
                }
            }
        }
        void FillMessages() 
        {
            ActiveItemPanel.Children.Clear();
            FillOverDue();
            FillDueDate();
            FillReturned();
        }

        List<Reserved> findAccociated() //Thins the list down to only reservations accociated with the user account.
        {
            List<Reserved> relavent = new List<Reserved>(); 
            foreach (Reserved r in reservedItems)
            {
                if (r.memberId == member.id)
                {
                    relavent.Add(r);
                }
            }
            return relavent;
        }

        void FillReservedItems()
        {
            foreach (Reserved r in reservedItems)
            {
                if(true)
                {
                   ActiveItemPanel.Children.Add(new BookTitleClickable(catalog.ElementAt(r.bookId - 1)));
                }
            }
        }

        void FillBorrowedItems()
        {
            foreach (Borrowed b in DatabaseManager.SearchBorrowedById(DatabaseManager.TABLE.BORROWED, DatabaseManager.COLUMN.MEMBERID, member.id))
            {
                if (true)
                {
                    ActiveItemPanel.Children.Add(new BookTitleClickable(catalog.ElementAt(b.bookId - 1)));
                }
            }
        }

        void FillReturnedItems()
        {
            foreach (Returned r in DatabaseManager.SearchReturnedById(DatabaseManager.TABLE.RETURNED, DatabaseManager.COLUMN.MEMBERID, member.id))
            {
                if (true)
                {
                    ActiveItemPanel.Children.Add(new BookTitleClickable(catalog.ElementAt(r.bookId - 1)));
                }
            }
        }

        void FillOverdueItems()
        {
            foreach (Overdue o in DatabaseManager.SearchOverdueById(DatabaseManager.TABLE.OVERDUE, DatabaseManager.COLUMN.MEMBERID, member.id))
            {
                if (true)
                {
                    ActiveItemPanel.Children.Add(new BookTitleClickable(catalog.ElementAt(o.bookId - 1)));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.MemberMenu();
        }

        void ShowingMessages()
        {
            FillMessages();
            SwitchBtn.Content = "Show Your Account Info";
            showMessages = true;
        }

        void ShowingInfo()
        {
            FillInfo();
            SwitchBtn.Content = "Show Your Messages";
            showMessages = false;
        }

        void Switch_Click(object sender, RoutedEventArgs e)
        {
            if(showMessages)
            {
                ShowingInfo();
            }
            else
            {
                ShowingMessages();
            }
        }
    }
}
