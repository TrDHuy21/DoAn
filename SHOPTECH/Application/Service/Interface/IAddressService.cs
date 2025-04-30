using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.AddressDtos;

namespace Application.Service.Interface
{
    public interface IAddressService
    {
        public Task<IEnumerable<ProvinceDto>> GetAllProvinceAsync();
        public Task<IEnumerable<DistrictDto>> GetDistrictByProvinceIdAsync(int provinceId);
        public Task<IEnumerable<WardDto>> GetWardByDistrictIdAsync(int districtId);
    }
}
