using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Service.Interface;
using AutoMapper;
using Domain.Enity;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Service.Implementation
{
    public class ImageFileService : IImageFileService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public ImageFileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            bool result = false;
            try
            {
                await _unitOfWork.ImageFiles.DeleteAsync(id);
                result = await _unitOfWork.SaveChangeAsync() > 0;
            } catch(Exception ex)
            {
                throw new Exception("Error deleting file", ex);
            }

             return result;
        }

        public Task<List<ImageFileDto>> GetAllFileAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ImageFile> GetByIdAsync(int id)
        {
            try
            {
                ImageFile result;
                result = await _unitOfWork.ImageFiles.GetByIdAsync(id);
                if (result == null)
                {
                    throw new Exception("File not found");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while get image id " + ex.Message);
                throw new Exception("Error while get image id", ex);
            }
        }

        public async Task<ImageFile> UploadFileAsync(IFormFile file)
        {
            if(file == null || file.Length == 0)
            { 
                throw new Exception("File is empty");
            }
            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var image = new ImageFile()
                {
                    Name = file.FileName,
                    Data = memoryStream.ToArray(),
                    Type = file.ContentType
                };
                await _unitOfWork.ImageFiles.AddAsync(image);
                await _unitOfWork.SaveChangeAsync();
                return image;
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file", ex);
            }
        }
    }
}
