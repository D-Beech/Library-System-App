using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using Dapper;
using System.IO;

namespace FinalProject
{
    public class Book : IDatabaseElement
    {
        public static int idCount; //Holds the current unique id number counter. Count is initialized from sequence table when program starts.

        public enum Genre { FANTASY, COMDEDY, DRAMA, CHILDREN, YOUNGADULT, ADULT }
        static public string[] Genres = { "Fantasy", "Comedy", "Drama", "Children", "Young-Adult", "Adult" }; 
        public enum Media { BOOK, DVD, VHS, CD, MAGAZINE, COMIC }
        static public string[] Medias = { "Book", "DVD", "CD", "Magazine", "Comic" };
        public int id { get; set; }
        public string title { get; set; }
        public string authorFirstNames { get; set; }
        public string authorLastNames { get; set; }
        public string description { get; set; }
        public string imagePath { get; set; }
        public int year { get; set; }
        public Genre genre { get; set; }
        public Genre genre2 { get; set; }
        public Media media { get; set; }
        public bool available { get; set; }
        public bool reserved { get; set; }

        public Book() { }

        public Book(string _title, string _authorFirstNames, string _authorLastName, string _description, string _imagePath, int _year, Genre _genre, Genre _genre2, Media _media)
        {
            AssignID();
            title = _title;
            authorFirstNames = _authorFirstNames;
            authorLastNames = _authorLastName;
            description = _description;
            imagePath = _imagePath;
            year = _year;
            genre = _genre;
            genre2 = genre2;
            media = _media;
            available = true;
            reserved = false;
        }

        public Book(int _id, string _title, string _authorFirstNames, string _authorLastName, string _description, string _imagePath, int _year, Genre _genre, Genre _genre2, Media _media) //Used for updating details.
        {
            id = _id; //This is constructor is only called when a catalog item is being copied (it will not assign a unique id and cannot be used to add a new item to the database).
            title = _title;
            authorFirstNames = _authorFirstNames;
            authorLastNames = _authorLastName;
            description = _description;
            imagePath = _imagePath;
            year = _year;
            genre = _genre;
            genre2 = _genre2;
            media = _media;
        }

        public void newReservation(int memberId)
        {
            reserved = true;
        }

        public ItemCard makeCatalogCard()
        {
            return new ItemCard(this);
        }

        public void WriteToDB()
        {
            DatabaseManager.AddBookToDB(this);
        }

        void AssignID()
        {
            idCount++;
            id = idCount;
        }
    }
}




  
