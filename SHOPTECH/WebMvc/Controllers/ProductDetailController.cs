using Application.Dtos.ImageDtos;
using Application.Dtos.ProductDetailDtos;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductDetailController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ProductDetail/{categoryName}")]
        public async Task<IActionResult> ListProductDetailWithCategory([FromRoute]string categoryName, [FromQuery]Dictionary<string, string> queryParams = null)
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.PRODUCT_DETAIL_FILTER(categoryName, queryParams));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to load product details.");
                }
                var productDetails = await response.Content.ReadFromJsonAsync<IEnumerable<IndexProductDetailDto>>()
                    ?? throw new Exception("Failed to load product details.");
                ViewData["ProductDetails"] = productDetails;
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                Console.WriteLine(ex.Message);
                
            }
            ViewData["CategoryName"] = categoryName;
            ViewData["QueryParams"] = queryParams;
            return View();
        }

        [HttpGet("ProductDetail/info/{id}")]
        public async Task<IActionResult> Info(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(CustomerApiString.PRODUCT_DETAIL(id));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to load product details.");
                }
                var productDetail = await response.Content.ReadFromJsonAsync<DetailClientProductDetail>();
                ViewData["productDetail"] = productDetail;
                ViewData["categoryName"] = productDetail?.CategoryUrlName;

                var imageResponse = await _httpClient.GetAsync(CustomerApiString.PRODUCT_IMAGES(productDetail.ProductId));
                if (!imageResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to load product details.");
                }
                var images = await imageResponse.Content.ReadFromJsonAsync<IEnumerable<ImageFileDto>>();
                ViewData["images"] = images;

                Console.WriteLine(CustomerApiString.OTHER_PRODUCT_DETAIL_PRODUCT(id));
                var otherProductDetailResponse = await _httpClient.GetAsync(CustomerApiString.OTHER_PRODUCT_DETAIL_PRODUCT(id));
                if (otherProductDetailResponse.IsSuccessStatusCode)
                {
                    var otherProductDetails = await otherProductDetailResponse.Content.ReadFromJsonAsync<IEnumerable<IndexProductDetailDto>>();
                    ViewData["otherProductDetails"] = otherProductDetails;
                }

            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                Console.WriteLine(ex.Message);

            }
            return View();
        }
    }
}
