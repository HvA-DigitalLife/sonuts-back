using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class IntentionConfiguration : IEntityTypeConfiguration<Intention>
{
	public void Configure(EntityTypeBuilder<Intention> builder)
	{
		builder.OwnsOne(intention => intention.Moment)
			.Property(moment => moment.Type)
			.HasConversion<string>();
	}
}
