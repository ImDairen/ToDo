using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Data.Interfaces
{
    public interface IRepository<TEntity> 
        where TEntity: class, IEntity
    {
        TEntity FindById(int id);
        Task<TEntity> FindByIdAsync(int id);
        IEnumerable<TEntity> FindByIds(IEnumerable<int> ids);
        Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<int> ids);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);
        IQueryable<TEntity> AsQueryable();
    }
}
