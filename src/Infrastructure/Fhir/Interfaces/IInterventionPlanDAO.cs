using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Interfaces;

public interface IInterventionPlanDao
{
	Task<List<InterventionPlan>> SelectAllByParticipantId (string participantId);
	Task<InterventionPlan> Insert ( InterventionPlan interventionPlan );
	Task<bool> Update ( InterventionPlan interventionPlan );
	Task<bool> Delete ( int recommendationId );
}
