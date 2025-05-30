using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ProductImage;
using Application.Service.Interface;
using Domain.Enity;
using Domain.Entity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Service.Implementation
{
    public class ProductImageService : IProductImageService
    {
        private readonly IImageFileService _imageFileService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductImageService(IImageFileService imageFileService, IUnitOfWork unitOfWork)
        {
            _imageFileService = imageFileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(ProductImageDto[] productImageDtos)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                foreach (var productImageDto in productImageDtos)
                {
                    var image = await _imageFileService.UploadFileAsync(productImageDto.formFile);
                    if (image == null)
                    {
                        throw new Exception("Image upload failed.");
                    }

                    var productImage = new ProductImage
                    {
                        ProductId = productImageDto.ProductId,
                        ImageFileId = image.Id,
                        Description = "Uploaded image"
                    };

                    await _unitOfWork.ProductImages.AddAsync(productImage);
                }
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public Task<bool> GetAllAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(ProductImageDto productImageDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UPdateAsync(ProductImageDto productImageDto)
        {
            throw new NotImplementedException();
        }
    }
}
