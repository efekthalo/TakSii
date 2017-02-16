using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace SiiTaxi.Models
{
    public abstract class AbstractViewModel
    {
        internal SiiTaxiEntities Context;

        public virtual IQueryable<T> Get<T>() where T : class
        {
            var list = Context.Set<T>();
            return list ?? new List<T>().AsQueryable();
        }

        public virtual T GetEntityBy<T>(string propertyToSelectBy, object valueToSelectBy) where T : class
        {
            var whereClause = "{0} = {1}";
            if (valueToSelectBy is string)
            {
                whereClause = "{0} = \"{1}\"";
            }

            var predicate = string.Format(whereClause, propertyToSelectBy, valueToSelectBy);
            var entity = Context.Set<T>().Where(predicate).FirstOrDefault();
            return entity;
        }

        public virtual T UpdateEntityBy<T>(string propertyToSelectBy, T update) where T : class
        {
            var entityValue = update.GetType().GetProperty(propertyToSelectBy).GetValue(update, null);
            var entity = GetEntityBy<T>(propertyToSelectBy, entityValue);
            return UpdateEntity(entity, update);
        }

        public virtual T UpdateEntity<T>(T entity, T update) where T : class
        {
            if (entity == null)
            {
                entity = (T)Context.Set(typeof(T)).Add(update);
            }
            else
            {
                Context.Entry(entity).CurrentValues.SetValues(update);
            }

            Context.SaveChanges();
            return entity;
        }

        public virtual void Delete<T>(string propertyToSelectBy, object valueToSelectBy) where T : class
        {
            var entity = GetEntityBy<T>(propertyToSelectBy, valueToSelectBy);
            if (entity != null)
            {
                Context.Set<T>().Remove(entity);
                Context.SaveChanges();
            }
            else
            {
                throw new NotImplementedException();
            }            
        }
    }
}