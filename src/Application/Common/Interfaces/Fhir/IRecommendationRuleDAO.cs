using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IRecommendationRuleDao
{
	Task<RecommendationRule> Select ( Guid id);
	Task<RecommendationRule> Insert ( RecommendationRule recommendationRule );
	Task<RecommendationRule> Update ( RecommendationRule recommendationRule );
	Task<bool> Delete ( Guid id );
}