using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.ProductAttributeDtos;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class ProductAttributeService : IProductAttributeService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductAttributeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductAttribute?> AddAsync(AddProductAttributeDto productAttributeDto)
        {
            try
            {
                if (productAttributeDto == null)
                {
                    throw new ArgumentNullException(nameof(productAttributeDto), "Product attribute data cannot be null");
                }

                // Map DTO to entity
                var productAttribute = _mapper.Map<ProductAttribute>(productAttributeDto);

                await _unitOfWork.ProductAttributes.AddAsync(productAttribute);
                await _unitOfWork.SaveChangeAsync();

                return productAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding product attribute: {ex.Message}", ex);
            }
        }

        public async Task<ProductAttribute?> UpdateAsync(UpdateProductAttributeDto productAttributeDto)
        {
            try
            {
                if (productAttributeDto == null)
                {
                    throw new ArgumentNullException(nameof(productAttributeDto), "Product attribute data cannot be null");
                }

                var existingAttribute = await _unitOfWork.ProductAttributes.GetByIdAsync(productAttributeDto.Id);
                if (existingAttribute == null)
                {
                    throw new KeyNotFoundException($"Product attribute with ID {productAttributeDto.Id} not found");
                }

                // Update properties
                _mapper.Map(productAttributeDto, existingAttribute);

                await _unitOfWork.ProductAttributes.UpdateAsync(existingAttribute);
                await _unitOfWork.SaveChangeAsync();

                return existingAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating product attribute: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var productAttribute = await _unitOfWork.ProductAttributes.GetByIdAsync(id);
                if (productAttribute == null)
                {
                    throw new KeyNotFoundException($"Product attribute with ID {id} not found");
                }

                await _unitOfWork.ProductAttributes.DeleteAsync(productAttribute);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error deleting product attribute: {ex.Message}", ex);
            }
        }

        public async Task<ProductAttribute?> GetByIdAsync(int id)
        {
            try
            {
                var productAttribute = await _unitOfWork.ProductAttributes.GetByIdAsync(id);
                if (productAttribute == null)
                {
                    throw new KeyNotFoundException($"Product attribute with ID {id} not found");
                }

                return productAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving product attribute: {ex.Message}", ex);
            }
        }

        public async Task<List<ProductAttribute>?> GetAllAsync()
        {
            try
            {
                var productAttributes = await _unitOfWork.ProductAttributes.GetAll().ToListAsync();
                return productAttributes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving product attributes: {ex.Message}", ex);
            }
        }

        public async Task<ProductAttribute?> ChangeActiveAsync(int id, bool isActive)
        {
            try
            {
                var productAttribute = await _unitOfWork.ProductAttributes.GetByIdAsync(id);
                if (productAttribute == null)
                {
                    throw new KeyNotFoundException($"Product attribute with ID {id} not found");
                }

                productAttribute.IsActive = isActive;
                await _unitOfWork.ProductAttributes.UpdateAsync(productAttribute);
                await _unitOfWork.SaveChangeAsync();

                return productAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing active status: {ex.Message}", ex);
            }
        }

        public async Task<ProductAttribute?> ChangeDisplayAsync(int id, bool isDisplay)
        {
            try
            {
                var productAttribute = await _unitOfWork.ProductAttributes.GetByIdAsync(id);
                if (productAttribute == null)
                {
                    throw new KeyNotFoundException($"Product attribute with ID {id} not found");
                }

                productAttribute.IsDisplay = isDisplay;
                await _unitOfWork.ProductAttributes.UpdateAsync(productAttribute);
                await _unitOfWork.SaveChangeAsync();

                return productAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing display status: {ex.Message}", ex);
            }
        }

        public async Task<ProductAttribute?> ChangeFilterAsync(int id, bool canFilter)
        {
            try
            {
                var productAttribute = await _unitOfWork.ProductAttributes.GetByIdAsync(id);
                if (productAttribute == null)
                {
                    throw new KeyNotFoundException($"Product attribute with ID {id} not found");
                }

                productAttribute.CanFilter = canFilter;
                await _unitOfWork.ProductAttributes.UpdateAsync(productAttribute);
                await _unitOfWork.SaveChangeAsync();

                return productAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing filter status: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ProductAttribute>> GetByCategoryIdAsync(int categoryId)
        {
            try
            {
                var productAttribute = await _unitOfWork.ProductAttributes.GetAll()
                    .Where(x => x.CategoryId == categoryId)
                    .ToListAsync();
                if (productAttribute == null)
                {
                    throw new KeyNotFoundException("No product attributes found for this category.");
                }
                return productAttribute;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FilterMenuDto>> GetFilterMenu(string categoryName)
        {
            // Lấy tất cả ProductAttributes của category, kèm theo ProductDetailAttributes
            var filterMenus = await _unitOfWork.ProductAttributes.GetAll()
                .Include(x => x.ProductDetailAttributes)
                .Where(x => x.Category.UrlName == categoryName)
                .Select(attribute => new FilterMenuDto
                {
                    Name = attribute.Name,
                    UrlName = attribute.UrlName,
                    // Lấy tất cả giá trị không trùng lặp từ ProductDetailAttributes
                    Values = attribute.ProductDetailAttributes
                    .Select(detail => detail.Value)
                    .Distinct()
                    .ToList(),
                    UrlValues = attribute.ProductDetailAttributes
                    .Select(detail => detail.UrlValue)
                    .Distinct()
                    .ToList()
                })
                .ToListAsync();

            return filterMenus;
        }

        public async Task<List<FilterMenuDto>> GetCurrentFilter(string categoryName, Dictionary<string, string> queryParams)
        {
            if (queryParams == null)
            {
                return new List<FilterMenuDto>();
            }

            // Lấy danh sách các thuộc tính trùng khớp với category và có trong queryParams
            var query = _unitOfWork.ProductAttributes.GetAll()
                .Include(x => x.ProductDetailAttributes)
                .Where(x => x.Category.UrlName == categoryName)
                .Where(x => queryParams.Keys.Contains(x.UrlName));

            var attributes = await query.ToListAsync();

            var currentFilter = new List<FilterMenuDto>();

            // Xử lý từng thuộc tính để tạo FilterMenuDto
            foreach (var attribute in attributes)
            {
                if (queryParams.TryGetValue(attribute.UrlName, out string filterValues))
                {
                    // Tách chuỗi giá trị từ queryParams (giả sử giá trị được phân tách bằng dấu phẩy)
                    var filterValuesList = filterValues.Split(',').Select(v => v.Trim()).ToList();

                    // Tạo FilterMenuDto cho thuộc tính hiện tại
                    var filterMenu = new FilterMenuDto
                    {
                        Name = attribute.Name,
                        UrlName = attribute.UrlName,
                        Values = attribute.ProductDetailAttributes
                            .Where(detail => filterValuesList.Contains(detail.UrlValue))
                            .Select(detail => detail.Value)
                            .Distinct()
                            .ToList(),
                        UrlValues = attribute.ProductDetailAttributes
                            .Where(detail => filterValuesList.Contains(detail.UrlValue))
                            .Select(detail => detail.UrlValue)
                            .Distinct()
                            .ToList(),
                    };

                    currentFilter.Add(filterMenu);
                }
            }

            return currentFilter;
        }
    }
}
