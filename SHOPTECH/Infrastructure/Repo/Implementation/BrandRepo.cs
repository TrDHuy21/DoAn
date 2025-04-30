using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;
using Infrastructure.Context;
using Infrastructure.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repo.Implementation
{
    public class BrandRepo : GenericRepo<Brand, int>, IBrandRepo
    {
       public BrandRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }
        public new async Task<Brand?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<Brand>()
                .Include(b => b.Image)
                .FirstOrDefaultAsync(b => b.Id == id);

            return entity;
        }
    }
    
}
