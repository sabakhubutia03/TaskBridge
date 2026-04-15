using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;

namespace TaskBridge.Infrastructure.Data;

public class AppDbContext :  DbContext , IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskBridge.Domain.Entity.Applicationn> Applications { get; set; } 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
         
        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Budget)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<TaskBridge.Domain.Entity.Applicationn>()
            .HasOne(a => a.Freelancer)
            .WithMany()
            .HasForeignKey(a => a.FreelancerId)
            .OnDelete(DeleteBehavior.NoAction); 
    }
}