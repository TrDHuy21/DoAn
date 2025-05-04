using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Infrastructure.Repo.Interface
{
    public interface IProductDetailRepo : IGenericRepo<ProductDetail, int>
    {
		public Task<IEnumerable<ProductDetail>> GetByProductIdAsync(int productId);

    }
}
