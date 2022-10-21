using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IQuestionnaireDao
{
	Task<Questionnaire> SelectByCategoryId( Guid id );
	Task<Questionnaire> Select( Guid id );
	Task<Questionnaire> Insert( Questionnaire questionnaire) ;

}
