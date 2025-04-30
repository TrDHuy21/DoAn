using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Domain.Enity;
using Infrastructure.Repo.Interface;

namespace Infrastructure.Repo.Implementation
{
    public class ImageFileRepo : GenericRepo<ImageFile, int>, IImageFileRepo
    {
       public ImageFileRepo(ShopTechContext dbContext) : base(dbContext)
        {
        }
    }
}
