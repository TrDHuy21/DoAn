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
    public class ProductDetailAttributeRepo : GenericRepo<ProductDetailAttribute, (int, int)>, IProductDetailAttributeRepo
    {
        public ProductDetailAttributeRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }

        public override async Task<ProductDetailAttribute?> GetByIdAsync((int, int) id)
        {
            var productDetailAttribute = await _context.Set<ProductDetailAttribute>()
                .Include(x => x.Image)
                .Include(x => x.ProductDetail)
                .Include(x => x.ProductAttribute)
                .FirstOrDefaultAsync(x => x.ProductDetailId == id.Item1 && x.ProductAttributeId == id.Item2);

            return productDetailAttribute;
        }
       
        public async Task<IEnumerable<ProductDetailAttribute>> GetByProductAttributeId(int ProductAttributeId)
        {
            var productDetailAttributes = await _context.Set<ProductDetailAttribute>()
                .Where(x => x.ProductDetailId == ProductAttributeId)
                .ToListAsync();

            return productDetailAttributes;
        }

        public Task<IEnumerable<ProductDetailAttribute>> GetByProductDetailIdAndProductAttributeId((int, int) id)
        {
            throw new NotImplementedException();
        }
    }
}
