using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirQuestionnaireAdapter
{
	public static Questionnaire FromJson(string json)
	{
		// create questionnaire instance
		Questionnaire questionnaire = new Questionnaire();

		// create fhir parser
		var parser = new FhirJsonParser();
		var fhirQuestionnaire = parser.Parse<Hl7.Fhir.Model.Questionnaire>(json);

		questionnaire.Title = fhirQuestionnaire.Title;
		questionnaire.Description = fhirQuestionnaire.Description.ToString();

		// add questionnaire id to questionnaire object
		questionnaire.Id = Guid.Parse(fhirQuestionnaire.Id);

		// loop trough fhir questions instance to questionnaire
		foreach (var item in fhirQuestionnaire.Item)
		{
			var multi = true;
			var question = new Question
			{
				Id = Guid.Parse(item.LinkId),
				Text = item.Text
			};

			// if we have a multiple choice option we need to loop trough all the options
			if ((item.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice) || (item.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice))
			{

				foreach (var itemExtension in item.Extension)
				{
					// check if we have the openLabel extension (for openchoice question answers)
					if (itemExtension.Url == "http://hl7.org/fhir/uv/sdc/StructureDefinition/sdc-questionnaire-openLabel")
					{
						question.Text = itemExtension.Value.ToString()!; //What is OpenLable
					}
					// check if we have the single option flag set
					if ((itemExtension.Url == "http://hl7.org/fhir/StructureDefinition/questionnaire-optionExclusive") && (itemExtension.Value.Equals(true)))
					{
						multi = false;
					}
				}

				question.Type = item.Type switch
				{
					// set question type
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice => QuestionType.MultipleChoice, // multi ? "multiChoice" : "choice",
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice => QuestionType.MultipleOpen, // multi ? "multiOpenChoice" : "openChoice",
					_ => question.Type
				};

				foreach (var answerOption in item.AnswerOption)
				{
					// create option object
					var qaOption = new AnswerOption();
					// parse coding of the fhir answer option object and store in our own option object
					var answerOptionCoding = (Hl7.Fhir.Model.Coding)answerOption.Value;
					qaOption.Id = Guid.Parse(answerOptionCoding.Code);
					qaOption.Text = answerOptionCoding.Display;
					question.AnswerOptions!.Add(qaOption);
				}

			}
			// add question to questionnaire
			questionnaire.Questions.Add(question);

		}
		return questionnaire;
	}

	public static string ToJson(Questionnaire questionnaire)
	{
		// create questionnaire fhir object
		var fhirQuestionnaire = new Hl7.Fhir.Model.Questionnaire
		{
			Title = questionnaire.Title,
			Description = new Hl7.Fhir.Model.Markdown(questionnaire.Description)
		};

		// loop trough all questions
		// TODO make recursive!
		foreach (var question in questionnaire.Questions)
		{
			// create fhir question item for each question
			var item = new Hl7.Fhir.Model.Questionnaire.ItemComponent
			{
				LinkId = question.Id.ToString(),
				Text = question.Text
			};
			// string based question
			if (question.Type == QuestionType.Open)
			{
				// add string reply option to question item
				item.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.String;
			}
			// create multiple choices for question item object
			if (question.Type is QuestionType.MultipleChoice or QuestionType.MultipleOpen) //(question.Type == "choice") || (question.Type == "openChoice") || (question.Type == "multiChoice") || (question.Type == "multiOpenChoice"))
			{

				if (question.Type is QuestionType.MultipleChoice) //(question.Type == "choice") || (question.Type == "multiChoice"))
				{
					item.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice;
				}
				if (question.Type is QuestionType.MultipleOpen) //(question.Type == "openChoice") || (question.Type == "multiOpenChoice"))
				{
					item.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice;
					// add open-choice label extension when open-choice is selected
					var answerOptionTypeExtension = new Hl7.Fhir.Model.Extension
						{
							Url = "http://hl7.org/fhir/uv/sdc/StructureDefinition/sdc-questionnaire-openLabel",
							Value = new Hl7.Fhir.Model.FhirString(question.Text) //.OpenLabel)
						};
					item.Extension.Add(answerOptionTypeExtension);
				}
				if (question.Type is QuestionType.MultipleChoice or QuestionType.MultipleOpen) //(question.Type == "choice") || (question.Type == "openChoice"))
				{
					// add single option choise extension no multi choice is selected
					var singleOptionTypeExtension = new Hl7.Fhir.Model.Extension
						{
							Url = "http://hl7.org/fhir/StructureDefinition/questionnaire-optionExclusive", Value = new Hl7.Fhir.Model.FhirBoolean(true)
						};
					item.Extension.Add(singleOptionTypeExtension);
				}

				if (question.AnswerOptions is not null)
				{
					foreach (var qaOption in question.AnswerOptions)
					{
						// create questionnaire fhir option type object and codeValue object
						var answerOption = new Hl7.Fhir.Model.Questionnaire.AnswerOptionComponent();
						var answerOptionCoding = new Hl7.Fhir.Model.Coding
						{
							// add questionOption coding
							Code = qaOption.Id.ToString(),
							Display = qaOption.Text
						};
						answerOption.Value = answerOptionCoding;
						//answerOption.InitialSelected = qaOption.Selected;

						// add answeroptions to answer fhir item
						item.AnswerOption.Add(answerOption);
					}
				}
			}
			// add question item to fhir questionnaire object
			fhirQuestionnaire.Item.Add(item);
		}
		// serialize and return fhir object
		var serializer = new FhirJsonSerializer();
		return serializer.SerializeToString(fhirQuestionnaire);
	}
}
