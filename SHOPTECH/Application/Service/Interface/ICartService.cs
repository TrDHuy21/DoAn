using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CartDtos;

namespace Application.Service.Interface
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAysnc();
        Task<bool> AddAysnc(int productDetailId);
        Task RemoveAysnc(int productDetailId);
        Task<bool> ChangeQuantityAysnc(int productDetailId, int quantity);
    }
}
