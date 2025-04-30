using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Models
{
    public static class CommonUrl
    {

        private static IConfiguration _configuration;

        public static string BASE_URL { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            BASE_URL = _configuration["API"];
        }

        // Base URL for the API
        //get from appsettings.json

        //login
        public static string LOGIN_URL = BASE_URL + "/login";

        // brand admin 
        public static string BRAND_ADMIN() => BASE_URL + "/BrandAdminApi";
        public static string BRAND_ADMIN(int id) => BRAND_ADMIN() + "/" + id;
        public static string BRAND_ADMIN_PAGE(int pageIndex, int pageSize) => BRAND_ADMIN() + $"/page/{pageIndex}?pagesize={pageSize}";
        internal static string BRAND_ADMIN_CHANGE_ACTIVE(int id, bool active) => BRAND_ADMIN() + $"/changeactive/{id}?isactive={active}";

        // category admin
        public static string CATEGORY_ADMIN() => BASE_URL + "/CategoryAdminApi";
        public static string CATEGORY_ADMIN(int id) => CATEGORY_ADMIN() + "/" + id;
        public static string CATEGORY_ADMIN_PAGE(int pageIndex, int pageSize) => CATEGORY_ADMIN() + $"/page/{pageIndex}?pagesize={pageSize}";
        internal static string CATEGORY_ADMIN_CHANGE_ACTIVE(int id, bool active) => CATEGORY_ADMIN() + $"/changeactive/{id}?isactive={active}";

        // Product Attribute admin

    }
}
