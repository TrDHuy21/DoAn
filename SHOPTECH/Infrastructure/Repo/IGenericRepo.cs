using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
    public interface IGenericRepo<E, K> where E : class
    {
        public Task<E?> GetByIdAsync(K? id);

        public IQueryable<E> GetAll();

        public Task AddAsync(E entity);

        public Task UpdateAsync(E entity);

        public Task DeleteAsync(K id);
        public Task DeleteAsync(E entity);

        public Task<IEnumerable<E>> FindAsync(Expression<Func<E, bool>> predicate);
    }
}
