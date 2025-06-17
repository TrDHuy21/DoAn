using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Dtos.UserDtos;
using Application.Helper;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.Implementation
{
    public class UserService : IUserService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IImageFileService _imageFileService;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IPasswordService _passwordService;

        public UserService(IUnitOfWork unitOfWork, IImageFileService imageFileService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _imageFileService = imageFileService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _passwordService = passwordService;
        }

        public async Task<User?> AddAsync(AddUserDto userDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                ImageFile? imageFile = null;
                if (userDto.FormFile != null)
                {
                    imageFile = await _imageFileService.UploadFileAsync(userDto.FormFile);
                }

                var user = _mapper.Map<User>(userDto);
                if (imageFile != null)
                {
                    user.ImageId = imageFile.Id;
                }
                user.UrlName = "";
                BaseEntityService<User>.Add(user);
                await _unitOfWork.Users.AddAsync(user);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return user;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error adding category: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public async Task<User> ChangeActive(int id, bool isActive)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id) ??
                throw new Exception("Không thể lấy user với id: " + id);
            user.IsActive = isActive;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangeAsync();
            return user;
        }

        public async Task<User?> ChangeActiveAsync(int id, bool isActive)
        {
            //get user by id
            // update user isActive property
            //savechange
            //return usser
            var user = await _unitOfWork.Users.GetByIdAsync(id) ??
                throw new Exception("Không thể lấy user với id: " + id);
            user.IsActive = isActive;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangeAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            bool result = await _unitOfWork.SaveChangeAsync() > 0;
            return result;
        }

        public async Task<IEnumerable<User>?> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAll()
                .Include(x => x.Role)
                .ToListAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var users = await _unitOfWork.Users.GetByIdAsync(id);
            return users;
        }

        public async Task<User> GetMyProfile()
        {
            var userCurrent = _httpContextAccessor.HttpContext?.User;
            int userId = Convert.ToInt32(userCurrent?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("Lỗi trong quá trình lấy thông tin người dùng"));

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            return user;
        }

        public async Task<PageResultDto<IndexUserDto>?> GetPageResultAsync(int pageIndex, int pageSize)
        {
            try
            {
                var pageResultDto = await _unitOfWork.Users.GetAll()
                 .Include(x => x.Image)
                 .ToPagedResultAsync(
                    pageIndex,
                    pageSize,
                    categories => _mapper.Map<List<IndexUserDto>>(categories)
                );
                return pageResultDto;
            }
            catch
            {
                throw new Exception("Error");
            }
        }

        public async Task<bool> RegisterUserAsync(RegisterUser registerUser)
        {
            try
            {
                //check username exsit
                var userExist = await _unitOfWork.Users.GetAll()
                    .FirstOrDefaultAsync(x => x.Username == registerUser.Phone);
                if (userExist != null)
                {
                    throw new Exception("Số điện thoại này đã được đăng ký");
                }

                var user = _mapper.Map<User>(registerUser);
                user.UrlName = "";
                user.IsActive = true; // Mặc định người dùng mới được kích hoạt
                user.HashedPassword = _passwordService.HashPassword(registerUser.Password);
                BaseEntityService<User>.Add(user);
                await _unitOfWork.Users.AddAsync(user);
                int rs = await _unitOfWork.SaveChangeAsync();
                return rs > 0;
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);
                throw ex;
            }
            

        }

        public async Task<User?> UpdateAsync(UpdateUserDto userDto)
        {
            try
            {
                if (userDto == null)
                {
                    throw new ArgumentNullException(nameof(userDto), "categoryDto cannot be null");
                }

                // Bắt đầu transaction
                await _unitOfWork.BeginTransactionAsync();

                // Tìm category cần cập nhật
                var user = await GetByIdAsync(userDto.Id);
                if (userDto == null)
                {
                    throw new KeyNotFoundException($"Category with ID {userDto.Id} not found");
                }

                // Cập nhật thông tin brand từ DTO
                _mapper.Map(userDto, user);

                // Xử lý upload file mới nếu có
                if (userDto.FormFile != null)
                {
                    // Upload file mới
                    var imageFile = await _imageFileService.UploadFileAsync(userDto.FormFile);

                    // Xóa file cũ nếu có
                    if (user.ImageId.HasValue)
                    {
                        await _imageFileService.DeleteFileAsync(user.ImageId.Value);
                    }

                    user.ImageId = imageFile.Id;
                }



                BaseEntityService<User>.Update(user); // Cập nhật các thuộc tính cơ bản như UpdatedAt, UpdatedBy
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return user;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine($"Error updating category: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw ex;

            }
        }
    }
}
