using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Application.Service.Interface
{
    public interface IProductDetailAttributeService
    {
        Task<IEnumerable<ProductDetailAttribute>> GetAllAsync();
        Task<ProductDetailAttribute> GetByProductDetailIdAndProductAttributeId(int productDetailId, int productAttributeId);


    }
}
