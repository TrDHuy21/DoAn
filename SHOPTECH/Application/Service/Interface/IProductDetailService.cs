using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.BrandDtos;
using Application.Dtos;
using Domain.Enity;
using Application.Dtos.ProductDetailDtos;

namespace Application.Service.Interface
{
    public interface IProductDetailService
    {
        Task<ProductDetail?> AddAsync(AddProductDetailDto brandDto);
        Task<ProductDetail?> UpdateAsync(UpdateProductDetailDto brandDto);
        Task<bool> DeleteAsync(int id);
        Task<ProductDetail?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDetail>?> GetAllAsync();
        Task<PageResultDto<IndexProductDetailDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<PageResultDto<IndexProductDetailDto>?> GetPageResultWithFilterAsync(string categoryName, int pageIndex, int pageSize, Dictionary<string, string> queryParams);
        Task<ProductDetail?> ChangeActive(int id, bool isActive);
        Task<IEnumerable<ProductDetail>?> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductDetail>?> GetWithFilterAsync(string categoryName, Dictionary<string, string> queryParams);
        Task<IEnumerable<ProductDetail>> GetOtherByProductIdAsync(int productDetailId);
        Task<IEnumerable<ProductDetail>> GetCheckout(IEnumerable<int> productDetailIds);
        Task<IEnumerable<ProductDetail>> GetByName(string str);
    }
}
