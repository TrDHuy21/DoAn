using Application.Dtos.AddressDtos;
using Domain.Enity;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    [Route("Address")]
    public class AddressController : Controller
    {
        private readonly HttpClient _httpClient;

        public AddressController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Ward")]
        public async Task<IActionResult> GetWardsByDistrictId(string districtId)
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.ADDRESS_WARD(districtId));
                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                var wards = await response.Content.ReadFromJsonAsync<IEnumerable<WardDto>>()
                    ?? throw new Exception();
                return Ok(wards);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("District")]
        public async Task<IActionResult> GetDistrictsByProvinceId(string provinceId)
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.ADDRESS_DISTRICT(provinceId));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                var districts = await response.Content.ReadFromJsonAsync<IEnumerable<DistrictDto>>()
                    ?? throw new Exception();
                return Ok(districts);
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
                Console.WriteLine(CustomerApiString.ADDRESS_PROVINCE());
                var response = await _httpClient.GetAsync(CustomerApiString.ADDRESS_PROVINCE());
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                var provinces = await response.Content.ReadFromJsonAsync<IEnumerable<ProvinceDto>>()
                    ?? throw new Exception();
                return Ok(provinces);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
