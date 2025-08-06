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
    /// Interaction logic for UpdateCatalogItem.xaml
    /// </summary>
    public partial class UpdateCatalogItem : UserControl
    {
        Book book;
        public UpdateCatalogItem(Book _book)
        {
            book = _book;
            InitializeComponent();
            FillListBoxes();
            SetListBoxesToExistingIndex();
            FillExistingDetails();
            nameboxes();
        }

        Book BookWithNewDetails() //Trying to keep this readable.
        {
            Book updatedBook =
            new Book(book.id, TitleBox.Text, AuthorFirstNameBox.Text,
            AuthorLastNameBox.Text, DescriptionBox.Text,
            ImageBox.Text, Convert.ToInt32(YearBox.Text),
            (Book.Genre)GenreBox.SelectedIndex,
            (Book.Genre)Genre2Box.SelectedIndex, (Book.Media)MediaBox.SelectedIndex);
            return updatedBook;
        }

        void SetGenreInitial()
        {
            for (int i = 0; i < Book.Genres.Length; i++)
            {
                if (Book.Genres[Convert.ToInt32(book.genre)] == Book.Genres[i])
                {
                    GenreBox.SelectedIndex = i;
                    break;
                }
            }
        }

        void SetGenre2Initial()
        {
            for (int i = 0; i < Book.Genres.Length; i++)
            {
                if (Book.Genres[Convert.ToInt32(book.genre2)] == Book.Genres[i])
                {
                    Genre2Box.SelectedIndex = i;
                    break;
                }
            }
        }

        void SetMediaInitial()
        {
            for (int i = 0; i < Book.Medias.Length; i++)
            {
                if (Book.Medias[Convert.ToInt32(book.media)] == Book.Medias[i])
                {
                    MediaBox.SelectedIndex = i;
                    break;
                }
            }
        }

        void SetListBoxesToExistingIndex()
        {
            SetGenreInitial();
            SetGenre2Initial();
            SetMediaInitial();
        }

        void FillExistingDetails() //For development. Values will need to be hard coded.
        {
            TextBox[] boxeshere = { TitleBox, AuthorFirstNameBox, AuthorLastNameBox, YearBox, DescriptionBox, AuthorFirstNameBox, ImageBox };
            TitleBox.Tag = book.title;
            AuthorFirstNameBox.Tag = book.authorFirstNames;
            AuthorLastNameBox.Tag = book.authorLastNames;
            YearBox.Tag = book.year.ToString();
            DescriptionBox.Tag = book.description;
            ImageBox.Tag = book.imagePath;
        }

        void nameboxes()
        {
            TextBox[] boxeshere = { TitleBox, AuthorFirstNameBox, AuthorLastNameBox, YearBox, DescriptionBox, ImageBox };
            foreach (TextBox t in boxeshere)
            {
                if(t.Tag == null)
                {
                    t.Tag = "This information is not available";
                }
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
            TextBox[] boxeshere = { TitleBox, AuthorFirstNameBox, AuthorLastNameBox, YearBox, DescriptionBox, ImageBox };
            string[] boxesTitles = { "Title", "Author First Names", "Author Last Name", "Year", "Description", "Image" };
            TextBox t = (TextBox)sender;
            if (t.Text == "")
            {
                for (int i = 0; i < boxeshere.Length; i++)
                {
                    if (t.Name == boxeshere[i].Name)
                    {
                        t.Text = boxesTitles[i];
                    }
                }
                t.Foreground = DefaultColor();
            }
        }

        void FillListBoxes()
        {
            for (int i = 0; i < Book.Genres.Length; i++)
            {
                GenreBox.Items.Add(Book.Genres[i]);
                Genre2Box.Items.Add(Book.Genres[i]);
            }

            for (int i = 0; i < Book.Medias.Length; i++)
            {
                MediaBox.Items.Add(Book.Medias[i]);
            }
        }

        void AddBook()
        {
            if (DatabaseManager.ValidNumberString(YearBox.Text) == false)
            {
                YearBox.Text = "Please enter an Interger Value";
                return;
            }
            DatabaseManager.UpdateCatalogItemDetails(BookWithNewDetails());
            Submit_Button.Content = "Item Details Have Been Updated";
        }

        void Go_Back_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.CatalogRegularAccess();
        }
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            AddBook();
        }
    }
}

