using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.AddressDtos;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class AddressService : IAddressService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProvinceDto>> GetAllProvinceAsync()
        {
            IEnumerable<ProvinceDto> result;
            try
            {
                var provinces = await _unitOfWork.Provinces.GetAll().ToListAsync();
                if (provinces == null || !provinces.Any())
                {
                    throw new Exception("No provinces found");
                }
                result = _mapper.Map<IEnumerable<ProvinceDto>>(provinces);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all provinces", ex);
            }

            return result;
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictByProvinceIdAsync(string provinceId)
        {
            IEnumerable<DistrictDto> result;
            try
            {
                var districts = await _unitOfWork.Districts.FindAsync(d => d.ProvinceId.Equals(provinceId) );
                result = _mapper.Map<IEnumerable<DistrictDto>>(districts);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all provinces", ex);
            }

            return result;
        }

        public async Task<IEnumerable<WardDto>> GetWardByDistrictIdAsync(string districtId)
        {
            IEnumerable<WardDto> result;
            try
            {
                var wards = await _unitOfWork.Wards.FindAsync(d => d.DistrictId.Equals(districtId));
                result = _mapper.Map<IEnumerable<WardDto>>(wards);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all provinces", ex);
            }

            return result;
        }
    }
}
