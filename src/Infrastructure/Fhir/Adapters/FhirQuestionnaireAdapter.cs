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

		foreach (var id in fhirQuestionnaire.Identifier) {
			if (id.System == "https://mibplatform.nl/fhir/mib/identifier") {
				questionnaire.Id =  Guid.Parse(id.Value);
			}
		}
		


		// to-do adding question dependency tag



		

		// loop trough fhir questions instance to questionnaire
		foreach (var item in fhirQuestionnaire.Item)
		{
			var multi = true;
			var question = new Question
			{
				Id = Guid.Parse(item.LinkId),
				Text = item.Text
			};

			foreach (var enableWhen in item.EnableWhen) {
				question.EnableWhen = new EnableWhen();
	
				question.EnableWhen.Operator = enableWhen.Operator.Value switch
				{
					// set question type
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.Equal => Operator.Equals,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.GreaterOrEqual => Operator.GreaterOrEquals,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.GreaterThan => Operator.GreaterThan,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.LessOrEqual => Operator.LessOrEquals,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.LessThan => Operator.LessThan,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.NotEqual => Operator.NotEquals,
					_ => question.EnableWhen.Operator
				};
						

				question.EnableWhen = new EnableWhen {
					QuestionId = Guid.Parse(enableWhen.Question),
					//Operator
					Answer = enableWhen.Answer.ToString()
				};
			}

			// if we have a multiple choice option we need to loop trough all the options
			if ((item.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice) || (item.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice))
			{

				foreach (var itemExtension in item.Extension)
				{
					
					// to be replaced by question dependency?

					// // check if we have the openLabel extension (for openchoice question answers)
					// if (itemExtension.Url == "http://hl7.org/fhir/uv/sdc/StructureDefinition/sdc-questionnaire-openLabel")
					// {
					// 	question.Text = itemExtension.Value.ToString()!; //What is OpenLable
					// }
					// check if we have the single option flag set
					if ((itemExtension.Url == "http://hl7.org/fhir/StructureDefinition/questionnaire-optionExclusive") && (itemExtension.Value.Equals(true)))
					{
						multi = false;
					}
				}

				question.Type = item.Type switch
				{
					// set question type
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice => QuestionType.MultiChoice,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice => QuestionType.MultiOpenChoice,
					_ => question.Type
				};

				foreach (var answerOption in item.AnswerOption)
				{
					// create option object
					var qaOption = new AnswerOption();
					// parse coding of the fhir answer option object and store in our own option object
					var answerOptionCoding = (Hl7.Fhir.Model.Coding)answerOption.Value;
					qaOption.Id = Guid.Parse(answerOptionCoding.Code);
					qaOption.Value = answerOptionCoding.Display;

					foreach (var answerOptionExtension in answerOption.Extension)
					{
						
						// to be replaced by question dependency?

						// // check if we have the openLabel extension (for openchoice question answers)
						// if (itemExtension.Url == "http://hl7.org/fhir/uv/sdc/StructureDefinition/sdc-questionnaire-openLabel")
						// {
						// 	question.Text = itemExtension.Value.ToString()!; //What is OpenLable
						// }



						// check if we have the single option flag set
						if (answerOptionExtension.Url == "https://mibplatform.nl/fhir/extensions/Questionnaire/answer-option-order")
						{
							// @thomaslem how best to do this?
							// i want to convert from   (Hl7.Fhir.Model.Integer) answerOptionExtension.Value  to int
							qaOption.Order = int.Parse(answerOptionExtension.Value.ToString());
						}
					}


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

		// add identifier
		fhirQuestionnaire.Identifier.Add(new Hl7.Fhir.Model.Identifier {
				System = "https://mibplatform.nl/fhir/mib/identifier",
				Value = questionnaire.Id.ToString()
			});

		

		foreach (var question in questionnaire.Questions)
		{
			// create fhir question item for each question
			var item = new Hl7.Fhir.Model.Questionnaire.ItemComponent
			{
				LinkId = question.Id.ToString(),
				Text = question.Text
			};


			// todo add operator conversion

			if (question.EnableWhen is not null) {
				

				var enableWhen = new Hl7.Fhir.Model.Questionnaire.EnableWhenComponent {
					Question=question.EnableWhen.QuestionId.ToString(),
					Answer=new Hl7.Fhir.Model.FhirString(question.EnableWhen.Answer) // multiple types?
				};
				enableWhen.Operator = question.EnableWhen.Operator switch
				{
					// set question type
					Operator.Equals  => Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.Equal,
					Operator.GreaterOrEquals => Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.GreaterOrEqual,
					Operator.GreaterThan => Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.GreaterThan,
					Operator.LessOrEquals => Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.LessOrEqual,
					Operator.LessThan => Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.LessThan,
					Operator.NotEquals => Hl7.Fhir.Model.Questionnaire.QuestionnaireItemOperator.NotEqual,
					_ => enableWhen.Operator
				};
			}






			// string based question
			if (question.Type == QuestionType.String)
			{
				// add string reply option to question item
				item.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.String;
			}

/*


TODO, add extra question types

*/


			// create multiple choices for question item object
			if (question.Type is QuestionType.MultiOpenChoice or QuestionType.MultiChoice or QuestionType.Choice or QuestionType.OpenChoice)
			{




				if (question.Type is QuestionType.Choice or QuestionType.MultiChoice)
				{
					item.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice;
				}
				if (question.Type is QuestionType.OpenChoice or QuestionType.MultiOpenChoice)
				{
					item.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice;
					// add open-choice label extension when open-choice is selected
					
					
					// to be replaced by question dependency?

					// var answerOptionTypeExtension = new Hl7.Fhir.Model.Extension
					// 	{
					// 		Url = "http://hl7.org/fhir/uv/sdc/StructureDefinition/sdc-questionnaire-openLabel",
					// 		Value = new Hl7.Fhir.Model.FhirString(question.Text) //.OpenLabel)
					// 	};
					// item.Extension.Add(answerOptionTypeExtension);


				}
				
				if (question.Type is QuestionType.Choice or QuestionType.OpenChoice)
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
							Display = qaOption.Value
						};
						answerOption.Value = answerOptionCoding;
						

						// answerOrder extension
						// to-do: check if there is something like this already within SDC
						// title extension
						var answerOptionOrderExtension = new Hl7.Fhir.Model.Extension() { 
							Url = "https://mibplatform.nl/fhir/extensions/Questionnaire/answer-option-order",
							Value = new Hl7.Fhir.Model.Integer(qaOption.Order)
						};
						answerOption.Extension.Add(answerOptionOrderExtension);


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
