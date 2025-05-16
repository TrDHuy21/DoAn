using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Service.Interface;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class ProductDetailAttributeService : IProductDetailAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductDetailAttributeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDetailAttribute>> GetAllAsync()
        {
            var productDetailAttributes = await _unitOfWork.ProductDetailAttributes
                 .GetAll().ToListAsync();
            return productDetailAttributes;
        }

        public async Task<ProductDetailAttribute> GetByProductDetailIdAndProductAttributeId(int productDetailId, int productAttributeId)
        {
            var productDetailAttribute = await _unitOfWork.ProductDetailAttributes
                .GetByIdAsync((productDetailId, productAttributeId));

            return productDetailAttribute;
        }
    }
}
