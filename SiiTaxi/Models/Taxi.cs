using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiiTaxi.Models
{
    public class Taxi
    {
        public Taxi() {}

        public Taxi(string from, string to, DateTime date, int ownerId, int approverId) {
            this.From = from;
            this.To = to;
            this.Date = date;
            this.OwnerID = ownerId;
            this.ApproverID = approverId;
        }

        public int TaxiID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }

        public int OwnerID { get; set; }
        public virtual Person Owner { get; set; }

        public int ApproverID { get; set; }
        public virtual Person Approver { get; set; }

        public bool ownerConfirmed { get; set; }
        public bool approverConfirmed { get; set; }

        public virtual List<Person> Passengers { get; set; }
    }
}