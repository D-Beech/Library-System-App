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
    /// Interaction logic for BookTitleClickable.xaml
    /// </summary>
    public partial class BookTitleClickable : UserControl
    {
        Book book; 
        public BookTitleClickable(Book _book)
        {
            InitializeComponent();
            book = _book;
            NameBox.Text = book.title;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageLoader.BookScreen(book);
        }
    }
}
