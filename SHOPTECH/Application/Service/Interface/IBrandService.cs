using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.BrandDtos;
using Domain.Enity;

namespace Application.Service.Interface
{
    public interface IBrandService
    {
        /*
            AddAsync
            UpdateAsync
            DeleteAsync
            GetByIdAsync
            GetAllAsync
         */
        Task<Brand?> AddAsync(AddBrandDto brandDto);
        Task<Brand?> UpdateAsync(UpdateBrandDto brandDto);
        Task<bool> DeleteAsync(int id);
        Task<Brand?> GetByIdAsync(int id);
        Task<IEnumerable<Brand>?> GetAllAsync();
        Task<PageResultDto<IndexBrandDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<Brand?> ChangeActive(int id, bool isActive);
    }
}
