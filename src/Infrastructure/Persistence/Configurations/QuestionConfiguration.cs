using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
	public void Configure(EntityTypeBuilder<Question> builder)
	{
		builder.Property(question => question.Type)
			.HasConversion<string>();

		builder.OwnsOne(activity => activity.QuestionDependency)
			.Property(questionDependency => questionDependency.Operator)
			.HasConversion<string>();
	}
}
