using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Infrastructure.Repo.Interface
{
    public interface ICartRepo : IGenericRepo<Cart, (int, int)>
    {
        
    }
}
