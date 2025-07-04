﻿using Microsoft.AspNetCore.Mvc;

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
        public static string PRODUCT_ATTRIBUTE_ADMIN_CHANGE_ACTIVE(int categoryId, bool isActive) => PRODUCT_ATTRIBUTE_ADMIN() + "/changeactive/" + categoryId + "?isActive=" + isActive;
        public static string PRODUCT_ATTRIBUTE_ADMIN_CHANGE_DISPLAY(int categoryId, bool isDisplay) => PRODUCT_ATTRIBUTE_ADMIN() + "/changedisplay/" + categoryId + "?isDisplay=" + isDisplay;
        public static string PRODUCT_ATTRIBUTE_ADMIN_CHANGE_FILTER(int categoryId, bool canFilter) => PRODUCT_ATTRIBUTE_ADMIN() + "/changefilter/" + categoryId + "?canFilter=" + canFilter;


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

        // order
        public static string ORDER_ADMIN() => BASE_URL + "/OrderAdminApi";
        public static string ORDER_ADMIN(int id) => BASE_URL + "/OrderAdminApi/"+id;

        // user
        public static string USER_ADMIN() => BASE_URL + "/UserAdminApi";
        public static string USER_ADMIN(int id) => USER_ADMIN() + "/" + id;
        public static string USER_ADMIN_PROFILE() => USER_ADMIN() + "/profile";
        public static string USER_ADMIN_CHAGNE_ACTIVE(int id) => USER_ADMIN() + "/changeActive/"+id;

        // product image admin
        public static string PRODUCT_IMAGE_ADMIN() => BASE_URL + "/ProductImageAdminApi";
        public static string PRODUCT_IMAGE_ADMIN_GETBYPRODUCTID(int productId) => PRODUCT_IMAGE_ADMIN() + $"/getbyproductid/{productId}";
        public static string PRODUCT_IMAGE_ADMIN_ADD(int productId) => PRODUCT_IMAGE_ADMIN() + "?productId=" + productId;
        public static string PRODUCT_IMAGE_ADMIN_UPDATE() => PRODUCT_IMAGE_ADMIN();
        public static string PRODUCT_IMAGE_ADMIN_DELETE() => PRODUCT_IMAGE_ADMIN();
        public static string PRODUCT_IMAGE_ADMIN_GETALL() => PRODUCT_IMAGE_ADMIN();


        // statistics
        public static string STATISTICS_ADMIN_DOANHTHUTHANG() => BASE_URL + "/StatisticsAdminApi/doanhthuthang";

    }
}
