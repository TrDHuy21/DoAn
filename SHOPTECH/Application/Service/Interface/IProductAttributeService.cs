using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.ProductAttributeDtos;
using Domain.Enity;

namespace Application.Service.Interface
{
    public interface IProductAttributeService
    {
        Task<ProductAttribute?> AddAsync(AddProductAttributeDto productAttributeDto);
        Task<ProductAttribute?> UpdateAsync(UpdateProductAttributeDto productAttributeDto);
        Task<bool> DeleteAsync(int id);
        Task<ProductAttribute?> GetByIdAsync(int id);
        Task<List<ProductAttribute>?> GetAllAsync();
        Task<ProductAttribute?> ChangeActiveAsync(int id, bool isActive);
        Task<ProductAttribute?> ChangeDisplayAsync(int id, bool isDisplay);
        Task<ProductAttribute?> ChangeFilterAsync(int id, bool canFilter);
        Task<IEnumerable<ProductAttribute>> GetByCategoryIdAsync(int categoryId);

        Task<List<FilterMenuDto>> GetFilterMenu(string categoryName);
        Task<List<FilterMenuDto>> GetCurrentFilter(string categoryName, Dictionary<string, string> queryParams);
    }
}
