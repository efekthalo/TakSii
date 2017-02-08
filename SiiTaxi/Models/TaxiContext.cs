using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SiiTaxi.Models
{
    public class TaxiContext : DbContext
    {
        public DbSet<Taxi> Taxis { get; set; }
        public DbSet<Person> People { get; set; }
    }
}