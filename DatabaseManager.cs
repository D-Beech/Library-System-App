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
    public enum TABLE { BORROWED, CATALOG, MEMBERS, OVERDUE, RESERVED, RETURNED, SEQUENCE };
    public enum COLUMN { ID, MEMBERID, BOOKID };
    public interface IDatabaseElement
    {
        public void WriteToDB()
        {

        }
    }

    public class DatabaseManager
    {
        public static int maxBorrowPeriod = 7;
        public static int dueDateNotifyPeriod = 3;

        static string catalogFileName = "LibrarySystemDB.db";
        static string source = $"Data Source={Path.Combine(Directory.GetCurrentDirectory(), catalogFileName)}";

        public delegate void writeToDB();
        public static event writeToDB CommitChanges;

        public static void AddToChanges(IDatabaseElement e)
        {
            CommitChanges += () => e.WriteToDB();
        }

        public static void CommitAllChanges()
        {
            CommitChanges?.Invoke();
            CommitChanges = delegate { }; //Check with Lawernce, do I need to use this to reset?
        }

        public static bool ValidNumberString(string input) //Checks textbox text is able to be converted to int. I considered using a listbox element instead but as release years may be hundreds of years ago that would be annoying.
        {
            int number;
            if (!int.TryParse(input, out number))
            {
                return false;
            }
            return true;
        }

        public static void InitializeIDCount()
        {
            Member.idCount = loadIdSequence()[0];
            Reserved.idCount = loadIdSequence()[2];
            Book.idCount = loadIdSequence()[3];
            Borrowed.idCount = loadIdSequence()[1];
            
        }

        public static int[] loadIdSequence()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
      
                var output = connection.Query<int>("select seq from sqlite_sequence", new DynamicParameters());
                connection.Close();

                return output.ToArray();
            }
        }

        static List<Overdue> CheckIfBorrowedItemsAreOverdue()
        {
            List<Borrowed> AllBorrowedItems = LoadBorrowedItems();
            List<Overdue> NewOverDueItems = new List<Overdue>();
            foreach (Borrowed b in AllBorrowedItems)
            {
                DateTime dateBorrowed;

                if (!DateTime.TryParse(b.dt, out dateBorrowed))
                {
                    continue; //skips items with incovertible/missing datetime strings.
                }

                if (DateTime.Now - dateBorrowed > TimeSpan.FromHours((maxBorrowPeriod * 24))) 
                    //checks how many days (24hr periods) have passed between dateBorrowed and dateNow. 
                    //If they exceeed max period new overdue item is created.
                {
                    NewOverDueItems.Add(new Overdue(b));
                }
            }
            return NewOverDueItems;

        }

        public static void CheckIfDueDatesUpcoming()
        {
            List<Borrowed> AllBorrowedItems = LoadBorrowedItems();
            
            foreach (Borrowed b in AllBorrowedItems)
            {
                DateTime dateBorrowed;

                if (!DateTime.TryParse(b.dt, out dateBorrowed))
                {
                    continue; //skips items with incovertible/missing datetime strings.
                }

                if (DateTime.Now - dateBorrowed > TimeSpan.FromHours(dueDateNotifyPeriod * 24)) 
                    //checks how many days (24hr periods) have passed between dateBorrowed and dateNow. 
                    //If they exceeed max period new overdue item is created.
                {
                    new DueDateMessage(b);
                }
            }
            

        }

        public static void CheckForOverdueItems()
        {
            List<Overdue> NewOverDueItems = CheckIfBorrowedItemsAreOverdue();
            foreach (Overdue o in NewOverDueItems)
            {
                AddToChanges(o);
            }
            CommitAllChanges();
        }



        public static bool CheckMemberIdExists(int input)
        {
            List<Member> members = LoadMembers();
            foreach (Member M in members)
            {
                if (M.id == input)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckReservedIdExists(int input)
        {
            List<Reserved> reserved = LoadReservedItems();
            foreach (Reserved r in reserved)
            {
                if (r.id == input)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckBorrowedIdExists(int input)
        {
            List<Borrowed> borrowed = LoadBorrowedItems();
            foreach (Borrowed b in borrowed)
            {
                if (b.id == input)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckBookIdExists(int input)
        {
            List<Book> books = DatabaseManager.LoadBooks();
            foreach (Book b in books)
            {
                if (b.id == input)
                {
                    return true;
                }
            }
            return false;
        }

        public static Reserved makeReservedItemFromDB(int itemId)
        {
            List<Reserved> reservedItems = DatabaseManager.LoadReservedItems();
            foreach (Reserved r in reservedItems)
            {
                if (r.id == itemId)
                {
                    return r;
                }
            }
            return new Reserved(); //this is a bit hacky.
        }

        public static Returned makeReturnedItemFromDB(int itemId)
        {
            List<Returned> returnedItems = DatabaseManager.LoadReturnedItems();
            foreach (Returned r in returnedItems)
            {
                if (r.id == itemId)
                {
                    return r;
                }
            }
            return null; //this is a bit hacky.
        }

        public static Borrowed makeBorrowedItemFromDB(int itemId)
        {
            List<Borrowed> borrowedItem = DatabaseManager.LoadBorrowedItems();
            foreach (Borrowed b in borrowedItem)
            {
                if (b.id == itemId)
                {
                    return b;
                }
            }
            return null; //this is a bit hacky.
        }
        //Member Start
        public static List<Member> LoadMembers()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<Member>("select * from Members", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }

        public static void AddMemberToDB(Member memberToStore)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into Members(name, DOB, username, password) values (@name, @DOB, @username, @password)";
                connection.Execute(insertQuery, memberToStore);
                connection.Close();
            }
        }
        //Member End




        //Catalog Start

        public static List<Book> LoadBooks()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<Book>("select * from Catalog", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }
        public static void AddBookToDB(Book bookToStore)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into Catalog(id, title, authorFirstNames, authorLastNames, imagePath, year, genre, genre2, media, available, reserved, description) values (@id, @title, @authorFirstNames, @authorLastNames, @imagePath, @year, @genre, @genre2, @media, @available, @reserved, @description)";
                connection.Execute(insertQuery, bookToStore);
                connection.Close();
            }
        }

        public static void UpdateCatalogItemDetails(Book book)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = $"UPDATE Catalog SET title = '{book.title}', authorFirstNames = '{book.authorFirstNames}', authorLastNames = '{book.authorLastNames}', imagePath = '{book.imagePath}', description = '{book.description}', year = '{book.year}', genre = '{book.genre}', genre2 = '{book.genre2}', media = '{book.media}' WHERE id = '{book.id}'";
                connection.Execute(insertQuery, book);
                connection.Close();
            }
        }

        public static void UpdateCatalogReservedandAvailableStatus(Book book)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = $"UPDATE Catalog SET available = '{book.available}', reserved = {book.reserved} WHERE id = '{book.id}'";
                connection.Execute(insertQuery, book);
                connection.Close();
            }
        }

        public static void UpdateMemberDetails (Member member)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = $"UPDATE Members SET name = '{member.name}', DOB = '{member.DOB}', username = '{member.username}', password = '{member.password}' WHERE id = '{member.id}'";
                connection.Execute(insertQuery, member);
                connection.Close();
            }
        }


        //Catalog End
        //Reserved Item Start
        public static List<Reserved> LoadReservedItems()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<Reserved>("select * from ReservedItem", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }

        public static void AddReservedItemToDB(ActiveItem item)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into ReservedItem(id, memberId, bookId, dt) values (@id, @memberId, @bookId, @dt)";
                connection.Execute(insertQuery, item);
                connection.Close();
            }
        }
        //reservedItem end

        public static void AddOverdueItemToDB(ActiveItem item)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into OverdueItem(id, memberId, bookId) values (@id, @memberId, @bookId)";
                connection.Execute(insertQuery, item);
                connection.Close();
            }
        }

        //Borrowed Item Start
        public static List<Borrowed> LoadBorrowedItems()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<Borrowed>("select * from BorrowedItem", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }
        public static void AddBorrowedItemToDB(ActiveItem item)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into BorrowedItem(id, memberId, bookId) values (@id, @memberId, @bookId)";
                connection.Execute(insertQuery, item);
                connection.Close();
            }
        }
        //borrowedItem end
        public static List<Returned> LoadReturnedItems()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<Returned>("select * from ReturnedItem", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }
        public static void AddReturnedItemToDB(Returned item)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into ReturnedItem(id, memberId, bookId) values (@id, @memberId, @bookId)";
                connection.Execute(insertQuery, item);
                connection.Close();
            }
        }


        //////

        public enum TABLE { BORROWED, OVERDUE, RESERVED, RETURNED };
        public enum COLUMN { ID, MEMBERID, BOOKID };

        static string[] tables = { "BorrowedItem", "OverdueItem", "ReservedItem", "ReturnedItem"};

        static string[] columns = { "id", "memberId", "bookId" };

      
        static string constructActiveItemIdQuery(TABLE t, COLUMN c, int id)
        {
            return "SELECT * FROM " + tables[Convert.ToInt32(t)] + " WHERE " + columns[Convert.ToInt32(c)] + $" = '{id}'";
        }

        public static List<Reserved> SearchReservedById(TABLE t, COLUMN c, int id)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = constructActiveItemIdQuery(t, c, id);
                var output = connection.Query<Reserved>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }
        public static List<Borrowed> SearchBorrowedById(TABLE t, COLUMN c, int id)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = constructActiveItemIdQuery(t, c, id);
                var output = connection.Query<Borrowed>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }

        public static List<Returned> SearchReturnedById(TABLE t, COLUMN c, int id)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = constructActiveItemIdQuery(t, c, id);
                var output = connection.Query<Returned>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }

        public static List<Overdue> SearchOverdueById(TABLE t, COLUMN c, int id)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = constructActiveItemIdQuery(t, c, id);
                var output = connection.Query<Overdue>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }



        public static List<Book> SearchCatalogById(int inputId, TABLE t, COLUMN c)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = $"SELECT * FROM Catalog WHERE id = '{inputId}'";

                var output = connection.Query<Book>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }

        public static List<Book> SearchCatalogById(int inputId) //Look Again.
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = $"SELECT * FROM Catalog WHERE id = '{inputId}'";

                var output = connection.Query<Book>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }

        public static List<Member> SearchMemberById(int inputId) //Look Again.
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string query = $"SELECT * FROM Members WHERE id = '{inputId}'";

                var output = connection.Query<Member>(query, new DynamicParameters());
                connection.Close();
                return output.ToList(); ;
            }
        }


        //Messages Starts
        public static void SaveReturnedMessageToFile(Message m)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into ReturnedMessage (details, messageRead, memberId)values (@details, @messageRead, @memberId)";
                connection.Execute(insertQuery, m);
                connection.Close();
            }
        }

        public static void SaveDueDateMessageToFile(Message m)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into DueDateMessage (details, messageRead, memberId)values (@details, @messageRead, @memberId)";
                connection.Execute(insertQuery, m);
                connection.Close();
            }
        }

        public static void SaveOverDueMessageToFile(Message m)
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                string insertQuery = "insert into OverDueMessage (details, messageRead, memberId)values (@details, @messageRead, @memberId)";
                connection.Execute(insertQuery, m);
                connection.Close();
            }
        }

        public static List<ReturnedMessage> LoadReturnedMessages()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<ReturnedMessage>("select * from ReturnedMessage", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }

        public static List<OverdueMessage> LoadOverDueMessages()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<OverdueMessage>("select * from OverDueMessage", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }
        public static List<DueDateMessage> LoadDueDateMessages()
        {
            using (IDbConnection connection = new SQLiteConnection(source))
            {
                connection.Open();
                var output = connection.Query<DueDateMessage>("select * from DueDateMessage", new DynamicParameters());
                connection.Close();
                return output.ToList();
            }
        }

        /*   public static List<Reserved> SearchReservedById(int inputId)
           {
               using (IDbConnection connection = new SQLiteConnection(source))
               {
                   connection.Open();
                   string query = $"SELECT * FROM ReservedItem WHERE id = '{inputId}'";

                   var output = connection.Query<Reserved>(query, new DynamicParameters());
                   connection.Close();
                   return output.ToList(); ;
               }
           }
   */

    }
}
