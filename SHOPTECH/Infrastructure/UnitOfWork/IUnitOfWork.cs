using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repo.Interface;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepo Brands { get; }
        ICartRepo Carts { get; }
        ICategoryRepo Categories { get; }
        IDistrictRepo Districts { get; }
        IUserRepo Users { get; }
        IImageFileRepo ImageFiles { get; }
        IOrderDetailRepo OrderDetails { get; }
        IOrderRepo Orders { get; }
        IProductAttributeRepo ProductAttributes { get; }
        IProductDetailAttributeRepo ProductDetailAttributes { get; }
        IProductDetailRepo ProductDetails { get; }
        IProductRepo Products { get; }
        IProvinceRepo Provinces { get; }
        IRoleRepo Roles { get; }
        IWardRepo Wards { get; }
        IProductImageRepo ProductImages { get; }

        // transaction 
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // load reference
        void LoadReference<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class;
        void LoadCollection<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class;
        Task LoadReferenceAsync<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class;
        Task LoadCollectionAsync<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class;

        //save change
        Task<int> SaveChangeAsync();
    }
}
