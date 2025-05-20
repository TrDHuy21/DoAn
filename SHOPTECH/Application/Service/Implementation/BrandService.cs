using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.BrandDtos;
using Application.Helper;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageFileService _imageFileService;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IImageFileService imageFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageFileService = imageFileService;
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            try
            {
                var brand = await _unitOfWork.Brands.GetByIdAsync(id);

                if (brand == null)
                {
                    throw new NullReferenceException("Brand not found");
                }

                return brand;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
        public async Task<IEnumerable<Brand>?> GetAllAsync()
        {
            try
            {
                var brands = await _unitOfWork.Brands.GetAll()
                .Include(x => x.Image)
                .ToListAsync();

                if (brands == null || brands.Count == 0)
                {
                    throw new NullReferenceException("No brands found");
                }

                return brands;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<PageResultDto<IndexBrandDto>?> GetPageResultAsync(int pageIndex, int pageSize)
        {
            try
            {
                var pageResultDto = await _unitOfWork.Brands.GetAll()
                 .Include(x => x.Image)
                 .ToPagedResultAsync(
                    pageIndex,
                    pageSize,
                    brands => _mapper.Map<List<IndexBrandDto>>(brands)
                );
                return pageResultDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
        }
        public async Task<Brand?> AddAsync(AddBrandDto brandDto)
        {
            try
            {
                if (brandDto == null)
                {
                    throw new ArgumentNullException(nameof(brandDto), "BrandDto cannot be null");
                }

                await _unitOfWork.BeginTransactionAsync();

                ImageFile? imageFile = null;
                if (brandDto.FormFile != null)
                {
                    imageFile = await _imageFileService.UploadFileAsync(brandDto.FormFile);
                }

                var brand = _mapper.Map<Brand>(brandDto);
                if (imageFile != null)
                {
                    brand.ImageId = imageFile.Id;
                }
                BaseEntityService<Brand>.Add(brand);
                await _unitOfWork.Brands.AddAsync(brand);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return brand;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error adding brand: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }

        }
        public async Task<Brand?> UpdateAsync(UpdateBrandDto brandDto)
        {
            try
            {
                if (brandDto == null)
                {
                    throw new ArgumentNullException(nameof(brandDto), "BrandDto cannot be null");
                }

                // Bắt đầu transaction
                await _unitOfWork.BeginTransactionAsync();

                // Tìm brand cần cập nhật
                var brand = await GetByIdAsync(brandDto.Id);
                if (brand == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {brandDto.Id} not found");
                }

                // Cập nhật thông tin brand từ DTO
                _mapper.Map(brandDto, brand);

                // Xử lý upload file mới nếu có
                if (brandDto.FormFile != null)
                {
                    // Upload file mới
                    var imageFile = await _imageFileService.UploadFileAsync(brandDto.FormFile);

                    // Xóa file cũ nếu có
                    if (brand.ImageId.HasValue)
                    {
                        await _imageFileService.DeleteFileAsync(brand.ImageId.Value);
                    }

                    brand.ImageId = imageFile.Id;
                }

                BaseEntityService<Brand>.Update(brand); // Cập nhật các thuộc tính cơ bản như UpdatedAt, UpdatedBy
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return brand;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error updating brand: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.Brands.DeleteAsync(id);

                var result = await _unitOfWork.SaveChangeAsync() > 0;
                await _unitOfWork.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error deactivating brand: {ex.Message}");
                throw new Exception("Error deactivating brand", ex);
            }
        }

        public async Task<Brand?> ChangeActive(int id, bool isActive)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Tìm brand cần vô hiệu hóa
                var brand = await _unitOfWork.Brands
                    .GetAll()
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (brand == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {id} not found");
                }

                // Đánh dấu brand không hoạt động thay vì xóa hoàn toàn
                brand.IsActive = isActive;
                foreach (var p in brand.Products)
                {
                    p.IsActive = isActive;
                    await _unitOfWork.LoadCollectionAsync(p, p => p.ProductDetails);
                    foreach (var pd in p.ProductDetails ?? new List<ProductDetail>())
                    {
                        pd.IsActive = isActive;
                    }
                }

                // Cập nhật brand
                BaseEntityService<Brand>.Update(brand);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return brand;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error deactivating brand: {ex.Message}");
                throw ex;
            }

        }
    }
}
