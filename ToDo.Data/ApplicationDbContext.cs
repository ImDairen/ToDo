using Microsoft.EntityFrameworkCore;
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

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Do>(e =>
        //    {
        //        e.ToTable("ToDoes");
        //        e.Property(e => e.Id).HasColumnName("DoId");
        //        e.HasOne<Do>().WithOne().HasForeignKey<Do>(e => e.Id);
        //    });
        //}
    }
}
