using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Interfaces;

public interface IGuidelineDao
{
	Task<List<Guideline>> SelectAllByDomainId (string domainId);
	Task<Guideline> Insert ( Guideline guideline );
	Task<bool> Update ( Guideline guideline );
	Task<bool> Delete ( int guidelineId );
}
