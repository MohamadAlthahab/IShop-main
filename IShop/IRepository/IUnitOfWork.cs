using IShop.Data;
using System;
using System.Threading.Tasks;

namespace IShop.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> User { get; }
        IGenericRepository<Role> Role { get; }
        IGenericRepository<Street> Street { get; }
        IGenericRepository<Rate> Rate { get; }
        IGenericRepository<Product> Product { get; }
        IGenericRepository<Log> Log { get; }
        IGenericRepository<Favorite> Favorite { get; }
        IGenericRepository<CompanyDetails> CompanyDetails { get; }
        IGenericRepository<Category> Category { get; }
        IGenericRepository<Cart> Cart { get; }
        IGenericRepository<Cart_Product> Cart_Product { get; }
        IGenericRepository<Favorite_Product> Favorite_Product { get; }
         IGenericRepository<Log_Product> Log_Product { get; }
         IGenericRepository<ProductImage> ProductImage { get; }
        IGenericRepository<ConfirmCode> ConfirmCode { get; }
        IGenericRepository<Reset_Password> ResetPassword { get; }

        Task Save();
    }
}
