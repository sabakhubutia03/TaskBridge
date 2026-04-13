using Microsoft.EntityFrameworkCore;
using TaskBridge.Domain.Entity;

namespace TaskBridge.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TaskItem> Tasks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}