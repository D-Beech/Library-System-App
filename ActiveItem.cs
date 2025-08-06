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
    public class Message : IDatabaseElement
    {
        ActiveItem item;
        public string details {get; set;}
        public bool messageRead { get; set; }
        public int memberId { get; set; }
        public Message(ActiveItem _item)
        {
            item = _item;
            messageRead = false;
            memberId = _item.memberId;
            details = FillDetails();
        }
        public Message()
        {
        
        }

        public string FillDetails()
        {
            string messageDetails;
            messageDetails = $"ID: {item.id}\nMember ID: {item.memberId} ({DatabaseManager.SearchMemberById(item.memberId).Last().name})\n";
            messageDetails += $"Item ID: {item.bookId} ({DatabaseManager.SearchCatalogById(item.bookId).Last().title})\n";
            messageDetails += $"Time Logged: {item.dt}";
            return messageDetails;
        }

        public virtual void WriteToDB()
        {

        }

    }

    public class ReturnedMessage : Message
    { 
        public ReturnedMessage(Returned r) : base(r)
        {
            WriteToDB();
        }

        public ReturnedMessage() : base()
        {

        }

        public override void WriteToDB()
        {
            DatabaseManager.SaveReturnedMessageToFile(this);
        }
    }

    public class OverdueMessage : Message
    {
        public OverdueMessage(Overdue o) : base (o)
        {
            WriteToDB();
        }

        public OverdueMessage(): base()
        {

        }

        public override void WriteToDB()
        {
            DatabaseManager.SaveOverDueMessageToFile(this);
        }
    }

    public class DueDateMessage : Message
    {
        public DueDateMessage(Borrowed b) : base(b)
        {
            details += " Book Due in 3 Days"; 
            WriteToDB();
        }

        public DueDateMessage() : base()
        {

        }

        public override void WriteToDB()
        {
            DatabaseManager.SaveDueDateMessageToFile(this);
        }
    }


    public abstract class ActiveItem : IDatabaseElement
    {
        public int id { get; set; }
        public int memberId { get; set; }
        public int bookId { get; set; }

        public string dt { get; set; }

        public ActiveItem(Member _memberAccount, Book _book)
        {
            memberId = _memberAccount.id;
            bookId = _book.id;
            dt = DateTime.Now.ToString();
        }

        public ActiveItem(ActiveItem copy)
        {

        }

        public ActiveItem ()
        {

        }

        protected virtual void assignId()
        {
         
        }

        public virtual void logMessage()
        {
            
        }

        public virtual void attachThisId(Member _memberAccount, Book _book)
        {
           
        }

    }

    public class Reserved : ActiveItem , IDatabaseElement
    {
        static public int idCount;

        public Reserved(Member _memberAccount, Book _book) : base(_memberAccount, _book)
        {
            assignId();
            logMessage();
            _book.available = false;
            _book.reserved = true;
            attachThisId(_memberAccount, _book);
            dt = DateTime.Now.ToString();
        }

        public Reserved(): base()
        {        
        }

        public override void attachThisId(Member _memberAccount, Book _book)
        {
            _book.newReservation(memberId);
        }

        public override void logMessage()
        {

        }

        protected override void assignId()
        {
            idCount++;
            id = idCount;
        }

        public void WriteToDB()
        {
            DatabaseManager.AddReservedItemToDB(this);
        }

    }

    public class Borrowed : ActiveItem, IDatabaseElement
    {
        public static int idCount;

        public Borrowed(Member _memberAccount, Book _book) : base(_memberAccount, _book)
        {
            assignId();
            logMessage();
            attachThisId(_memberAccount, _book);
            dt = DateTime.Now.ToString();
        }
        public Borrowed(): base()
        {

        }

        public override void logMessage()
        {

        }

        protected override void assignId()
        {
            idCount++;
            id = idCount;
        }

        public void WriteToDB()
        {
            DatabaseManager.AddBorrowedItemToDB(this);
        }

    }

    public class Returned : ActiveItem, IDatabaseElement
    {
        public Returned(Borrowed item) : base(item)
        {
            id = item.id;
            memberId = item.memberId;
            bookId = item.bookId;
            dt = DateTime.Now.ToString();
            logMessage();

        }

        public Returned() : base()
        {

        }

        protected override void assignId()
        {

        }

        public override void logMessage()
        {
            new ReturnedMessage(this);
        }

        public override void attachThisId(Member _memberAccount, Book _book)
        {
            
        }

        public void WriteToDB()
        {
            DatabaseManager.AddReturnedItemToDB(this);
        }


    }

    public class Overdue : ActiveItem, IDatabaseElement
    {
        public Overdue(Borrowed item) : base()
        {
            id = item.id;
            memberId = item.memberId;
            bookId = item.bookId;
            dt = DateTime.Now.ToString();
            logMessage();
        }

        public Overdue() : base()
        {
            
        }

        public override void logMessage()
        {
            new OverdueMessage(this);
        }

        public void WriteToDB()
        {
            DatabaseManager.AddOverdueItemToDB(this);
        }
    }


}
