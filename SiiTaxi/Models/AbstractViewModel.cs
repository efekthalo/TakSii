using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SiiTaxi.Models
{
    public abstract class AbstractViewModel
    {
        public SiiTaxiEntities _context;

        public T GetEntityBy<T>(string propertyToSelectBy, object valueToSelectBy)
        {
            return _context.Set(typeof(T)).Where(string.Format("{0} = {1}", propertyToSelectBy, valueToSelectBy)).;
        }

        public T UpdateEntityBy<T>(string propertyToSelectBy, T update)
        {
            var entityValue = update.GetType().GetProperty(propertyToSelectBy).GetValue(update, null);
            var entity = GetEntityBy<T>(propertyToSelectBy, entityValue);
            return UpdateEntity<T>(entity, update);
        }

        private T UpdateEntity<T>(T entity, T update)
        {
            if (entity == null)
            {
                entity = (T)_context.Set(typeof(T)).Add(update);
            }
            else
            {
                _context.Entry<T>(entity).CurrentValues.SetValues(update);
            }

            _context.SaveChanges();
            return entity;
        }
    }
}