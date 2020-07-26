using Microsoft.EntityFrameworkCore;
using System;
using ToDo.Data.Interfaces;
using ToDo.Data.Models;
using ToDo.Data.Repositories;

namespace ToDo.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private DoRepository _doRepository;

        public UnitOfWork(DbContextOptions<ApplicationDbContext> options)
        {
            _dbContext = new ApplicationDbContext(options);
        }

        public IRepository<Do> ToDoes
        {
            get
            {
                if (_doRepository == null)
                    _doRepository = new DoRepository(_dbContext);
                return _doRepository;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
