using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;

namespace Infrastructure.Repo.Interface
{
    public interface IUserRepo : IGenericRepo<User, int>
    {
       public Task<User?> GetByUserName(string userName);
    }
}
