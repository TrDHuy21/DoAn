using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Domain.Enity;
using Microsoft.AspNetCore.Http;

namespace Application.Service.Interface
{
    public interface IImageFileService
    {
        public Task<ImageFile> GetByIdAsync(int id);

        //return this imagefile if upload is success
        public Task<ImageFile> UploadFileAsync(IFormFile file);
        public Task<bool> DeleteFileAsync(int id);

        public Task<List<ImageFileDto>> GetAllFileAsync();
    }
}
