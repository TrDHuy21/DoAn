using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.UserDtos
{
    public class AddUserDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Username is required.")]

        public string Password { get; set; }
        public string Cccd { get; set; }
        public IFormFile? FormFile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AdressDetail { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Username is required.")]

        public int? RoleId { get; set; }
        [Required(ErrorMessage = "Username is required.")]

        public string WardId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
