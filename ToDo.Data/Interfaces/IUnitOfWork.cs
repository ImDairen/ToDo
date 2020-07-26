using System;
using ToDo.Data.Models;

namespace ToDo.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Do> ToDoes { get; }
        void Save();
    }
}
