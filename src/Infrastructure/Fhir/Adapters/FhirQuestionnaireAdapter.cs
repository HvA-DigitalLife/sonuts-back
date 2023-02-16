using Hl7.Fhir.Serialization;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Fhir.Adapters;

public static class FhirQuestionnaireAdapter
{
	public static Questionnaire FromJson(string json)
	{
		// create fhir parser
		var fhirJsonParser = new FhirJsonParser();
		var fhirQuestionnaire = fhirJsonParser.Parse<Hl7.Fhir.Model.Questionnaire>(json);

		// create questionnaire instance
		Questionnaire questionnaire = new Questionnaire{
			Id = Guid.Parse(fhirQuestionnaire.Id),
			Title = fhirQuestionnaire.Title,
			Description = fhirQuestionnaire.Description.ToString()
		};

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
				Text = fhirItem.Text,
				Order = 0,
				Type = QuestionType.MultiChoice

			};

			foreach (var fhirItemExtension in fhirItem.Extension)
			{
				
				if ((fhirItemExtension.Url == "https://mibplatform.nl/fhir/extensions/Questionnaire/answerOrder"))
				{
					question.Order = int.Parse(fhirItemExtension.Value.ToString() ?? "0");
				}
			}

			foreach (var fhirEnableWhen in fhirItem.EnableWhen) {	

				question.EnableWhen = new EnableWhen {
					DependentQuestionId = Guid.Parse(fhirEnableWhen.Question),
					Answer = fhirEnableWhen.Answer.ToString() ?? "",
					Operator = Operator.Equals
				};
				if (fhirEnableWhen.Operator is not null) {
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
			}


			// set question types
			if (fhirItem.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.String)
				question.Type = QuestionType.String;

			if (fhirItem.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Boolean)
				question.Type = QuestionType.Boolean;

			if (fhirItem.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Integer)
				question.Type = QuestionType.Integer;
			
			if (fhirItem.Type == Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Decimal)
				question.Type = QuestionType.Decimal;

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
					var fhirAnswerOptionCoding = (Hl7.Fhir.Model.Coding)fhirAnswerOption.Value;
					// create option object
					var answerOption = new AnswerOption{
						Id = Guid.Parse(fhirAnswerOptionCoding.Code),
						Name = "", // to-do: implement
						Value = fhirAnswerOptionCoding.Display,
						Order = 0 // to-do: implement

					};
					// parse coding of the fhir answer option object and store in our own option object
					


					foreach (var fhirAnswerOptionExtension in fhirAnswerOption.Extension)
					{
						


						// check if we have the single option flag set
						if (fhirAnswerOptionExtension.Url == "https://mibplatform.nl/fhir/extensions/Questionnaire/answerOptionOrder")
						{
							// @thomaslem how best to do this?
							// i want to convert from   (Hl7.Fhir.Model.Integer) answerOptionExtension.Value  to int
							answerOption.Order = int.Parse(fhirAnswerOptionExtension.Value.ToString() ?? "0");
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
			Id = questionnaire.Id.ToString(),
			Title = questionnaire.Title,
			Description = new Hl7.Fhir.Model.Markdown(questionnaire.Description)
		};


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
				fhirItem.EnableWhen.Add(enableWhen);
			}


			// set question types
			if (question.Type == QuestionType.String)
				fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.String;

			if (question.Type == QuestionType.Boolean)
				fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Boolean;

			if (question.Type == QuestionType.Integer)
				fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Integer;
			
			if (question.Type == QuestionType.Decimal)
				fhirItem.Type = Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType.Decimal;

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
