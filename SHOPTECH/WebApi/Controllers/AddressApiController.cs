using Application.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressApiController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressApiController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("Ward")]
        public async Task<IActionResult> GetWardsByDistrictId([FromQuery] string districtId)
        {
            try
            {
                var Wards = await _addressService.GetWardByDistrictIdAsync(districtId);
                return Ok(Wards);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("District")]
        public async Task<IActionResult> GetDistrictsByProvinceId([FromQuery] string provinceId)
        {
            try
            {
                var district = await _addressService.GetDistrictByProvinceIdAsync(provinceId);
                return Ok(district);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Province")]
        public async Task<IActionResult> GetProvince()
        {
            try
            {
                var province = await _addressService.GetAllProvinceAsync();
                return Ok(province);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
