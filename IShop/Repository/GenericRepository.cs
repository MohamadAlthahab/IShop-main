using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IShop.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;
        public GenericRepository(DatabaseContext context) 
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public async Task Remove(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var includePropery in includes)
                {
                    query = query.Include(includePropery);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;

            if(expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includePropery in includes)
                {
                    query = query.Include(includePropery);
                }
            }

            if (orderby != null)
            {
                query = orderby(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }


        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            IQueryable<Product> query = _context.Product;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Contains(name));
            }
            return await query.ToListAsync();
        }
    }
}
