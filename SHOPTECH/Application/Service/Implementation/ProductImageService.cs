using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductImage;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Domain.Entity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class ProductImageService : IProductImageService
    {
        private readonly IImageFileService _imageFileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductImageService(IImageFileService imageFileService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _imageFileService = imageFileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(List<IFormFile> files, int productId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                foreach (var file in files)
                {
                    var image = await _imageFileService.UploadFileAsync(file);
                    if (image == null)
                    {
                        throw new Exception("Image upload failed.");
                    }

                    var productImage = new ProductImage
                    {
                        ProductId = productId,
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

        public async Task<IEnumerable<ProductImageDto>> GetByProductIdAsync(int productId)
        {
            var ProductImages = await _unitOfWork.ProductImages.GetAll()
                .Where(x => x.ProductId == productId)
                .Select(x => new ProductImageDto
                {
                    ProductId = x.ProductId,
                    ImageFileId = x.ImageFileId,
                    Description = x.Description,
                    Image = _mapper.Map<ImageFileDto>(x.ImageFile)
                }).ToListAsync();
            return ProductImages;
        }

        public async Task<bool> DeleteAsync(ProductImageDto productImageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Tìm ProductImage theo ProductId và ImageFileId
                var productImage = await _unitOfWork.ProductImages.GetAll()
                    .FirstOrDefaultAsync(x => x.ProductId == productImageDto.ProductId && x.ImageFileId == productImageDto.ImageFileId);

                if (productImage == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                // Xóa ProductImage
                await _unitOfWork.ProductImages.DeleteAsync(productImage);

                // Xóa file ảnh nếu cần
                await _imageFileService.DeleteFileAsync(productImage.ImageFileId);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UPdateAsync(ProductImageDto productImageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Tìm ProductImage theo ProductId và ImageFileId
                var productImage = await _unitOfWork.ProductImages.GetAll()
                    .Include(x => x.ImageFile)
                    .FirstOrDefaultAsync(x => x.ProductId == productImageDto.ProductId && x.ImageFileId == productImageDto.ImageFileId);

                if (productImage == null)
                {
                    throw new Exception("Không tìm thấy productImage");
                }

                // Cập nhật mô tả
                productImage.Description = productImageDto.Description;

                await _unitOfWork.ProductImages.UpdateAsync(productImage);

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

        public async Task<IEnumerable<ProductImageDto>> GetAllAsync()
        {
            var productImages = await _unitOfWork.ProductImages.GetAll()
                .Select(x => new ProductImageDto
                {
                    ProductId = x.ProductId,
                    ImageFileId = x.ImageFileId,
                    Description = x.Description,
                    Image = _mapper.Map<ImageFileDto>(x.ImageFile)
                }).ToListAsync();
            return productImages;
        }
    }
}
