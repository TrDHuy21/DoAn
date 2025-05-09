using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.ProductDetailDtos;
using Application.Helper;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class ProductDetailService : IProductDetailService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IImageFileService _imageFileService;

        public ProductDetailService(IUnitOfWork unitOfWork, IMapper mapper, IImageFileService imageFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageFileService = imageFileService;
        }

        public async Task<ProductDetail?> AddAsync(AddProductDetailDto productDetailDto)
        {
            try
            {
                if (productDetailDto == null)
                {
                    throw new ArgumentNullException("Branddto can not null");
                }

                var productDetail = _mapper.Map<ProductDetail>(productDetailDto);

                await _unitOfWork.BeginTransactionAsync();

                if (productDetailDto.FormFile != null)
                {
                    var image = await _imageFileService.UploadFileAsync(productDetailDto.FormFile);
                    if (image != null)
                    {
                        productDetail.ImageId = image.Id;
                    }
                }

                BaseEntityService<ProductDetail>.Add(productDetail);
                await _unitOfWork.ProductDetails.AddAsync(productDetail);
                await _unitOfWork.SaveChangeAsync();

                //add productdetail attribute
                 if (productDetailDto.ProductDetailAttributes != null)
                {
                    foreach (var productAttribute in productDetailDto.ProductDetailAttributes)
                    {
                        var productDetailAttribute = new ProductDetailAttribute()
                        {
                            ProductDetailId = productDetail.Id,
                            ProductAttributeId = productAttribute.ProductAttributeId,
                            Value = productAttribute.Value
                        };
                        await _unitOfWork.ProductDetailAttributes.AddAsync(productDetailAttribute);
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return productDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(id);
                if (productDetail == null)
                {
                    throw new Exception("Product detail not found");
                }
                await _unitOfWork.LoadCollectionAsync(productDetail, productDetail => productDetail.ProductDetailAttributes);
                foreach (var pda in productDetail?.ProductDetailAttributes ?? Enumerable.Empty<ProductDetailAttribute>())
                {
                    await _unitOfWork.ProductDetailAttributes.DeleteAsync(pda);
                }
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.ProductDetails.DeleteAsync(id);

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
        public async Task<IEnumerable<ProductDetail>?> GetAllAsync()
        {
            try
            {
                var categories = await _unitOfWork.ProductDetails.GetAll()
               .Include(x => x.Image)
               .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProductDetail?> GetByIdAsync(int id)
        {
            try
            {
                var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(id);

                if (productDetail == null)
                {
                    throw new Exception("Brand not found");
                }

                return productDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
        public async Task<PageResultDto<IndexProductDetailDto>?> GetPageResultAsync(int pageIndex, int pageSize)
        {
            try
            {
                var pageResultDto = await _unitOfWork.ProductDetails.GetAll()
                 .Include(x => x.Image)
                 .ToPagedResultAsync(
                    pageIndex,
                    pageSize,
                    categories => _mapper.Map<List<IndexProductDetailDto>>(categories)
                );
                return pageResultDto;
            }
            catch
            {
                throw new Exception("Error");
            }
        }
        public async Task<ProductDetail?> UpdateAsync(UpdateProductDetailDto productDetailDto)
        {
            try
            {
                if (productDetailDto == null)
                {
                    throw new ArgumentNullException(nameof(productDetailDto), "productDetailDto cannot be null");
                }

                var productDetail = await GetByIdAsync(productDetailDto.Id);
                if (productDetail == null)
                {
                    throw new KeyNotFoundException($"Product detail with ID {productDetailDto.Id} not found");
                }

                var product = await _unitOfWork.Products.GetByIdAsync(productDetailDto.ProductId ?? 0)
                    ?? throw new Exception("Product not found");

                // Bắt đầu transaction
                await _unitOfWork.BeginTransactionAsync();

                // Cập nhật thông tin brand từ DTO
                _mapper.Map(productDetailDto, productDetail);

                // Xử lý upload file mới nếu có
                if (productDetailDto.FormFile != null)
                {
                    // Upload file mới
                    var imageFile = await _imageFileService.UploadFileAsync(productDetailDto.FormFile);

                    // Xóa file cũ nếu có
                    if (productDetail.ImageId.HasValue)
                    {
                        await _imageFileService.DeleteFileAsync(productDetail.ImageId.Value);
                    }

                    productDetail.ImageId = imageFile.Id;
                }

                // Cập nhật các thuộc tính cơ bản như UpdatedAt, UpdatedBy
                BaseEntityService<ProductDetail>.Update(productDetail);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return productDetail;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error updating category: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;

            }
        }
        public async Task<ProductDetail?> ChangeActive(int id, bool isActive)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Tìm brand cần vô hiệu hóa
                var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(id);

                if (productDetail == null)
                {
                    throw new KeyNotFoundException($"Brand with ID {id} not found");
                }

                // Đánh dấu brand không hoạt động thay vì xóa hoàn toàn
                productDetail.IsActive = isActive;



                // Cập nhật brand
                BaseEntityService<ProductDetail>.Update(productDetail);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return productDetail;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error deactivating category: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<ProductDetail>?> GetByProductIdAsync(int productId)
        {
            try
            {
                var productDetails = await _unitOfWork.ProductDetails.GetByProductIdAsync(productId);
                if (productDetails == null)
                {
                    throw new Exception("ProductDetail not found");
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PageResultDto<IndexProductDetailDto>?> GetPageResultWithFilterAsync(string categoryName, int pageIndex, int pageSize, Dictionary<string, string> queryParams)
        {
            try
            {
                var productDetails = await GetWithFilterAsync(categoryName, queryParams);

                // Thực hiện phân trang và mapping
                var pageResultDto = productDetails.ToPagedResult(
                    pageIndex,
                    pageSize,
                    productDetails => _mapper.Map<List<IndexProductDetailDto>>(productDetails)
                );

                return pageResultDto;
            }
            catch (Exception ex)
            {
                // Nên log lỗi cụ thể thay vì throw chung chung
                throw new Exception("Error getting product details", ex);
            }
        }

        public async Task<IEnumerable<ProductDetail>?> GetWithFilterAsync(string categoryName, Dictionary<string, string> queryParams)
        {
            try
            {
                // Tạo một query ban đầu
                var query = _unitOfWork.ProductDetails.GetAll()
                    .Include(x => x.Image)
                    .Where(pd => pd.Product.Category.UrlName== categoryName);

                // In ra query gốc để debug
                Console.WriteLine("Query ban đầu: " + query.ToQueryString());

                // Xử lý lọc từ queryParams
                if (queryParams != null && queryParams.Count > 0)
                {
                    // Tạo danh sách các điều kiện lọc để kết hợp sau
                    var conditions = new List<Expression<Func<ProductDetail, bool>>>();

                    // Xử lý các điều kiện lọc thuộc tính
                    foreach (var param in queryParams)
                    {
                        string filterName = param.Key.ToLower().Trim();  // Chuẩn hóa key
                        string filterValue = param.Value;

                        // Bỏ qua các tham số đặc biệt để xử lý riêng sau
                        if (filterName == "min_price" || filterName == "max_price" ||
                            filterName == "ishot" || filterName == "isnew" ||
                            filterName == "issale" || filterName == "order" ||
                            filterName == "dir")
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(filterValue))
                        {
                            var values = filterValue.Split(',').Select(v => v.Trim()).ToList();

                            // Lọc theo từng thuộc tính
                            query = query.Where(p => p.ProductDetailAttributes.Any(
                                pda => pda.ProductAttribute.UrlName == filterName &&
                                       values.Contains(pda.Value)));

                            // In ra query sau mỗi điều kiện lọc để debug
                            Console.WriteLine($"Query sau lọc {filterName}: " + query.ToQueryString());
                        }
                    }

                    // Xử lý lọc theo giá
                    if (queryParams.TryGetValue("min_price", out var minPriceStr) &&
                        !string.IsNullOrEmpty(minPriceStr) &&
                        decimal.TryParse(minPriceStr, out var minPrice))
                    {
                        query = query.Where(p => p.Price >= minPrice);
                        Console.WriteLine("Query sau lọc min_price: " + query.ToQueryString());
                    }

                    if (queryParams.TryGetValue("max_price", out var maxPriceStr) &&
                        !string.IsNullOrEmpty(maxPriceStr) &&
                        decimal.TryParse(maxPriceStr, out var maxPrice))
                    {
                        query = query.Where(p => p.Price <= maxPrice);
                        Console.WriteLine("Query sau lọc max_price: " + query.ToQueryString());
                    }

                    // Xử lý các trường boolean
                    // Chú ý: Trong URL, các giá trị boolean thường là "true" hoặc "false" (chữ thường)
                    if (queryParams.TryGetValue("ishot", out var isHotStr) &&
                        !string.IsNullOrEmpty(isHotStr))
                    {
                        bool isHot = isHotStr.ToLower() == "true";
                        query = query.Where(p => p.isHot == isHot);
                        Console.WriteLine("Query sau lọc isHot: " + query.ToQueryString());
                    }

                    if (queryParams.TryGetValue("isnew", out var isNewStr) &&
                        !string.IsNullOrEmpty(isNewStr))
                    {
                        bool isNew = isNewStr.ToLower() == "true";
                        query = query.Where(p => p.isNew == isNew);
                        Console.WriteLine("Query sau lọc isNew: " + query.ToQueryString());
                    }

                    if (queryParams.TryGetValue("issale", out var isSaleStr) &&
                        !string.IsNullOrEmpty(isSaleStr))
                    {
                        bool isSale = isSaleStr.ToLower() == "true";
                        query = query.Where(p => p.isSale == isSale);
                        Console.WriteLine("Query sau lọc isSale: " + query.ToQueryString());
                    }

                    // Xử lý sắp xếp
                    // Sửa lỗi trong đoạn code gốc: "   " -> "order"
                    string order = string.Empty;
                    string dir = string.Empty;

                    queryParams.TryGetValue("order", out order);  // Sửa từ "   " thành "order"
                    queryParams.TryGetValue("dir", out dir);

                    if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(dir))
                    {
                        switch (order.ToLower())
                        {
                            case "name":
                                query = dir.ToLower() == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                                break;
                            case "price":
                                query = dir.ToLower() == "asc" ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                                break;
                            default:
                                // Mặc định sắp xếp theo ID để đảm bảo có kết quả ổn định
                                query = dir.ToLower() == "asc" ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
                                break;
                        }
                        Console.WriteLine("Query sau sắp xếp: " + query.ToQueryString());
                    }
                    else
                    {
                        // Mặc định sắp xếp theo ID nếu không có tham số sắp xếp
                        query = query.OrderBy(p => p.Id);
                    }
                }

                Console.WriteLine("Query cuối cùng: " + query.ToQueryString());

                // Thực thi query và trả về kết quả
                var result = await query.ToListAsync();
                Console.WriteLine($"Số lượng kết quả: {result.Count}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception("Error getting product details", ex);
            }
        }
    }
}
