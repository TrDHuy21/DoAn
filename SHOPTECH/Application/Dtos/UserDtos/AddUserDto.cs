using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.UserDtos
{
    public class AddUserDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Cccd { get; set; }
        public IFormFile? FormFile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AdressDetail { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? RoleId { get; set; }
        public string WardId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
