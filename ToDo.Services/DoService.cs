using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Data.Models;
using ToDo.Services.Interfaces;
using ToDo.Services.Models;

namespace ToDo.Services
{
    public class DoService : IDoService
    {
        private readonly ApplicationDbContext _context;

        public DoService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Work with database
        public IEnumerable<DoServiceModel> GetAll()
        {
            return _context.ToDoes
                .Include(d => d.SubTasks)
                .Select(d => new DoServiceModel(d));

        }

        public DoServiceModel GetById(int id)
        {
            var entity = _context.ToDoes
                .Where(d => d.Id == id)
                .First();

            if (entity == null)
                return null;

            return new DoServiceModel(entity);
        }

        public async Task Add(DoServiceModel model)
        {
            var entity = new Do
            {
                Title = model.Title,
                Status = model.Status,
                Plan = model.Plan,
                Fact = model.Fact
            };

            _context.ToDoes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<DoServiceModel> Delete(int id)
        {
            var entity = _context.ToDoes
                .Where(e => e.Id == id)
                .First();

            if (entity != null)
            {
                _context.ToDoes.Remove(entity);
                await _context.SaveChangesAsync();
                return new DoServiceModel(entity);
            }
            else throw new NullReferenceException();
        }

        public async Task<DoServiceModel> Update(DoServiceModel model)
        {
            AttachIfNot(model);
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return model;
        }

        private void AttachIfNot(DoServiceModel model)
        {
            var entity = _context.ToDoes
                .Where(e => e.Id == model.Id)
                .First();

            if (!_context.ToDoes.Local.Contains(entity))
            {
                _context.ToDoes.Attach(entity);
            }
        }
        #endregion
    }
}
