using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class QuestionnaireConfiguration : IEntityTypeConfiguration<Questionnaire>
{
	public void Configure(EntityTypeBuilder<Questionnaire> builder)
	{
		builder.Property(content => content.Title)
			.HasMaxLength(200)
			.IsRequired();
	}
}
