namespace SiiTaxi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SiiTaxi.Models;
    using System.Collections.Generic;
    using System.Globalization;


    internal sealed class Configuration : DbMigrationsConfiguration<SiiTaxi.Models.TaxiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaxiContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var jednaLista = new List<Person> { new Person { Name = "Pierwszy pasa¿er" } , new Person { Name = "Drugi pasa¿er" } };
            var drugaLista = new List<Person> { new Person { Name = "inny pasa¿er" }, new Person { Name = "Drugi inny pasa¿er" } };




            context.People.AddOrUpdate(x => x.PersonID,
                new Person("Adam Kowalski", "a.kowalski@sd.xd", "trololo@kotek.com", "0543432432"),
                new Person("£ukasz Nowak", "kiki@ksok.ss", "ajaja@wsa.pl", "0333322233"),
                new Person("Tadeusz Koœciuszko", "t.kosciuszko@kosciuszko.pl", "ajajaj@aj.aj", "012341234"),
                new Person("Karolina Ahoj", "kahoj@miau.pl", "tr@xd.com", "01230987"),
                new Person("Joann Pryk", "jjsdh@sjja.pl", "ds.sa", "1234123421")


                );


            context.Taxis.AddOrUpdate(x => x.TaxiID,
                new Taxi("Intel", "Sii", DateTime.ParseExact("2017-02-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)) { Passengers = jednaLista},
                new Taxi("Wrzeszcz", "Sopot", DateTime.ParseExact("2017-02-07 12:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)) { Passengers = drugaLista },
                new Taxi("Sopot", "Oliwa", DateTime.ParseExact("2017-02-07 10:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)),
                new Taxi("Sii", "Lotnisko", DateTime.ParseExact("2017-02-09 15:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)),
                new Taxi("Gdañsk", "Sopot", DateTime.ParseExact("2017-02-08 17:50:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)),
                new Taxi("Gdynia", "Hel", DateTime.ParseExact("2017-02-08 07:20:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)),
                new Taxi("Przymorze", "Osowa", DateTime.ParseExact("2017-02-08 16:10:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture)),
                new Taxi("Intel", "Lotnisko", DateTime.ParseExact("2017-02-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture))
                );
        }
    }
}
