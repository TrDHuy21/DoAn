using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Infrastructure.Context;
using Infrastructure.Repo.Interface;

namespace Infrastructure.Repo.Implementation
{
    internal class ProductImageRepo : GenericRepo<ProductImage, (int, int)>, IProductImageRepo
    {
        public ProductImageRepo(ShopTechContext context) : base(context)
        {
        }
    }
}
