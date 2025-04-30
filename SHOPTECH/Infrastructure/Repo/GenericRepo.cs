using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repo
{
    public class GenericRepo<E, K> : IGenericRepo<E, K> where E : class 
                                                        
    {
        protected readonly ShopTechContext _context;

        public GenericRepo(ShopTechContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(E entity)
        {
            await _context.Set<E>().AddAsync(entity);
        }

        public async Task DeleteAsync(K id)
        {
            var entity = await _context.Set<E>().FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity not found");
            }
            _context.Set<E>().Remove(entity);
        }

        public virtual Task DeleteAsync(E entity)
        {
            _context.Set<E>().Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> predicate)
        {
            var List = await _context.Set<E>().Where(predicate).ToListAsync();
            return List;

        }

        public virtual IQueryable<E> GetAll()
        {
            return _context.Set<E>().AsQueryable();
        }

        public virtual async Task<E?> GetByIdAsync(K? id)
        {
            if (id == null)
            {
                return null;
            }
            var entity = await _context.Set<E>().FindAsync(id);

            return entity;
        }

        public virtual Task UpdateAsync(E entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
