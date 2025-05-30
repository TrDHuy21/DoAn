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
using Domain.Entity;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopTechContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ShopTechContext context)
        {
            _context = context;
        }

        private IBrandRepo _brands;
        public IBrandRepo Brands => _brands ??= new BrandRepo(_context);

        private ICartRepo _carts;
        public ICartRepo Carts => _carts ??= new CartRepo(_context);

        private ICategoryRepo _categories;
        public ICategoryRepo Categories => _categories ??= new CategoryRepo(_context);

        private IDistrictRepo _districts;
        public IDistrictRepo Districts => _districts ??= new DistrictRepo(_context);

        private IUserRepo _users;
        public IUserRepo Users => _users ??= new UserRepo(_context);

        private IImageFileRepo _imageFiles;
        public IImageFileRepo ImageFiles => _imageFiles ??= new ImageFileRepo(_context);

        private IOrderDetailRepo _orderDetails;
        public IOrderDetailRepo OrderDetails => _orderDetails ??= new OrderDetailRepo(_context);

        private IOrderRepo _orders;
        public IOrderRepo Orders => _orders ??= new OrderRepo(_context);

        private IProductAttributeRepo _productAttributes;
        public IProductAttributeRepo ProductAttributes => _productAttributes ??= new ProductAttributeRepo(_context);

        private IProductDetailAttributeRepo _productDetailAttributes;
        public IProductDetailAttributeRepo ProductDetailAttributes => _productDetailAttributes ??= new ProductDetailAttributeRepo(_context);

        private IProductDetailRepo _productDetails;
        public IProductDetailRepo ProductDetails => _productDetails ??= new ProductDetailRepo(_context);

        private IProductRepo _products;
        public IProductRepo Products => _products ??= new ProductRepo(_context);

        private IProvinceRepo _provinces;
        public IProvinceRepo Provinces => _provinces ??= new ProvinceRepo(_context);

        private IRoleRepo _roles;
        public IRoleRepo Roles => _roles ??= new RoleRepo(_context);

        private IWardRepo _wards;
        public IWardRepo Wards => _wards ??= new WardRepo(_context);

        private IProductImageRepo _productImages;
        public IProductImageRepo ProductImages => _productImages ??= new ProductImageRepo(_context);

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
