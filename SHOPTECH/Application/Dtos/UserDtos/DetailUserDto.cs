using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.ImageDtos;
using Application.Dtos.RoleDtos;
using Domain.Enity;

namespace Application.Dtos.UserDtos
{
    public class DetailUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public DateTime? BirthDate { get; set; }
        public string? AdressDetail { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public string? Username { get; set; }
        //role
        public RoleDto? Role { get; set; }
        //image
        public ImageFileDto? Image { get; set; }

        //address
        public string Ward { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
