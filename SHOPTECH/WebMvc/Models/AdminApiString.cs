using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Models
{
    public static class AdminApiString
    {

        private static IConfiguration _configuration;

        public static string BASE_URL { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            BASE_URL = _configuration["ApiServerIp"] + "/api";
        }

        // Base URL for the API
        //get from appsettings.json

        //login
        public static string LOGIN_URL() => BASE_URL + "/login";

        // brand admin 
        public static string BRAND_ADMIN() => BASE_URL + "/BrandAdminApi";
        public static string BRAND_ADMIN(int id) => BRAND_ADMIN() + "/" + id;
        public static string BRAND_ADMIN_PAGE(int pageIndex, int pageSize) => BRAND_ADMIN() + $"/page/{pageIndex}?pagesize={pageSize}";
        public static string BRAND_ADMIN_CHANGE_ACTIVE(int id, bool active) => BRAND_ADMIN() + $"/changeactive/{id}?isactive={active}";

        // category admin
        public static string CATEGORY_ADMIN() => BASE_URL + "/CategoryAdminApi";
        public static string CATEGORY_ADMIN(int id) => CATEGORY_ADMIN() + "/" + id;
        public static string CATEGORY_ADMIN_PAGE(int pageIndex, int pageSize) => CATEGORY_ADMIN() + $"/page/{pageIndex}?pagesize={pageSize}";
        public static string CATEGORY_ADMIN_CHANGE_ACTIVE(int id, bool active) => CATEGORY_ADMIN() + $"/changeactive/{id}?isactive={active}";

        // product attribute admin
        public static string PRODUCT_ATTRIBUTE_ADMIN() => BASE_URL + "/ProductAttributeAdminApi";
        public static string PRODUCT_ATTRIBUTE_ADMIN(int id) => PRODUCT_ATTRIBUTE_ADMIN() + "/" + id;
        public static string PRODUCT_ATTRIBUTE_ADMIN_CATEGORY(int categoryId) => PRODUCT_ATTRIBUTE_ADMIN() + "/category/" + categoryId;


        // product admin
        public static string PRODUCT_ADMIN() => BASE_URL + "/ProductAdminApi";
        public static string PRODUCT_ADMIN(int id) => PRODUCT_ADMIN() + "/" + id;
        public static string PRODUCT_ADMIN_PAGE(int pageIndex, int pageSize) => PRODUCT_ADMIN() + $"/page/{pageIndex}?pagesize={pageSize}";
        public static string PRODUCT_ADMIN_CHANGE_ACTIVE(int id, bool active) => PRODUCT_ADMIN() + $"/changeactive/{id}?isactive={active}";

        //product detail admin
        public static string PRODUCT_DETAIL_ADMIN() => BASE_URL + "/ProductDetailAdminApi";
        public static string PRODUCT_DETAIL_ADMIN(int id) => PRODUCT_DETAIL_ADMIN() + "/" + id;
        public static string PRODUCT_DETAIL_ADMIN_PRODUCT(int productId) => PRODUCT_DETAIL_ADMIN() + "/product/" + productId;
        public static string PRODUCT_DETAIL_ADMIN_PAGE(int pageIndex, int pageSize) => PRODUCT_DETAIL_ADMIN() + $"/page/{pageIndex}?pagesize={pageSize}";
        public static string PRODUCT_DETAIL_ADMIN_CHANGE_ACTIVE(int id, bool active) => PRODUCT_DETAIL_ADMIN() + $"/changeactive/{id}?isactive={active}";

    }
}
