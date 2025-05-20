using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.BrandDtos;
using Application.Dtos.CategoryDtos;
using Application.Helper;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageFileService _imageFileService;
        private readonly IProductAttributeService _productAttributeService;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IImageFileService imageFileService, IProductAttributeService productAttributeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageFileService = imageFileService;
            _productAttributeService = productAttributeService;
        }

        public async Task<Category?> AddAsync(AddCategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    throw new ArgumentNullException(nameof(categoryDto), "BrandDto cannot be null");
                }

                await _unitOfWork.BeginTransactionAsync();

                ImageFile? imageFile = null;
                if (categoryDto.FormFile != null)
                {
                    imageFile = await _imageFileService.UploadFileAsync(categoryDto.FormFile);
                }

                var category = _mapper.Map<Category>(categoryDto);
                if (imageFile != null)
                {
                    category.ImageId = imageFile.Id;
                }

                BaseEntityService<Category>.Add(category);
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return category;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error adding category: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public async Task<Category?> ChangeActiveAsync(int id, bool isActive)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var category = await _unitOfWork.Categories
                    .GetAll()
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (category == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {id} not found");
                }

                category.IsActive = isActive;
                foreach(var p in category.Products ?? new List<Product>())
                {
                    p.IsActive = isActive;
                    await _unitOfWork.LoadCollectionAsync(p, p => p.ProductDetails);
                    foreach(var pd in p.ProductDetails ?? new List<ProductDetail>())
                    {
                        pd.IsActive = isActive;
                    }
                }
                
                // Cập nhật brand
                BaseEntityService<Category>.Update(category);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return category;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error deactivating category: {ex.Message}");
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.Categories.DeleteAsync(id);

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

        public async Task<IEnumerable<Category>?> GetAllAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAll()
               .Include(x => x.Image)
               .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);

                if (category == null)
                {
                    throw new Exception("Category not found");
                }

                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public async Task<PageResultDto<IndexCategoryDto>?> GetPageResultAsync(int pageIndex, int pageSize)
        {
            try
            {
                var pageResultDto = await _unitOfWork.Categories.GetAll()
                 .Include(x => x.Image)
                 .ToPagedResultAsync(
                    pageIndex,
                    pageSize,
                    categories => _mapper.Map<List<IndexCategoryDto>>(categories)
                );
                return pageResultDto;
            }
            catch
            {
                throw new Exception("Error");
            }
        }

        public async Task<Category?> UpdateAsync(UpdateCategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    throw new ArgumentNullException(nameof(categoryDto), "categoryDto cannot be null");
                }

                // Bắt đầu transaction
                await _unitOfWork.BeginTransactionAsync();

                // Tìm category cần cập nhật
                var category = await GetByIdAsync(categoryDto.Id);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found");
                }

                // Cập nhật thông tin brand từ DTO
                _mapper.Map(categoryDto, category);

                // Xử lý upload file mới nếu có
                if (categoryDto.FormFile != null)
                {
                    // Upload file mới
                    var imageFile = await _imageFileService.UploadFileAsync(categoryDto.FormFile);

                    // Xóa file cũ nếu có
                    if (category.ImageId.HasValue)
                    {
                        await _imageFileService.DeleteFileAsync(category.ImageId.Value);
                    }

                    category.ImageId = imageFile.Id;
                }



                BaseEntityService<Category>.Update(category); // Cập nhật các thuộc tính cơ bản như UpdatedAt, UpdatedBy
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return category;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error updating category: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;

            }
        }
    }
}
