using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IQuestionnaireResponseDao
{
	Task<QuestionnaireResponse> Select ( Guid id );
	Task<QuestionnaireResponse> Insert ( QuestionnaireResponse questionnaireResponse );
	Task<bool> Update ( QuestionnaireResponse questionnaireResponse );
	Task<bool> Delete ( Guid id );
}
