using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Do> GetAll()
        {
            return _dbContext.ToDoes
                .Include(x => x.SubTasks);
        }

        public Do Get(int id)
        {
            return _dbContext.ToDoes.Find(id);
        }

        public void Create(Do entity)
        {
            _dbContext.ToDoes.Add(entity);
        }

        public void Update(Do entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Do> Find(Func<Do, Boolean> predicate)
        {
            return _dbContext.ToDoes.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Do entity = _dbContext.ToDoes.Find(id);

            if (entity != null)
                _dbContext.Remove(entity);
        }
    }
}
