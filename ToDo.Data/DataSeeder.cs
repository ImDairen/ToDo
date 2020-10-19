using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data.Models;
using ToDo.Data.Models.Static;

namespace ToDo.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }


        public Task Seed()
        {
            if (_context.ToDoes.Count() != 0)
                return Task.CompletedTask;

            var first = new Do
            {
                Title = "First Task",
                Description = "This is First Task",
                Status = DoStatus.Created,
                Executors = "Kalinin",
                Created = DateTime.Now,
                Done = null,
                Plan = 20,
                Fact = 10
            };

            var second = new Do
            {
                Title = "Second Task",
                Description = "This is Second Task",
                Status = DoStatus.Created,
                Executors = "Someone",
                Created = DateTime.Now,
                Done = null,
                Plan = 40,
                Fact = 0,
                SubTasks = new List<Do> { first }
            };

            var third = new Do
            {
                Title = "Third Task",
                Description = "This is Third Task",
                Status = DoStatus.Done,
                Executors = "Someone",
                Created = DateTime.Now,
                Done = DateTime.Now,
                Plan = 12,
                Fact = 10,
                SubTasks = new List<Do> { second }
            };

            _context.ToDoes.Add(first);
            _context.ToDoes.Add(second);
            _context.ToDoes.Add(third);

            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
