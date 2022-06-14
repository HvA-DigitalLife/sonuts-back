using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
	public void Configure(EntityTypeBuilder<Content> builder)
	{
		builder.HasIndex(content => content.Type)
			.IsUnique();

		builder.Property(content => content.Type)
			.IsRequired()
			.HasConversion<string>();

		builder.Property(content => content.Title)
			.HasMaxLength(200)
			.IsRequired();
	}
}
