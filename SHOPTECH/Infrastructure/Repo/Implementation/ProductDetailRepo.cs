using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Domain.Enity;
using Infrastructure.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repo.Implementation
{
    public class ProductDetailRepo : GenericRepo<ProductDetail, int>, IProductDetailRepo
    {
        public ProductDetailRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }

        public override async Task<ProductDetail?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<ProductDetail>()
                .Include(b => b.Image)
                .Include(c => c.Product)
                .FirstAsync(c => c.Id == id);

            return entity;
        }

        public async Task<IEnumerable<ProductDetail>> GetByProductIdAsync(int productId)
        {
            var productDetails = await _context.ProductDetails
                .Include(b => b.Image)
                .Include(c => c.Product)
                .Where(c => c.ProductId == productId)
                .ToListAsync();
            return productDetails;
        }
    }
}
