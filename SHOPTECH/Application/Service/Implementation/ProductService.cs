using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDtos;
using Application.Helper;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class ProductService : IProductService
    {
        //inject unitofwork and mapper
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageFileService _imageFileService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IImageFileService imageFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageFileService = imageFileService;
        }

        public async Task<Product?> AddAsync(AddProductDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    throw new ArgumentNullException(nameof(productDto), "BrandDto cannot be null");
                }
                var category = await _unitOfWork.Categories.GetByIdAsync(productDto.CategoryId);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {productDto.CategoryId} not found");
                }

                var brand = await _unitOfWork.Brands.GetByIdAsync(productDto.BrandId);
                if (brand == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {productDto.CategoryId} not found");
                }


                await _unitOfWork.BeginTransactionAsync();

                ImageFile? imageFile = null;
                if (productDto.FormFile != null)
                {
                    imageFile = await _imageFileService.UploadFileAsync(productDto.FormFile);
                }

                var product = _mapper.Map<Product>(productDto);
                if (imageFile != null)
                {
                    product.MainImageId = imageFile.Id;
                }

                BaseEntityService<Product>.Add(product);
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return product;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error adding product: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public async Task<Product?> ChangeActiveAsync(int id, bool isActive)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Tìm brand cần vô hiệu hóa
                var product = await _unitOfWork.Products
                    .GetAll()
                    .Include(b => b.ProductDetails)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {id} not found");
                }

                product.IsActive = isActive;

                foreach (var productdetail in product.ProductDetails)
                {
                    productdetail.IsActive = isActive;
                }


                // Cập nhật brand
                BaseEntityService<Product>.Update(product);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return product;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error deactivating product: {ex.Message}");
                throw ex;
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>?> GetAllAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetAll()
                    .Include(x => x.MainImage)
                    .Include(x => x.Category)
                    .Include(x => x.Brand)
                    .ToListAsync();
                if (products.Count <= 0)
                {
                    throw new Exception("Product list is empty");
                }

                return products;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                _unitOfWork.LoadReference<Product, Category>(product, product => product.Category);
                _unitOfWork.LoadReference<Product, Brand>(product, product => product.Brand);
                _unitOfWork.LoadReference<Product, ImageFile>(product, product => product.MainImage);
                if (product == null)
                {
                    throw new Exception("Not found");
                }
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ImageFileDto>> GetImages(int id)
        {
            // get mainimage, Product.Images
            // image of productdetail

            try
            {
                var listImage = new List<ImageFileDto>();
                var product = await _unitOfWork.Products.GetAll()
                                    .Include(x => x.MainImage)
                                    .Include(x => x.ProductImages)
                                    .Include(x => x.ProductDetails)
                                        .ThenInclude(x => x.Image)
                                    .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
                if (product == null)
                {
                    throw new Exception("Not found");
                }

                if (product.MainImage != null)
                {
                    listImage.Add(_mapper.Map<ImageFileDto>(product.MainImage));
                }

                if (product.ProductImages != null)
                {
                    foreach (var item in product.ProductImages)
                    {
                        listImage.Add(_mapper.Map<ImageFileDto>(item));
                    }
                }

                if (product.ProductDetails != null)
                {
                    foreach (var item in product.ProductDetails)
                    {
                        if (item.Image != null)
                        {
                            listImage.Add(_mapper.Map<ImageFileDto>(item.Image));
                        }
                    }
                }

                return listImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PageResultDto<IndexProductDto>?> GetPageResultAsync(int pageIndex, int pageSize)
        {
            try
            {
                var pageResultDto = await _unitOfWork.Products.GetAll()
                 .Include(x => x.MainImage)
                 .Include(x => x.Brand)
                 .Include(x => x.Category)
                 .ToPagedResultAsync(
                    pageIndex,
                    pageSize,
                    products => _mapper.Map<List<IndexProductDto>>(products)
                );
                return pageResultDto;
            }
            catch
            {
                throw new Exception("Error");
            }
        }

        public async Task<Product?> UpdateAsync(UpdateProductDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    throw new ArgumentNullException(nameof(productDto), "categoryDto cannot be null");
                }
                if (productDto == null)
                {
                    throw new ArgumentNullException(nameof(productDto), "BrandDto cannot be null");
                }
                var category = await _unitOfWork.Categories.GetByIdAsync(productDto.CategoryId);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {productDto.CategoryId} not found");
                }

                var brand = await _unitOfWork.Brands.GetByIdAsync(productDto.BrandId);
                if (brand == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {productDto.CategoryId} not found");
                }

                // Bắt đầu transaction
                await _unitOfWork.BeginTransactionAsync();

                // Tìm category cần cập nhật
                var product = await GetByIdAsync(productDto.Id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Category with ID {productDto.Id} not found");
                }

                // Cập nhật thông tin brand từ DTO
                _mapper.Map(productDto, product);

                // Xử lý upload file mới nếu có
                if (productDto.FormFile != null)
                {
                    // Upload file mới
                    var imageFile = await _imageFileService.UploadFileAsync(productDto.FormFile);

                    // Xóa file cũ nếu có
                    if (product.MainImageId.HasValue)
                    {
                        await _imageFileService.DeleteFileAsync(product.MainImageId.Value);
                    }

                    product.MainImageId = imageFile.Id;
                }



                BaseEntityService<Product>.Update(product); // Cập nhật các thuộc tính cơ bản như UpdatedAt, UpdatedBy
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return product;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error updating product: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;

            }
        }
    }
}
