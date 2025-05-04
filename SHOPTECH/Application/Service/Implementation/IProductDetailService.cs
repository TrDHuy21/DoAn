using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.BrandDtos;
using Application.Dtos;
using Domain.Enity;
using Application.Dtos.ProductDetailDtos;

namespace Application.Service.Implementation
{
    public interface IProductDetailService
    {
        Task<ProductDetail?> AddAsync(AddProductDetailDto brandDto);
        Task<ProductDetail?> UpdateAsync(UpdateProductDetailDto brandDto);
        Task<bool> DeleteAsync(int id);
        Task<ProductDetail?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDetail>?> GetAllAsync();
        Task<PageResultDto<IndexProductDetailDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<ProductDetail?> ChangeActive(int id, bool isActive);

        Task<IEnumerable<ProductDetail>?> GetByProductIdAsync(int productId);

    }
}
