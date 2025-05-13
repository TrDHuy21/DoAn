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

        //product detail
        public static string PRODUCT_DETAIL() => BASE_URL + "/ProductDetailApi";
        public static string PRODUCT_DETAIL(int id) => PRODUCT_DETAIL() + "/" + id;
        public static string PRODUCT_DETAIL_PRODUCT(int productId) => PRODUCT_DETAIL() + "/product/" + productId;
        public static string PRODUCT_DETAIL_FILTER(string categoryName, Dictionary<string, string> queryParams)
        {
            string queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            return PRODUCT_DETAIL() + "/category/" + categoryName + "?" + queryString;
        }

        //category
        public static string CATEGORY() => BASE_URL + "/CategoryApi";
    }
}
