using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiiTaxi.ViewModels
{
    public class TaxiOrderViewModel
    {
        public string OwnerName { get; set; }

        public string OwnerPhone { get; set; }

        public string OwnerEmail { get; set; }

        public DateTime Time { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Approver { get; set; }

        public bool IsBigTaxi { get; set; }

        public IList<String> PassangerList { get; set; }

    }
}