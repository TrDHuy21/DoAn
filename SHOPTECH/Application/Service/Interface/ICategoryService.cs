using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.BrandDtos;
using Application.Dtos;
using Domain.Enity;
using Application.Dtos.CategoryDtos;

namespace Application.Service.Interface
{
    public interface ICategoryService
    {
        Task<Category?> AddAsync(AddCategoryDto categoryDto);
        Task<Category?> UpdateAsync(UpdateCategoryDto categoryDto);
        Task<bool> DeleteAsync(int id);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>?> GetAllAsync();
        Task<PageResultDto<IndexCategoryDto>?> GetPageResultAsync(int pageIndex, int pageSize);
        Task<Category?> ChangeActiveAsync(int id, bool isActive);

    }
}
