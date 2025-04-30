using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Domain.Enity;
using Infrastructure.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repo.Implementation
{
    public class UserRepo : GenericRepo<User, int>, IUserRepo
    {
        public UserRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByUserName(string userName)
        {
            User? user = await _context.Users.Where(e => e.Username == userName).FirstOrDefaultAsync();
            return user;
        }
    }
}
