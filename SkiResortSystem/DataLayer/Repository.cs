using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    /// <summary>
    ///  Generic repository class.
    /// </summary>
    public class Repository<T>
        where T : class
    {

        protected AppDbContext Context { get; }
        protected DbSet<T> Table { get; }

        internal Repository(AppDbContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }

        /// <summary>
        ///  Add a new entity to the Table.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            Table.Add(entity);
        }

        /// <summary>
        /// Updatein entity with new attributes
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            Table.Update(entity);
        }

        /// <summary>
        ///  Remove an entity from the Table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true if removed and false otherwise.</returns>
        public bool Remove(T entity)
        {
            EntityEntry ee = Table.Remove(entity);
            return ee.State.Equals(EntityState.Deleted);
        }

        /// <summary>
        ///  Find a set of entities that match a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<T> Find(Func<T, bool> predicate)
        {
            return Table.Where(predicate).ToList();
        }

        public IList<T> Find(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = Table.AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.Where(predicate).ToList();
        }

        /// <summary>
        ///  Find the first entity that match a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        public T FirstOrDefault(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = Table.AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        ///  Is this repository empty?
        /// </summary>
        /// <returns>true is it is empty, false otherwise.</returns>
        public bool IsEmpty()
        {
            return Table.Count() == 0;
        }

        /// <summary>
        ///  Count the entities in the Table.
        /// </summary>
        /// <returns>the number of entities.</returns>
        public int Count()
        {
            return Table.Count();
        }
    }
}