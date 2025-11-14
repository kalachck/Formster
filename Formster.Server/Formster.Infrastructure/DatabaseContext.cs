using Formster.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Formster.Infrastructure;

public sealed class DatabaseContext : DbContext
{
    public DbSet<FormSubmission> FormSubmissions { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormSubmission>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FormName).IsRequired();
            entity.Property(x => x.JsonData).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
