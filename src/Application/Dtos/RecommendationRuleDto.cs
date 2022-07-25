using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class RecommendationRuleDto : IMapFrom<RecommendationRule>
{
	public RecommendationRuleType Type { get; set; }
	public List<QuestionDto> Questions { get; set; } = new();
}
