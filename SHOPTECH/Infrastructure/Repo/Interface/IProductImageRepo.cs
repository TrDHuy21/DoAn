﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Infrastructure.Repo.Interface
{
    public interface IProductImageRepo : IGenericRepo<ProductImage, (int,int)>
    {
    }
}
