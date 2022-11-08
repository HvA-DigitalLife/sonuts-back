using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirQuestionnaireResponseAdapter
{
	public static QuestionnaireResponse FromJson (string json)
	{ 

		var fhirJsonParser = new FhirJsonParser();
		var fhirQuestionnaireResponse = fhirJsonParser.Parse<Hl7.Fhir.Model.QuestionnaireResponse>(json);

		// create questionnaire instance
		QuestionnaireResponse questionnaireResponse = new QuestionnaireResponse{
			Id = Guid.Parse(fhirQuestionnaireResponse.Id)
		};

		// to-do use separate identifiers fields
		questionnaireResponse.Questionnaire = new Questionnaire{Id = Guid.Parse(fhirQuestionnaireResponse.Questionnaire.ToString().Replace("Questionnaire/", ""))};
		questionnaireResponse.Participant = new Participant{Id = Guid.Parse(fhirQuestionnaireResponse.Author.Reference.ToString().Replace("Patient/", ""))};
         

		// loop trough fhir questions instance to questionnaire
		foreach (var fhirItem in fhirQuestionnaireResponse.Item) {
			var questionResponse = new QuestionResponse();
			questionResponse.Id = Guid.Parse(fhirItem.LinkId);

			// parse responses
			foreach (var fhirQuestionResponse in fhirItem.Answer) {
				if (fhirQuestionResponse.Value.GetType() == typeof(Hl7.Fhir.Model.FhirString)) {
					questionResponse.Answer = fhirQuestionResponse.Value.ToString() ?? "";
				}

			}
			// add response to questionnaire response
			questionnaireResponse.Responses.Add(questionResponse);
		}
		return questionnaireResponse;
	}

	public static string ToJson ( QuestionnaireResponse questionnaireResponse )
	{ 
		// create fhir questionnaire response
		var fhirQuestionnaireResponse = new Hl7.Fhir.Model.QuestionnaireResponse{
			Id = questionnaireResponse.Id.ToString()
		};
	

		//// to-do use separate identifiers fields
		fhirQuestionnaireResponse.Questionnaire = "Questionnaire/" + questionnaireResponse.Questionnaire.Id.ToString();
		fhirQuestionnaireResponse.Author = new Hl7.Fhir.Model.ResourceReference("Patient/" + questionnaireResponse.Participant.Id.ToString());
          
		foreach (var questionReponse in questionnaireResponse.Responses) {
			// create and fill response
			var fhirItem = new Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent();
			fhirItem.LinkId = questionReponse.Id.ToString();
			var fhirAnswer = new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent();
			fhirAnswer.Value = new Hl7.Fhir.Model.FhirString(questionReponse.Answer);
			fhirItem.Answer.Add(fhirAnswer);
			fhirQuestionnaireResponse.Item.Add(fhirItem);

		}

		// serialize and return FHIR object
		var fhirJsonSerializer = new FhirJsonSerializer();

		return fhirJsonSerializer.SerializeToString(fhirQuestionnaireResponse);
	}
}
