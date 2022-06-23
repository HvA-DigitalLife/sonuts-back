using Hl7.Fhir.Serialization;
using Sonuts.Infrastructure.Fhir.Models;

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
		questionnaireResponse.Id =  fhirQuestionnaireResponse.Id;
		questionnaireResponse.QuestionnaireId = fhirQuestionnaireResponse.Questionnaire.ToString().Replace("Questionnaire/", "");
		questionnaireResponse.ParticipantId= fhirQuestionnaireResponse.Author.Reference.ToString().Replace("Patient/", "");
         

		// loop trough fhir questions instance to questionnaire
		foreach (var item in fhirQuestionnaireResponse.Item) {
			var oq = new QuestionResponse();
			oq.QuestionId = item.LinkId;

			// parse responses
			foreach (var aResponse in item.Answer) {
				if (aResponse.Value.GetType() == typeof(Hl7.Fhir.Model.Coding)) {
					var aResponseCoding = (Hl7.Fhir.Model.Coding)aResponse.Value;
					oq.ChoiceOptionIds.Add(aResponseCoding.Code);
				}
				if (aResponse.Value.GetType() == typeof(Hl7.Fhir.Model.FhirString)) {
					oq.Response = aResponse.Value.ToString();
				}

			}
			// add response to questionnaire response
			questionnaireResponse.QuestionResponses.Add(oq);
		}
		return questionnaireResponse;
	}

	public static string ToJson ( QuestionnaireResponse questionnaireResponse )
	{ 
		// create fhir questionnaire response
		var fhirQuestionnaireResponse = new Hl7.Fhir.Model.QuestionnaireResponse();
		fhirQuestionnaireResponse.Questionnaire = "Questionnaire/" + questionnaireResponse.QuestionnaireId;
		fhirQuestionnaireResponse.Author = new Hl7.Fhir.Model.ResourceReference("Patient/" + questionnaireResponse.ParticipantId);
          
		foreach (var questionReponse in questionnaireResponse.QuestionResponses) {
			// create and fill response
			var item = new Hl7.Fhir.Model.QuestionnaireResponse.ItemComponent();
			item.LinkId = questionReponse.QuestionId;
			var answer = new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent();
			answer.Value = new Hl7.Fhir.Model.FhirString(questionReponse.Response);
			item.Answer.Add(answer);
			fhirQuestionnaireResponse.Item.Add(item);

			// parse selected options
			foreach (var aOptionId in questionReponse.ChoiceOptionIds)
			{
				// create questionnaire fhir option type object and codeValue object
				var answerOption = new Hl7.Fhir.Model.QuestionnaireResponse.AnswerComponent();
				var answerOptionCoding = new Hl7.Fhir.Model.Coding();
				// add questionOption coding
				answerOptionCoding.Code = aOptionId;
				answerOptionCoding.Display = "selected";
				answerOption.Value = answerOptionCoding;

				// add answeroptions to answer fhir item
				item.Answer.Add(answerOption);
			}
		}

		// serialize and return FHIR object
		var serializer = new FhirJsonSerializer();

		return serializer.SerializeToString(fhirQuestionnaireResponse);
	}
}
