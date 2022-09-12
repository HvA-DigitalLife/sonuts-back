using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IQuestionnaireDao
{
	Task<Questionnaire> SelectByCategoryId(System.Guid id);
	Task<Questionnaire> Select(System.Guid id);
	Task<Questionnaire> Insert(Questionnaire questionnaire);

}
