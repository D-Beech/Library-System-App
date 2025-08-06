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
    /// Interaction logic for AddCatalogItemScreen.xaml
    /// </summary>
    public partial class AddCatalogItemScreen : UserControl
    {
        public AddCatalogItemScreen()
        {
            InitializeComponent();
            InitializeListBoxes();
            nameboxes();
        }

        string DefaultImagePath()
        {
            return "D:\\Users\\270298599\\source\\repos\\DatuProgress\\FinalProject\\FinalProject\\Icons\\Book_Icon.png";
        }

        Book makeBookFromForm() //Trying to keep this readable.
        {
            if(ImageBox.Text == "" || ImageBox.Text == ImageBox.Tag.ToString())
            {
                ImageBox.Text = DefaultImagePath();
            }
            Book book = 
            new Book(TitleBox.Text, AuthorFirstNameBox.Text,
            AuthorLastNameBox.Text,  DescriptionBox.Text, 
            ImageBox.Text, Convert.ToInt32(YearBox.Text), 
            (Book.Genre)GenreBox.SelectedIndex, 
            (Book.Genre)Genre2Box.SelectedIndex, (Book.Media)MediaBox.SelectedIndex);  
            return book;
        }

        void nameboxes() //For development. Values will need to be hard coded.
        {
            TextBox[] boxeshere = { TitleBox, AuthorFirstNameBox, AuthorLastNameBox , YearBox, DescriptionBox, ImageBox};

            foreach(TextBox t in boxeshere)
            {
                t.Text = t.Tag.ToString();
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

        private void Empty_Box(object sender, RoutedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (t.Text == "")
            {
                t.Text = t.Tag.ToString();
                t.Foreground = DefaultColor();
            }
        }

        void InitializeListBoxes()
        {
            for (int i = 0; i < Book.Genres.Length; i++ )
            {
                GenreBox.Items.Add(Book.Genres[i]);
                Genre2Box.Items.Add(Book.Genres[i]);
            }

            for (int i = 0; i< Book.Medias.Length; i++)
            {
                MediaBox.Items.Add(Book.Medias[i]);
            }

            GenreBox.SelectedIndex = 0;
            Genre2Box.SelectedIndex = 0;
            MediaBox.SelectedIndex = 0;
        }

        bool CheckValidYear() //Checks textbox text is able to be converted to int. I considered using a listbox element instead but as release years may be hundreds of years ago that would be annoying.
        {
            int yearInt; 
            if (!int.TryParse(YearBox.Text, out yearInt)) 
            {
                return false;
            }
            return true;         
        }

        void AddBook()
        {
            if (DatabaseManager.ValidNumberString(YearBox.Text) == false)
            {
                YearBox.Text = "Please enter an Interger Value";
                return;
            }
            DatabaseManager.AddToChanges(makeBookFromForm());
            DatabaseManager.CommitAllChanges();
            Submit_Button.Content = "Item added to catalog";
        }

        void Go_Back_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.AdminInitial();
        }


        private void Genre2Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
   
        }

        private void MediaBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            AddBook();
        }

        private void TitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
