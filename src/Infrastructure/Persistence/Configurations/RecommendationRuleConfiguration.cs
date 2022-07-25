using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Configurations;

public class RecommendationRuleConfiguration : IEntityTypeConfiguration<RecommendationRule>
{
	public void Configure(EntityTypeBuilder<RecommendationRule> builder)
	{
		builder.Property(recommendationRule => recommendationRule.Type)
			.HasConversion<string>();

		builder.Property(recommendationRule => recommendationRule.Operator)
			.HasConversion<string>();
	}
}
