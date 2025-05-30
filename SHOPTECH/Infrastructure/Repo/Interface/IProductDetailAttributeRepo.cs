﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Infrastructure.Repo.Interface
{
    public interface IProductDetailAttributeRepo : IGenericRepo<ProductDetailAttribute, (int , int)>
    {
        Task<IEnumerable<ProductDetailAttribute>> GetByProductDetailIdAndProductAttributeId((int, int) id);
        Task<IEnumerable<ProductDetailAttribute>> GetByProductAttributeId(int ProductAttributeId);
    }
}
