using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirQuestionnaireResponseAdapter
{
	public static QuestionnaireResponse FromJson (string json)
	{ 
		// create questionnaire instance
		QuestionnaireResponse questionnaireResponse = new QuestionnaireResponse();
         
        
		var parser = new FhirJsonParser();
		var fhirQuestionnaireResponse = parser.Parse<Hl7.Fhir.Model.QuestionnaireResponse>(json);

		// add questionnaire id to questionnaire object
		questionnaireResponse.Id =  Guid.Parse(fhirQuestionnaireResponse.Id);

		// to-do use separate identifiers fields
		//questionnaireResponse.Questionnaire.Id = fhirQuestionnaireResponse.Questionnaire.ToString().Replace("Questionnaire/", "");
		//questionnaireResponse.Participant.Id = fhirQuestionnaireResponse.Author.Reference.ToString().Replace("Patient/", "");
         

		// loop trough fhir questions instance to questionnaire
		foreach (var item in fhirQuestionnaireResponse.Item) {
			var qr = new QuestionResponse();
			qr.Id = Guid.Parse(item.LinkId);

			// parse responses
			foreach (var aResponse in item.Answer) {
				// note we all store in string now
				// if (aResponse.Value.GetType() == typeof(Hl7.Fhir.Model.Coding)) {
				// 	var aResponseCoding = (Hl7.Fhir.Model.Coding)aResponse.Value;
				// 	// todo Fix ask Thomas
				// 	//qr.ChoiceOptionIds.Add(aResponseCoding.Code);
				// }
				if (aResponse.Value.GetType() == typeof(Hl7.Fhir.Model.FhirString)) {
					qr.Answer = aResponse.Value.ToString();
				}

			}
			// add response to questionnaire response
			questionnaireResponse.Responses.Add(qr);
		}
		return questionnaireResponse;
	}

	public static string ToJson ( QuestionnaireResponse questionnaireResponse )
	{ 
		// create fhir questionnaire response
		var fhirQuestionnaireResponse = new Hl7.Fhir.Model.QuestionnaireResponse();

		//// to-do use separate identifiers fields
		//fhirQuestionnaireResponse.Questionnaire = "Questionnaire/" + questionnaireResponse;
		//fhirQuestionnaireResponse.Author = new Hl7.Fhir.Model.ResourceReference("Patient/" + questionnaireResponse.ParticipantId);
          
		foreach (var questionReponse in questionnaireResponse.Responses) {
			// create and fill response
			var item = new Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent();
			item.LinkId = questionReponse.Id.ToString();
			var answer = new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent();
			answer.Value = new Hl7.Fhir.Model.FhirString(questionReponse.Answer);
			item.Answer.Add(answer);
			fhirQuestionnaireResponse.Item.Add(item);

			// parse selected options


			// foreach (var aOptionId in questionReponse.ChoiceOptionIds)
			// {
			// 	// create questionnaire fhir option type object and codeValue object
			// 	var answerOption = new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent();
			// 	var answerOptionCoding = new Hl7.Fhir.Model.Coding();
			// 	// add questionOption coding
			// 	answerOptionCoding.Code = aOptionId;
			// 	answerOptionCoding.Display = "selected";
			// 	answerOption.Value = answerOptionCoding;

			// 	// add answeroptions to answer fhir item
			// 	item.Answer.Add(answerOption);
			// }
		}

		// serialize and return FHIR object
		var serializer = new FhirJsonSerializer();

		return serializer.SerializeToString(fhirQuestionnaireResponse);
	}
}
