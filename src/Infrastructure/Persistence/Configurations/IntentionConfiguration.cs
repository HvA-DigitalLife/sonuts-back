using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class IntentionConfiguration : IEntityTypeConfiguration<Goal>
{
	public void Configure(EntityTypeBuilder<Goal> builder)
	{
		builder.OwnsOne(intention => intention.Moment)
			.Property(moment => moment.Type)
			.HasConversion<string>();
	}
}
