using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data.Interfaces;
using ToDo.Data.Models;

namespace ToDo.Data.Repositories
{
    public class DoRepository : IRepository<Do>
    {
        private readonly ApplicationDbContext _dbContext;

        public DoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbContext DbContext { get { return _dbContext; } }
        protected DbSet<Do> Set { get { return DbContext.Set<Do>(); } }

        public Do FindById(int id)
        {
            return Set.Include(x => x.SubTasks)
                .AsQueryable()
                .First(x => x.Id == id);
        }

        public Task<Do> FindByIdAsync(int id)
        {
            return  Set.Include(x => x.SubTasks)
                .AsQueryable().
                FirstAsync(x => x.Id == id);
        }

        public IEnumerable<Do> FindByIds(IEnumerable<int> ids)
        {
            return Set.Include(x => x.SubTasks)
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public async Task<IEnumerable<Do>> FindByIdsAsync(IEnumerable<int> ids)
        {
            return await Set.Include(x => x.SubTasks)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public IEnumerable<Do> GetAll()
        {
            return Set
                .Include(x => x.SubTasks)
                .ToList();
        }

        public async Task<IEnumerable<Do>> GetAllAsync()
        {
            return await Set
                .Include(x => x.SubTasks)
                .ToListAsync();
        }

        public Do Insert(Do entity)
        {
            return Set.Add(entity).Entity;
        }

        public Do Update(Do entity)
        {
            AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(int id)
        {
            var entity = Set.Find(id);

            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public void Delete(Do entity)
        {
            AttachIfNot(entity);
            Set.Remove(entity);
        }

        public IQueryable<Do> AsQueryable()
        {
            return Set.AsQueryable();
        }

        protected void AttachIfNot(Do entity)
        {
            if (!Set.Local.Contains(entity))
            {
                Set.Attach(entity);
            }
        }
    }
}
