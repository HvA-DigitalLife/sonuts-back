namespace Sonuts.Infrastructure.Fhir.Interfaces;

public interface IDomainDao
{
	Task<List<Models.Domain>> SelectAll ();
	Task<Models.Domain> Insert ( Models.Domain domain );
	Task<bool> Update ( Models.Domain participant );
	Task<bool> Delete ( int participantId );
}
