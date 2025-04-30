using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repo.Interface;
using Infrastructure.Context;
using Domain.Enity;

namespace Infrastructure.Repo.Implementation
{
    public class CartRepo : GenericRepo<Cart, (int, int)>, ICartRepo
    {
        public CartRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }

    }
}
