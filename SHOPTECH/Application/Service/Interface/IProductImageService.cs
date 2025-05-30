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
        Task<bool> AddAsync(ProductImageDto[] productImageDtos);
        Task<bool> UPdateAsync(ProductImageDto productImageDto);
        Task<bool> RemoveAsync(ProductImageDto productImageDto);
        Task<bool> GetAllAsync(int productId);
    }
}
