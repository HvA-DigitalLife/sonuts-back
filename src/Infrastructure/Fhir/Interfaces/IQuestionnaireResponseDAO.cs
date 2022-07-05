using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Interfaces;

public interface IQuestionnaireResponseDao
{
	Task<QuestionnaireResponse> Select ( string id );
	Task<QuestionnaireResponse> Insert ( QuestionnaireResponse questionnaireResponse );
	Task<bool> Update ( QuestionnaireResponse questionnaireResponse );
	Task<bool> Delete ( int id );
}
