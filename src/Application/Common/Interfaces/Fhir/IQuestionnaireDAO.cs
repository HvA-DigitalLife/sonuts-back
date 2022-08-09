using Sonuts.Domain.Entities;

namespace Sonuts.Application.Common.Interfaces.Fhir;

public interface IQuestionnaireDao
{
	Task<Questionnaire> Select(string id);
	Task<Questionnaire> Insert(Questionnaire questionnaire);

}
