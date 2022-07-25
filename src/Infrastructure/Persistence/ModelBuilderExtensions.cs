using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sonuts.Infrastructure.Persistence;

internal static class ModelBuilderExtensions
{
	internal static ModelBuilder RenameIdentityTables(this ModelBuilder builder)
	{
		builder.Entity<IdentityRole>().ToTable("Roles");
		builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
		builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
		builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
		builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
		builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

		return builder;
	}

	internal static ModelBuilder AddEnumStringConversions(this ModelBuilder builder)
	{
		foreach (var property in builder.Model.GetEntityTypes().SelectMany(type => type.GetProperties().Where(property => property.ClrType.IsEnum)))
		{
			var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
			var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

			property.SetValueConverter(converter);
		}

		return builder;
	}
}
