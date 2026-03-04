using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace dasboardApplications.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        int Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
