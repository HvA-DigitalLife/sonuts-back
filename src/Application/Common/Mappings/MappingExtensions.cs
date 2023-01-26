using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Models;

namespace Sonuts.Application.Common.Mappings;

public static class MappingExtensions
{
	public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
		this IQueryable<TDestination> queryable,
		int pageNumber, int pageSize)
		where TDestination : class =>
		await PaginatedList<TDestination>
			.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

	public static async Task<List<TDestination>> ProjectToListAsync<TDestination>(
		this IQueryable queryable,
		IConfigurationProvider configuration,
		CancellationToken cancellationToken = default)
		where TDestination : class =>
		await queryable
			.ProjectTo<TDestination>(configuration)
			.AsNoTracking()
			.ToListAsync(cancellationToken);
}
