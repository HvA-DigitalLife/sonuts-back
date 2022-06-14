using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.Property(content => content.Color)
			.HasMaxLength(6)
			.IsRequired();
	}
}
