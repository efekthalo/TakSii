using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace SiiTaxi.Models
{
    public abstract class AbstractViewModel
    {
        public SiiTaxiEntities _context;

        public IQueryable<T> Get<T>() where T : class
        {
            var list = _context.Set<T>();
            return list == null ? new List<T>().AsQueryable() : list;
        }

        public T GetEntityBy<T>(string propertyToSelectBy, object valueToSelectBy) where T : class
        {
            return _context.Set<T>().Where(string.Format("{0} = {1}", propertyToSelectBy, valueToSelectBy)).FirstOrDefault();
        }

        public T UpdateEntityBy<T>(string propertyToSelectBy, T update) where T : class
        {
            var entityValue = update.GetType().GetProperty(propertyToSelectBy).GetValue(update, null);
            var entity = GetEntityBy<T>(propertyToSelectBy, entityValue);
            return UpdateEntity(entity, update);
        }

        private T UpdateEntity<T>(T entity, T update) where T : class
        {
            if (entity == null)
            {
                entity = (T)_context.Set(typeof(T)).Add(update);
            }
            else
            {
                _context.Entry(entity).CurrentValues.SetValues(update);
            }

            _context.SaveChanges();
            return entity;
        }

        public void Delete<T>(string propertyToSelectBy, object valueToSelectBy) where T : class
        {
            var entity = GetEntityBy<T>(propertyToSelectBy, valueToSelectBy);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new NotImplementedException();
            }            
        }
    }
}