using System.Linq;
using Domain.Enity;

namespace WebMvc.Models
{
    public static class CustomerApiString
    {
        private static IConfiguration _configuration;

        public static string BASE_URL { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            BASE_URL = _configuration["ApiServerIp"] + "/api";
        }

        //product
        public static string PRODUCT() => BASE_URL + "/ProductApi";
        public static string PRODUCT_IMAGES(int id) => BASE_URL + "/ProductApi/images/" + id;

        //product detail
        public static string PRODUCT_DETAIL() => BASE_URL + "/ProductDetailApi";
        public static string PRODUCT_DETAIL(int id) => PRODUCT_DETAIL() + "/" + id;
        public static string OTHER_PRODUCT_DETAIL_PRODUCT(int productId) => PRODUCT_DETAIL() + "/product/other/" + productId;
        public static string PRODUCT_DETAIL_FILTER(string categoryName, Dictionary<string, string> queryParams)
        {
            string queryString = string.Empty;
            if (queryParams != null)
            {
                queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }
            return PRODUCT_DETAIL() + "/category/" + categoryName + "?" + queryString;
        }
        public static string PRODUCT_DETAIL_CHECKOUT(IEnumerable<int> productDetailIds)
        {
            string queryString = string.Empty;
            if (productDetailIds != null)
            {
                queryString = string.Join("&", productDetailIds.Select(kvp => $"productDetailIds={kvp}"));
            }
            return PRODUCT_DETAIL() + "/checkout?" + queryString;
        }

        //category
        public static string CATEGORY() => BASE_URL + "/CategoryApi";


        //brand
        public static string BRAND() => BASE_URL + "/BrandApi";

        //filermenu
        public static string Filtermenu(string categoryName) => BASE_URL + "/ProductAttributeApi/filtermenu/" + categoryName;
        public static string CurrentFilter(string categoryName, Dictionary<string, string> queryParams)
        {
            string queryString = string.Empty;
            if (queryParams != null)
            {
                queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }
            return BASE_URL + "/ProductAttributeApi/currentfilter/" + categoryName + "?" + queryString;
        }

        //cart
        public static string CART() => BASE_URL + "/CartApi";
        public static string CART_ADD(int productDetailId) => BASE_URL + "/CartApi/add/" + productDetailId;
        public static string CART_REMOVE(int productDetailId) => BASE_URL + "/CartApi/remove/" + productDetailId;
        public static string CART_CHANGE_QUANTITY(int productDetailId, int quantity) => BASE_URL + "/CartApi/change-quantity/" + productDetailId + "?quantity=" + quantity;

        // address
        public static string ADDRESS_PROVINCE() => BASE_URL + "/Addressapi/province";
        public static string ADDRESS_DISTRICT(string provinceId) => BASE_URL + "/Addressapi/district?provinceId=" + provinceId;
        public static string ADDRESS_WARD(string districtId) => BASE_URL + "/Addressapi/ward?districtId=" + districtId;
    

    }
}
