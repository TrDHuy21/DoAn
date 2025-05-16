using Application.Dtos.BrandDtos;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using Application.Dtos.ProductDetailDtos;

namespace WebMvc.ViewComponents
{
    [ViewComponent(Name = "CardProductDetail")]
    public class CardProductDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IndexProductDetailDto productDetail)
        {
            ViewData["productDetail"] = productDetail;
            return View();
        }
    }
}
