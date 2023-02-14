using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Domain.Common;

namespace Sonuts.Application.Common.Extensions;
internal static class QueryableExtensions
{
	/// <summary>
	/// Get first entity with <see cref="id"/> and if not found throws <see cref="NotFoundException"/>.
	/// </summary>
	/// <param name="query"></param>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="NotFoundException"></exception>
	public static async Task<TEntity> FindOrNotFoundAsync<TEntity>(this IQueryable<TEntity> query, Guid id, CancellationToken cancellationToken = default)
		where TEntity : BaseEntity =>
		await query.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken)
		?? throw new NotFoundException(typeof(TEntity).Name, id.ToString());
}
