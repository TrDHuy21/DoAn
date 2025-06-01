using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductImage;
using Microsoft.AspNetCore.Http;

namespace Application.Service.Interface
{
    public interface IProductImageService
    {
        Task<bool> AddAsync(List<IFormFile> files, int productId);
        Task<bool> UPdateAsync(ProductImageDto productImageDto);
        Task<bool> DeleteAsync(ProductImageDto productImageDto);
        Task<IEnumerable<ProductImageDto>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductImageDto>> GetAllAsync();
    }
}
