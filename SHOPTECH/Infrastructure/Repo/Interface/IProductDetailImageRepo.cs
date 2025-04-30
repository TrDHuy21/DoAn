using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Infrastructure.Repo.Interface
{
    internal interface IProductDetailImageRepo : IGenericRepo<ProductDetailImage, (int, int)>
    {
    }
}
