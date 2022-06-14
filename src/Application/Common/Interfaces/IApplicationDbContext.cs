using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<TodoList> TodoLists { get; }

	DbSet<TodoItem> TodoItems { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
