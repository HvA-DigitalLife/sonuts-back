using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
	public void Configure(EntityTypeBuilder<Activity> builder)
	{
		builder.OwnsOne(activity => activity.QuestionDependency)
			.Property(questionDependency => questionDependency.Operator)
			.HasConversion<string>();
	}
}
