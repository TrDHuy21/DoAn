using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductDtos;
using Application.Dtos;
using Domain.Enity;
using Application.Dtos.ImageDtos;

namespace Application.Service.Interface
{
    public interface IProductService
    {
        Task<Product?> AddAsync(AddProductDto productDto);
        Task<Product?> UpdateAsync(UpdateProductDto productDto);
        Task<bool> DeleteAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>?> GetAllAsync();
        Task<PageResultDto<IndexProductDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<Product?> ChangeActiveAsync(int id, bool isActive);
        Task<List<ImageFileDto>> GetImages(int id);
    }
}
