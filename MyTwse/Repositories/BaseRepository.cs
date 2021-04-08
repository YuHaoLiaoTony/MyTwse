using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyTwse.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        public void Create(T model);
        public void Update(T model);
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda);
        public T GetBy(Expression<Func<T, bool>> whereLambda);
        public int Delete(T model);
        public List<T> GetPagedListOrderBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy);
    }

    public class BaseRepository<T> where T : class
    {
        protected DbContext _DB = null;
        public BaseRepository(DbContext context)
        {
            _DB = context;
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return _DB.Set<T>().Where(whereLambda).ToList();
        }
        public T GetBy(Expression<Func<T, bool>> whereLambda)
        {
            return _DB.Set<T>().Where(whereLambda).FirstOrDefault();
        }
        public void Create(T model) 
        {
            _DB.Set<T>().Add(model);
            _DB.SaveChanges();
        }
        public void Update(T model)
        {
            _DB.Set<T>().Update(model);
            _DB.SaveChanges();
        }
        public int Delete(T model)
        {
            _DB.Set<T>().Attach(model);
            _DB.Set<T>().Remove(model); 
            return _DB.SaveChanges();
        }
        public List<T> GetPagedListOrderBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy 
            return _DB.Set<T>().Where(whereLambda).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<T> GetPagedListOrderByDescending<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy 
            return _DB.Set<T>().Where(whereLambda).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
