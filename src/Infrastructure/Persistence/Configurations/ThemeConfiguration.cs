using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
{
	public void Configure(EntityTypeBuilder<Theme> builder)
	{
		builder.Property(content => content.FrequencyType)
			.HasConversion<string>();

		builder.OwnsOne(activity => activity.QuestionDependency)
			.Property(questionDependency => questionDependency.Operator)
			.HasConversion<string>();
	}
}
