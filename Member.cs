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
    public class Member : IDatabaseElement
    {
        public static int idCount; //Holds current unique id number count;
        public int id { get; set; }
        public string name { get; set; }
        public string DOB { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public Member(string _name, string _DOB, string _username, string _password) 
        {
            assignID(); 
            name = _name;
            DOB = _DOB;
            username = _username;
            password = _password;
        }

        public Member()
        {
     
        }

        void assignID()
        {
            id = idCount;
            idCount++;
        }

        public void WriteToDB()
        {
           DatabaseManager.AddMemberToDB(this);
        }
    }
}
