using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiiTaxi.Models
{
    public class Taxi
    {
        public Taxi() {}

        public Taxi(string from, string to, DateTime date) {
            this.From = from;
            this.To = to;
            this.Date = date;
        }

        public int TaxiID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }

        public Person Owner { get; set; }
        public Person Approver { get; set; }

        public bool ownerConfirmed { get; set; }
        public bool approverConfirmed { get; set; }
        public bool bigTaxi { get; set; }

        public virtual List<Person> Passengers { get; set; }
    }
}