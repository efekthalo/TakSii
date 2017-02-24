using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
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
                Context.SaveChanges();
            }
            else
            {
                var metadata = ((IObjectContextAdapter)Context).ObjectContext.MetadataWorkspace;
                var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));
                var entityMetadata = metadata.GetItems<EntityType>(DataSpace.OSpace).Single(e => objectItemCollection.GetClrType(e) == typeof(T));
                if (entityMetadata.KeyProperties.Count > 1)
                    throw new NotImplementedException();

                var keyName = entityMetadata.KeyProperties[0].Name;

                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.Name.Contains(keyName))
                        continue;

                    var value = property.GetValue(update);
                    property.SetValue(entity, value);
                }

                Context.SaveChanges();
            }

            return entity;
        }

        public virtual void Delete<T>(string propertyToSelectBy, object valueToSelectBy) where T : class
        {
            var entity = GetEntityBy<T>(propertyToSelectBy, valueToSelectBy);
            Delete(entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
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

    public partial class SiiTaxiEntities
    {
        public SiiTaxiEntities(bool initDb = true)
            : base("name=SiiTaxiEntities")
        {
            if (initDb)
            {
                try
                {
                    Database.SetInitializer(new CreateDatabaseIfNotExists<SiiTaxiEntities>());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}