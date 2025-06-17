using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enity
{
    public class User:BaseEntity
    {
        public DateTime? BirthDate { get; set; }
        public string? AdressDetail { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string HashedPassword { get; set; }

        //role
        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        //image
        public int? ImageId { get; set; }
        public ImageFile? Image { get; set; }

        //address
        public string? WardId { get; set; }
        public Ward? Ward { get; set; }

        //order
        public IEnumerable<Order>? EmployeeOrders { get; set; }
        public IEnumerable<Order>? CustomerOrders { get; set; }

        //cart
        public IEnumerable<Cart>? Carts { get; set; }


        //public IEnumerable<Brand>? CreatedBrand { get; set; }
        //public IEnumerable<Brand>? UpdatedBrand { get; set; }
        //public IEnumerable<Category>? CreatedCategory { get; set; }
        //public IEnumerable<Category>? UpdatedCategory { get; set; }
        //public IEnumerable<User>? CreatedUser { get; set; }
        //public IEnumerable<User>? UpdatedUser { get; set; }
        //public IEnumerable<Product>? CreatedProduct { get; set; }
        //public IEnumerable<Product>? UpdatedProduct { get; set; }
        //public IEnumerable<ProductAttribute>? CreatedProductAttribute { get; set; }
        //public IEnumerable<ProductAttribute>? UpdatedProductAttribute { get; set; }
        //public IEnumerable<ProductDetail>? CreatedProductDetail { get; set; }
        //public IEnumerable<ProductDetail>? UpdatedProductDetail { get; set; }
    }
}
