﻿using Microsoft.EntityFrameworkCore;
using ToDo.Data.Models;

namespace ToDo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Do> ToDoes { get; set; }
    }
}
