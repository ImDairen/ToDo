using System;
using System.Collections.Generic;

namespace ToDo.Data.Interfaces
{
    public interface IRepository<TEntity> 
        where TEntity: class, IEntity
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
