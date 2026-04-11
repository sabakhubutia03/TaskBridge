using Microsoft.EntityFrameworkCore;
using TaskBridge.Domain.Entity;

namespace TaskBridge.Infrastructure.Data;

public class AppDbContext :  DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskBridge.Domain.Entity.Application> Applications { get; set; }
}