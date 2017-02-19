﻿using System.Linq;

namespace SiiTaxi.Models
{
    public sealed class TaxiPeopleViewModel : AbstractViewModel
    {
        public IQueryable<TaxiPeople> TaxiPeople;

        public TaxiPeopleViewModel()
        {
            Context = new SiiTaxiEntities(true);
            TaxiPeople = Get<TaxiPeople>();
        }
    }
}
