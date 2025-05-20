using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.CartDtos;
using Application.Dtos.ImageDtos;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Application.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<bool> AddAysnc(int productDetailId)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new Exception("User id is not valid");

                int userId = int.Parse(userIdClaim.Value);

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(productDetailId);
                if (productDetail == null)
                {
                    throw new Exception("Product detail not found");
                }

                var cart = await _unitOfWork.Carts.GetByIdAsync((productDetailId, userId));

                if (cart != null)
                {
                    cart.Quantity++;
                    await _unitOfWork.Carts.UpdateAsync(cart);
                }
                else
                {
                    cart = new Cart()
                    {
                        ProductDetailId = productDetailId,
                        UserId = userId,
                        Quantity = 1
                    };
                    await _unitOfWork.Carts.AddAsync(cart);
                }

                var result = await _unitOfWork.SaveChangeAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ChangeQuantityAysnc(int productDetailId, int quantity)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new Exception("User id is not valid");

                int userId = int.Parse(userIdClaim.Value);

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(productDetailId);
                if (productDetail == null)
                {
                    throw new Exception("Product detail not found");
                }

                var cart = await _unitOfWork.Carts.GetByIdAsync((productDetailId, userId));
                if (cart == null)
                {
                    throw new Exception("Cart not found");
                }

                if (cart != null)
                {
                    cart.Quantity = quantity;
                    await _unitOfWork.Carts.UpdateAsync(cart);
                }

                var result = await _unitOfWork.SaveChangeAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CartDto>> GetAysnc()
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                   ?? throw new Exception("User id is not valid");

                int userId = int.Parse(userIdClaim.Value);

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var carts = await _unitOfWork.Carts.GetAll()
                    .Include(x => x.ProductDetail)
                    .ThenInclude(x => x.Image)
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
                var cartDtos = carts.Select(x => new CartDto()
                {
                    ProductDetailId = x.ProductDetailId,
                    ProductName = x.ProductDetail.Name,
                    Image = _mapper.Map<ImageFileDto>(x.ProductDetail.Image),
                    Price = x.ProductDetail.Price ?? 0,
                    Quantity = x.Quantity,
                    AvailableQuantity = x.ProductDetail.Quantity ?? 0

                });
                return cartDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveAysnc(int productDetailId)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new Exception("User id is not valid");

                int userId = int.Parse(userIdClaim.Value);

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var cart = await _unitOfWork.Carts.GetByIdAsync((productDetailId, userId));

                if (cart != null)
                {
                    cart.Quantity++;
                    await _unitOfWork.Carts.DeleteAsync(cart);
                    await _unitOfWork.SaveChangeAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}
