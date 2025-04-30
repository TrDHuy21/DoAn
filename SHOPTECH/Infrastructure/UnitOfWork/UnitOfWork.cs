using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Context;
using Domain.Enity;
using Infrastructure.Repo.Implementation;
using Infrastructure.Repo.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopTechContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ShopTechContext context)
        {
            _context = context;
            Brands = new BrandRepo(_context);
            Carts = new CartRepo(_context);
            Categories = new CategoryRepo(_context);
            Districts = new DistrictRepo(_context);
            Users = new UserRepo(_context);
            ImageFiles = new ImageFileRepo(_context);
            OrderDetails = new OrderDetailRepo(_context);
            Orders = new OrderRepo(_context);
            ProductAttributes = new ProductAttributeRepo(_context);
            ProductDetailAttributes = new ProductDetailAttributeRepo(_context);
            ProductDetails = new ProductDetailRepo(_context);
            Products = new ProductRepo(_context);
            Provinces = new ProvinceRepo(_context);
            Roles = new RoleRepo(_context);
            Wards = new WardRepo(_context);

        }

        public IBrandRepo Brands { get; }
        public ICartRepo Carts { get; }
        public ICategoryRepo Categories { get; }
        public IDistrictRepo Districts { get; }
        public IUserRepo Users { get; }
        public IImageFileRepo ImageFiles { get; }
        public IOrderDetailRepo OrderDetails { get; }
        public IOrderRepo Orders { get; }
        public IProductAttributeRepo ProductAttributes { get; }
        public IProductDetailAttributeRepo ProductDetailAttributes { get; }
        public IProductDetailRepo ProductDetails { get; }
        public IProductRepo Products { get; }
        public IProvinceRepo Provinces { get; }
        public IRoleRepo Roles { get; }
        public IWardRepo Wards { get; }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // load reference
        public void LoadReference<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class
        {
            _context.Entry<E>(entity).Reference<R>(propertyExpression).Load();
        }

        public void LoadCollection<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class
        {
            _context.Entry<E>(entity).Collection<R>(propertyExpression).Load();
        }

        public Task LoadReferenceAsync<E, R>(E entity, Expression<Func<E, R?>> propertyExpression) where E : class where R : class
        {
            return _context.Entry<E>(entity).Reference<R>(propertyExpression).LoadAsync();
        }

        public Task LoadCollectionAsync<E, R>(E entity, Expression<Func<E, IEnumerable<R>>> propertyExpression) where E : class where R : class
        {
            return _context.Entry<E>(entity).Collection<R>(propertyExpression).LoadAsync();
        }


        //save change
        public async Task<int> SaveChangeAsync()
        {
            var result = 0;
            try
            {
                result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
