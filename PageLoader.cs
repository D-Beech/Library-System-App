using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//The purpose of this class is to minimize dependencies between other classes when opening/closing windows.

namespace FinalProject
{
     public class PageLoader
    {
        public static Member loggedIn;
        public static bool isAdmin = false;
        public static void updateBook(Book book)
        {
            MainWindow.GetInstance().Content = new UpdateCatalogItem(book);
        }

        public static void addCatalogItemScreen()
        {
            MainWindow.GetInstance().Content = new AddCatalogItemScreen();
        }
        
        public static void AddMember()
        {
            MainWindow.GetInstance().Content = new AddMember();
        }

        public static void AdminInitial()
        {
            isAdmin = true;
            MainWindow.GetInstance().Content = new Admin();
        }

        public static void BookScreen(Book book)
        {
            MainWindow.GetInstance().Content = new Book_Screen(book, loggedIn);
        }

        public static void BookScreenFromCatalog(Book book)
        {
            MainWindow.GetInstance().Content = new Book_Screen(book, loggedIn, true);
        }

        public static void BorrowBookScreen()
        {
            MainWindow.GetInstance().Content = new BorrowBookScreen();
        }

        public static void BorrowBookScreen(Book book)
        {
            MainWindow.GetInstance().Content = new BorrowBookScreen(book);
        }

        public static void CatalogRegularAccess()
        {
            ItemCard.borrowing = false;
            MainWindow.GetInstance().Content = new Catalog();
        }

        public static void CatalogAdminBorrowingAccess()
        {
            ItemCard.borrowing = true;
            MainWindow.GetInstance().Content = new Catalog();
        }

        public static void LogOut()
        {
            loggedIn = null;
            isAdmin = false;
            LogInScreen();
        }
        public static void LogInScreen()
        {
            MainWindow.GetInstance().Content = new LogIn();
        }

        public static void LoadAccount(Member M)
        {
            loggedIn = M;
            MemberMenu();
        }

        public static void MemberMenu()
        {
            MainWindow.GetInstance().Content = new MemberMenu(loggedIn);
        }

        public static void MyAccount()
        {
            MainWindow.GetInstance().Content = new MyAccount(loggedIn);
        }

        public static void ReturnItemsScreen()
        {
            MainWindow.GetInstance().Content = new ReturnItemsScreen();
        }

        public static void AllLibrarySystemMessages()
        {
            MainWindow.GetInstance().Content = new LibrarySystemMessages();
        }

        public static void AllUserAccounts()
        {
            MainWindow.GetInstance().Content = new AllUserAccounts();
        }

        public static void UpdateMemberDetails(Member m)
        {
            MainWindow.GetInstance().Content = new AddMember(m);
        }
    }
}
