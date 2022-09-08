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
		var fhirJsonParser = new FhirJsonParser();
		var fhirQuestionnaire = fhirJsonParser.Parse<Hl7.Fhir.Model.Questionnaire>(json);

		questionnaire.Title = fhirQuestionnaire.Title;
		questionnaire.Description = fhirQuestionnaire.Description.ToString();

		// add questionnaire id to questionnaire object

		foreach (var fhirId in fhirQuestionnaire.Identifier) {
			if (fhirId.System == "https://mibplatform.nl/fhir/mib/identifier") {
				questionnaire.Id =  Guid.Parse(fhirId.Value);
			}
		}
		

		// loop trough fhir questions instance to questionnaire
		foreach (var fhirItem in fhirQuestionnaire.Item)
		{
			var question = new Question
			{
				Id = Guid.Parse(fhirItem.LinkId),
				Text = fhirItem.Text
			};

			foreach (var fhirItemExtension in fhirItem.Extension)
			{
				
				if ((fhirItemExtension.Url == "https://mibplatform.nl/fhir/extensions/Questionnaire/answerOrder"))
				{
					question.Order = int.Parse(fhirItemExtension.Value.ToString());
				}
			}

			foreach (var fhirEnableWhen in fhirItem.EnableWhen) {	

				question.EnableWhen = new EnableWhen {
					DependentQuestionId = Guid.Parse(fhirEnableWhen.Question),
					Answer = fhirEnableWhen.Answer.ToString()
				};

				question.EnableWhen.Operator = fhirEnableWhen.Operator.Value switch
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
			}


			// if we have a multiple choice option we need to loop trough all the options
			if ((fhirItem.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice) || (fhirItem.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice))
			{

				question.Type = fhirItem.Type switch
				{
					// set question type
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice => QuestionType.MultiChoice,
					Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice => QuestionType.MultiOpenChoice,
					_ => question.Type
				};

				foreach (var fhirAnswerOption in fhirItem.AnswerOption)
				{
					// create option object
					var answerOption = new AnswerOption();
					// parse coding of the fhir answer option object and store in our own option object
					var fhirAnswerOptionCoding = (Hl7.Fhir.Model.Coding)fhirAnswerOption.Value;
					answerOption.Id = Guid.Parse(fhirAnswerOptionCoding.Code);
					answerOption.Value = fhirAnswerOptionCoding.Display;

					foreach (var fhirAnswerOptionExtension in fhirAnswerOption.Extension)
					{
						


						// check if we have the single option flag set
						if (fhirAnswerOptionExtension.Url == "https://mibplatform.nl/fhir/extensions/Questionnaire/answerOptionOrder")
						{
							// @thomaslem how best to do this?
							// i want to convert from   (Hl7.Fhir.Model.Integer) answerOptionExtension.Value  to int
							answerOption.Order = int.Parse(fhirAnswerOptionExtension.Value.ToString());
						}
					}


					question.AnswerOptions!.Add(answerOption);
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
			var fhirItem = new Hl7.Fhir.Model.Questionnaire.ItemComponent
			{
				LinkId = question.Id.ToString(),
				Text = question.Text
			};

			// add order
			fhirItem.Extension.Add(new Hl7.Fhir.Model.Extension() { 
				Url = "https://mibplatform.nl/fhir/extensions/Questionnaire/answerOrder",
				Value = new Hl7.Fhir.Model.Integer(question.Order)
			});

			if (question.EnableWhen is not null) {
				

				var enableWhen = new Hl7.Fhir.Model.Questionnaire.EnableWhenComponent {
					Question=question.EnableWhen.DependentQuestionId.ToString(),
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
				fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.String;
			}


			// create multiple choices for question item object
			if (question.Type is QuestionType.MultiOpenChoice or QuestionType.MultiChoice or QuestionType.Choice or QuestionType.OpenChoice)
			{


				if (question.Type is QuestionType.Choice or QuestionType.MultiChoice)
				{
					fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Choice;
				}
				if (question.Type is QuestionType.OpenChoice or QuestionType.MultiOpenChoice)
				{
					fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.OpenChoice;
				}
				
				if (question.Type is QuestionType.Choice or QuestionType.OpenChoice)
				{
					// add single option choise extension no multi choice is selected
					var singleOptionTypeExtension = new Hl7.Fhir.Model.Extension
						{
							Url = "http://hl7.org/fhir/StructureDefinition/questionnaire-optionExclusive", Value = new Hl7.Fhir.Model.FhirBoolean(true)
						};
					fhirItem.Extension.Add(singleOptionTypeExtension);
				}

				if (question.AnswerOptions is not null)
				{
					foreach (var answerOption in question.AnswerOptions)
					{
						// create questionnaire fhir option type object and codeValue object
						var fhirAnswerOption = new Hl7.Fhir.Model.Questionnaire.AnswerOptionComponent();
						var fhirAnswerOptionCoding = new Hl7.Fhir.Model.Coding
						{
							// add questionOption coding
							Code = answerOption.Id.ToString(),
							Display = answerOption.Value
						};
						fhirAnswerOption.Value = fhirAnswerOptionCoding;
						

						// answer option order
						fhirAnswerOption.Extension.Add(new Hl7.Fhir.Model.Extension() { 
							Url = "https://mibplatform.nl/fhir/extensions/Questionnaire/answerOptionOrder",
							Value = new Hl7.Fhir.Model.Integer(answerOption.Order)
						});


						// add answeroptions to answer fhir item
						fhirItem.AnswerOption.Add(fhirAnswerOption);
					}
				}
			}
			// add question item to fhir questionnaire object
			fhirQuestionnaire.Item.Add(fhirItem);
		}
		// serialize and return fhir object
		var fhirJsonSerializer = new FhirJsonSerializer();
		return fhirJsonSerializer.SerializeToString(fhirQuestionnaire);
	}
}
