using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Domain.Enity;
using Infrastructure.Repo.Interface;

namespace Infrastructure.Repo.Implementation
{
    public class RoleRepo : GenericRepo<Role, int>, IRoleRepo
    {
       public RoleRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }
    }
}
