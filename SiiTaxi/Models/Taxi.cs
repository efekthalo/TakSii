using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiiTaxi.Models
{
    public class Taxi
    {
        [Key]
        public int TaxiID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public People Owner { get; set; }
        public IList<People> Additional { get; set; }
        public DateTime Time { get; set; }
    }
}