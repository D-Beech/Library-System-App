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
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : UserControl
    {
        List<Book> catalog;
        public Catalog()
        {
            InitializeComponent();
            FillBooks();
        }

        public Catalog(Member _member)
        {
            InitializeComponent();
        }

        void FillBooks()
        {
            catalog = DatabaseManager.LoadBooks();
            foreach (Book b in catalog)
            {
                CatalogStack.Children.Add(b.makeCatalogCard());
            }
        }

        void UpdateSearchBoxText()
        {
            if (searchBar.Text != searchBar.Tag.ToString())
            {
                SearchBlock.Text = "Fliter Catalogue\n\nShowing\n\n+ " + searchBar.Text;
            }
        }

        private void Search(string searchTerm) //I wrote this agorithm before I understood SQL WHERE and LIKE Queries, I've run out of time to replace it but that would be a more efficient search method.
        {
           
            CatalogStack.Children.Clear();
            foreach (Book b in catalog)
            {
                if (b.title == searchTerm) //The method I've implemented reads every string so is very slow but it works.
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
                if (b.authorLastNames == searchTerm)
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
                if (b.authorFirstNames == searchTerm)
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
                if (Convert.ToString(b.year) == searchTerm)
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
                if (Book.Genres[Convert.ToInt32(b.genre)] == searchTerm)
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
                if (Book.Genres[Convert.ToInt32(b.genre2)] == searchTerm)
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
                if (Book.Medias[Convert.ToInt32(b.media)] == searchTerm)
                { 
                    CatalogStack.Children.Add(b.makeCatalogCard());
                    continue;
                }
            }
        }

        private void SignOut_Button_Click(object sender, RoutedEventArgs e)
        {
           PageLoader.LogOut();
        }

        private void Search_Entry(object sender, RoutedEventArgs e)
        {
            if (searchBar.Text == "Search Catalog")
            {
                searchBar.Text = "";
                searchBar.Foreground = FilledInColor();
            }
           
        }

        private void Search_Event(object sender, RoutedEventArgs e)
        {
            Search(searchBar.Text); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(PageLoader.isAdmin)
            {
                PageLoader.AdminInitial();
            }
            else
            {
                PageLoader.MemberMenu();
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

        void ResetTextBox(TextBox t)
        {
            t.Text = t.Tag.ToString();
            t.Foreground = DefaultColor();
        }

        private void Key_Up(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateSearchBoxText();
                Search(searchBar.Text);
            }
            TextBox t = (TextBox)sender;
            if (t.Text == "")
            {
                ResetTextBox(t);
            }
        }
    }

}
