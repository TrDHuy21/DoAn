using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.RoleDtos;

namespace Application.Dtos.UserDtos
{
    public class IndexUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public RoleDto Role { get; set; }

    }
}
