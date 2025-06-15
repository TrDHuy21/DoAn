using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Service.Interface;
using GenerativeAI;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Service.Implementation
{
    public class GeminiService : IGeminiService
    {

        private readonly IConfiguration _configuration;
        private readonly GenerativeModel _model;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManagerChatSession _managerChatSession;

        public GeminiService(IConfiguration configuration, IUnitOfWork unitOfWork, IManagerChatSession managerChatSession)
        {
            _configuration = configuration;
            var apiKey = _configuration["Gemini:ApiKey"];
            var geminiModel = _configuration["Gemini:Model"];
            _model = new GenerativeModel(apiKey, geminiModel);
            _unitOfWork = unitOfWork;
            _managerChatSession = managerChatSession;
        }

        public async Task<string> SendMessageAsync(string message, string jwt)
        {
            try
            {
                ChatSession chatSession;
                string conversationId = jwt;
                chatSession = _managerChatSession.GetChatId(conversationId);

                // Sử dụng session có sẵn hoặc tạo mới nếu không tìm thấy
                if (chatSession == null)
                {
                    chatSession = _model.StartChat();
                    _managerChatSession.SetChatId(conversationId, chatSession);
                    var firstPrompt = await GeneratePrompt();
                    await chatSession.GenerateContentAsync(firstPrompt);
                }

                var response = await chatSession.GenerateContentAsync(message);
                return response.Text;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error communicating with Gemini AI: {ex.Message}", ex);
            }
        }

        private async Task<string> GeneratePrompt()
        {
            var prompt = "hãy ghi nhớ và không trả lời tin nhắn này" +
                "Bạn là **TechBot** - trợ lý AI thông minh của cửa hàng công nghệ. Tính cách thân thiện, tư vấn nhanh gọn và chính xác.\r\n# nhiệm vụ\r\n1. TƯ VẤN SẢN PHẨM\r\n2. HỖ TRỢ KHÁCH HÀNG\r\n# Template phản hồi\r\n## đưa ra gợi ý sản phẩm\r\n[TÊN SẢN PHẨM] - [Xem chi tiết](link sản phẩm)\r\nGiá: [GIÁ] \r\nĐiểm nổi bật:\r\n- [Tính năng 1]\r\n- [Tính năng 2] \r\n- [Tính năng 3]\r\nPhù hợp với bạn vì: [LÝ DO CỤ THỂ]\r\n## KHI SO SÁNH 2-3 SẢN PHẨM:\r\n🔥 TOP 3 GỢI Ý:\r\n1️⃣ [Sản phẩm A] - [Giá]\r\n👉 [Xem chi tiết](link sản phẩm)\r\n2️⃣ [Sản phẩm B] - [Giá] \r\n👉 [Xem chi tiết](link sản phẩm)\r\n3️⃣ [Sản phẩm C] - [Giá]\r\n👉 [Xem chi tiết](link sản phẩm)\r\ncâu trúc của link sản phẩm: http://localhost:5048/ProductDetail/info/{id}\r\n# chú ý:\r\n- trả lời ngắn gọn để có thể hiện thị trong khung chat nhỏ\r\n- trả lời tin nhắn dưới dạng markdown\r\n- không trả lời các câu hỏi không liên quan\r\n- chỉ gợi ý các sản có trong shop\r\n# thông tin về cửa hàng và sản phẩm\r\n1. Tên cửa hàng: SHOP TECH\r\n2. địa chỉ: Trường đại học công nghiệp hà nội\r\n3. địa chỉ liên hệ: 0336361234, trdasc21@gmail.com\r\n4. thông tin sản phẩm trong cửa hàng";
            var productinfo = await GetProductInfo();
            return prompt + productinfo;
        }

        private async Task<string> GetProductInfo()
        {
            var rs = await _unitOfWork.ProductDetails.GetAll()
                .Include(x => x.Product)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Brand = x.Product.Brand.Name,
                    Category = x.Product.Category.Name,
                    Price = x.Price,
                    Spec = String.Join(",", x.ProductDetailAttributes.Select(x => x.ProductAttribute.Name + ":" + x.Value).ToList())
                })
                .ToListAsync();

            var productStrings = rs.Select(product =>
     $"Id: {product.Id}, Name: {product.Name}, Brand: {product.Brand}, Category: {product.Category}, Price: {product.Price}, Spec: {product.Spec}");

            return String.Join(" | ", productStrings);
        }
    }
}
