using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiiTaxi.Models
{
    public class Person
    {
        public Person() {}

        public Person(string name, string email, string emailAlt, string phone)
        {
            this.Name = name;
            this.Email = email;
            this.EmailAlt = emailAlt;
            this.Phone = phone;
        }

        public int PersonID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string EmailAlt { get; set; }
        public string Phone { get; set; }
        public bool IsApprover { get; set; }
    }
}