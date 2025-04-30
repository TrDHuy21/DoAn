using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;
using Infrastructure.Context;
using Infrastructure.Repo.Interface;

namespace Infrastructure.Repo.Implementation
{
    public class DistrictRepo : GenericRepo<District, int>, IDistrictRepo
    {
        public DistrictRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }
    }
}
