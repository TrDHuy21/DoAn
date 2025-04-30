using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Service.Interface;
using Domain.Enity;

namespace Application.Service.Implementation
{
    public  class BaseEntityService<T> () where T : BaseEntity
    {
        public static void Add(T entity)
        {
            //entity.CreatedAt = DateTime.UtcNow;
            //entity.UpdatedAt = DateTime.UtcNow;
            //// get id from jwt in requets
            //int? id = null;
            //entity.CreatedBy = id;
            //entity.UpdatedBy = id;

        }
        public static void Update(T entity)
        {
            //entity.UpdatedAt = DateTime.UtcNow;
            //// get id from jwt in requets
            //int? id = null;
            //entity.UpdatedBy = id;
        }
    }
}
