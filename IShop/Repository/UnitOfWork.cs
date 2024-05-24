using IShop.Data;
using IShop.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IShop.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<User> _user;
        private IGenericRepository<Role> _role;
        private IGenericRepository<Street> _street;
        private IGenericRepository<Rate> _rate;
        private IGenericRepository<Product> _product;
        private IGenericRepository<Log> _log;
        private IGenericRepository<Favorite> _favorite;
        private IGenericRepository<Favorite_Product> _fv;
        private IGenericRepository<CompanyDetails> _companyDetails;
        private IGenericRepository<Category> _category;
        private IGenericRepository<Cart> _cart;
        private IGenericRepository<Cart_Product> _cp;
        private IGenericRepository<Log_Product> _log_product;
        private IGenericRepository<ProductImage> _productImage;
        private IGenericRepository<ConfirmCode> _confirmCode;
        private IGenericRepository<Reset_Password> _resetPassword;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<User> User => _user ??= new GenericRepository<User>(_context);

        public IGenericRepository<Role> Role => _role ??= new GenericRepository<Role>(_context);

        public IGenericRepository<Street> Street => _street ??= new GenericRepository<Street>(_context);

        public IGenericRepository<Rate> Rate => _rate ??= new GenericRepository<Rate>(_context);

        public IGenericRepository<Product> Product => _product ??= new GenericRepository<Product>(_context);

        public IGenericRepository<Log> Log => _log ??= new GenericRepository<Log>(_context);

        public IGenericRepository<Favorite> Favorite => _favorite ??= new GenericRepository<Favorite>(_context);

        public IGenericRepository<CompanyDetails> CompanyDetails => _companyDetails ??= new GenericRepository<CompanyDetails>(_context);

        public IGenericRepository<Category> Category => _category ??= new GenericRepository<Category>(_context);

        public IGenericRepository<Cart> Cart => _cart ??= new GenericRepository<Cart>(_context);

        public IGenericRepository<Cart_Product> Cart_Product => new GenericRepository<Cart_Product>(_context);
        public IGenericRepository<Favorite_Product> Favorite_Product => new GenericRepository<Favorite_Product>(_context);
        public IGenericRepository<Log_Product> Log_Product => new GenericRepository<Log_Product>(_context);
        public IGenericRepository<ProductImage> ProductImage => new GenericRepository<ProductImage>(_context);
        public IGenericRepository<ConfirmCode> ConfirmCode => new GenericRepository<ConfirmCode>(_context);
        public IGenericRepository<Reset_Password> ResetPassword => new GenericRepository<Reset_Password>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
