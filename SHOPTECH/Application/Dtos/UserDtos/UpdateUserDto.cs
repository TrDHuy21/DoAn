using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AdressDetail { get; set; }
        public ImageFileDto? Image { get; set; }

        public IFormFile? FormFile { get; set; }
    }
}
