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
    public class CategoryRepo : GenericRepo<Category, int>, ICategoryRepo   
    {
        public CategoryRepo(ShopTechContext dbContext) : base(dbContext)
        {

        }

        public new async Task<Category?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<Category>()
                .Include(b => b.Image)
                .Include(c => c.ProductAttributes)
                .FirstAsync(c => c.Id == id);

            return entity;
        }
    }
}
