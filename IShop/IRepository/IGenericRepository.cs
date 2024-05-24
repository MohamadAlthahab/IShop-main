using IShop.Data;
using IShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace IShop.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            List<string> includes = null);


        Task<IEnumerable<Product>> Search(string name) ;
   

        Task<T> Get(Expression<Func<T, bool>> expression,List<string> includes = null);
        void Add (T enAsynctity);
        Task Insert(T entity);
        Task Remove(int id);
        void Update(T entity);
    }
}
