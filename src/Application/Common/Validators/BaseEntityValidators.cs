using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Constants;
using Sonuts.Domain.Common;

namespace Sonuts.Application.Common.Validators;

internal static class BaseEntityValidators
{
	public static IRuleBuilder<T, Guid?> Exists<T, TEntity>(this IRuleBuilder<T, Guid?> ruleBuilder, IQueryable<TEntity> query)
		where TEntity : BaseEntity =>
		ruleBuilder
			.MustAsync(async (id, cancellationToken) =>
				id is not null
				&& await query.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken) is not null)
			.WithMessage($"{typeof(TEntity).Name} '{{PropertyValue}}' could not be found.")
			.WithErrorCode(ErrorCodes.NotFound);
}
