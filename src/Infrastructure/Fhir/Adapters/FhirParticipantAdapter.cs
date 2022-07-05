using Hl7.Fhir.Serialization;
using Sonuts.Infrastructure.Fhir.Models;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirParticipantAdapter
{
	// public static Questionnaire FromJson (string json)
	// { 
	// 	// create questionnaire instance
	// 	Questionnaire questionnaire = new Questionnaire();
         
        
	// 	var parser = new FhirJsonParser();
	// 	var fhirQuestionnaire = parser.Parse<Hl7.Fhir.Model.Questionnaire>(json);

	// 	// add questionnaire id to questionnaire object
	// 	questionnaire.Id =  fhirQuestionnaire.Id;

	// 	// loop trough fhir questions instance to questionnaire
	// 	foreach (var item in fhirQuestionnaire.Item) {
	// 		var oq = new Question();
	// 		oq.Id = item.LinkId;
	// 		oq.Text = item.Text;
	// 		oq.Type = item.TypeElement.ToString();
	// 		questionnaire.Questions.Add(oq);
	// 	}

	// 	return questionnaire;
	// }

	// public static string ToJson ( Questionnaire questionnaire )
	// { 
 
	// 	return "";
	// }
}
