using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
    public class BaseRepository
    {
        protected DbContext _DB = null;
        public BaseRepository(DbContext context)
        {
            _DB = context;
        }
        protected List<T> GetListBySql<T>(string sql,params object[] parameters) where T : class
        {
            return _DB.Set<T>().FromSqlRaw(sql, parameters).ToList();
        }
        protected T GetBySql<T>(string sql, params object[] parameters) where T : class
        {
            
            return _DB.Set<T>().FromSqlRaw(sql, parameters).FirstOrDefault();
        }

        protected List<T> ExecSQL<T>(string query, object parameters = null) where T : class
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            if (parameters != null)
            {
                foreach (var para in parameters.GetType().GetProperties())
                {
                    var value = para.GetValue(parameters);
                    paras.Add(new SqlParameter($"@{para.Name}", value));
                }
            }
            return ExecSQL<T>(query, paras);
        }
        public List<T> ExecSQL<T>(string query, List<SqlParameter> parameters = null)
        {
            using (var command = _DB.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    foreach (var para in parameters)
                    {
                        command.Parameters.Add(para);
                    }
                }

                _DB.Database.OpenConnection();

                List<T> list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                _DB.Database.CloseConnection();
                return list;
            }
        }
    }
    public class BaseRepository<T> : BaseRepository where T : class
    {
        public BaseRepository(DbContext context) : base(context)
        {
        }
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return _DB.Set<T>().Where(whereLambda).ToList();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListBySQL(string sql,object[] parameters)
        {
            return _DB.Set<T>().FromSqlRaw<T>(sql, parameters).ToList();
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
