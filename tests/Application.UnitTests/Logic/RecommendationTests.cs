using NUnit.Framework;
using Sonuts.Application.Logic.Categories.Queries;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.UnitTests.Logic
{
	public class ActivityRecommendationTests
	{
		[Test]
		public void IsRecommendedThemeCalcuationTest_Balance_Exercises()
		{
			//Arrange
			var question = new Question
			{
				Type = QuestionType.Integer,
				Text = "Number of days per week balancing exercises",
				Description = null,
				Information = null,
				Order = 29,
				EnableWhen = null,
				AnswerOptions = null,
				OpenAnswerLabel = null,
				IsRequired = true,
				Min = 0,
				Max = 7
			};

			RecommendationRule balanceExercises = new RecommendationRule
			{
				Type = RecommendationRuleType.All,
				Operator = Operator.GreaterOrEquals,
				Value = "2",
			};

			balanceExercises.Questions.Add(question);

			RecommendationRule[] ruleArray = new RecommendationRule[1];
			ruleArray[0] = balanceExercises;

			var questionResponse = CreateQuestionResponseForBalancingExercises(question);
			QuestionResponse[] questionResponses = new QuestionResponse[1];
			questionResponses[0] = questionResponse;

			//Act
			GetCategoriesQueryHandler getCategoriesQueryHandler = new GetCategoriesQueryHandler();
			var actual = getCategoriesQueryHandler.IsRecommendedThemeCalculation(questionResponses, ruleArray);

			//Assert
			var expected = false;
			Assert.AreEqual(expected, actual);
		}

		private QuestionResponse CreateQuestionResponseForBalancingExercises(Question question)
		{
			var participant = new Participant
			{
				FirstName = "Jane",
				LastName = "Doe",
				Birth = new DateOnly(year: 1958, month: 10, day: 20),
				Gender = "Female",
				Weight = 76,
				Height = 178,
				MaritalStatus = "Married",
				IsActive = true
			};

			var questionnaire = new Questionnaire
			{
				Title = "Questions about exercise",
				Description = "Questions about exercise"
			};
			questionnaire.Questions.Add(question);

			var response = new QuestionResponse
			{
				Question = question,
				Answer = "2"
			};

			var questionnaireResponse = new QuestionnaireResponse
			{
				CreatedAt = new DateOnly(year: 2023, month: 07, day: 14),
				Questionnaire = questionnaire,
				Participant = participant,
			};
			questionnaireResponse.Responses.Add(response);
			response.QuestionnaireResponse = questionnaireResponse;
			return response;
		}

		[Test]
		public void IsRecommendedThemeCalcuationTest_Bone_Exercises()
		{
			//Arrange
			var question = new Question
			{
				Type = QuestionType.Integer,
				Text = "Number of days per week bone-strengthening exercises",
				Description = null,
				Information = null,
				Order = 35,
				EnableWhen = null,
				AnswerOptions = null,
				OpenAnswerLabel = null,
				IsRequired = true,
				Min = 0,
				Max = 7
			};

			RecommendationRule boneExercises = new RecommendationRule
			{
				Type = RecommendationRuleType.All,
				Operator = Operator.GreaterOrEquals,
				Value = "2",
			};

			boneExercises.Questions.Add(question);

			RecommendationRule[] ruleArray = new RecommendationRule[1];
			ruleArray[0] = boneExercises;

			var questionResponse = CreateQuestionResponseForBoneExercises(question);
			QuestionResponse[] questionResponses = new QuestionResponse[1];
			questionResponses[0] = questionResponse;

			//Act
			GetCategoriesQueryHandler getCategoriesQueryHandler = new GetCategoriesQueryHandler();
			var actual = getCategoriesQueryHandler.IsRecommendedThemeCalculation(questionResponses, ruleArray);

			//Assert
			var expected = true;
			Assert.AreEqual(expected, actual);
		}

		private QuestionResponse CreateQuestionResponseForBoneExercises(Question question)
		{

			var participant = new Participant
			{
				FirstName = "Jane",
				LastName = "Doe",
				Birth = new DateOnly(year: 1958, month: 10, day: 20),
				Gender = "Female",
				Weight = 76,
				Height = 178,
				MaritalStatus = "Married",
				IsActive = true
			};

			var questionnaire = new Questionnaire
			{
				Title = "Questions about exercise",
				Description = "Questions about exercise"
			};
			questionnaire.Questions.Add(question);

			var response = new QuestionResponse
			{
				Question = question,
				Answer = "1"
			};

			var questionnaireResponse = new QuestionnaireResponse
			{
				CreatedAt = new DateOnly(year: 2023, month: 07, day: 14),
				Questionnaire = questionnaire,
				Participant = participant,
			};
			questionnaireResponse.Responses.Add(response);
			response.QuestionnaireResponse = questionnaireResponse;
			return response;
		}

		[Test]
		public void IsRecommendedThemeCalcuationTest_Muscle_Exercises()
		{
			//Arrange
			var question = new Question
			{
				Type = QuestionType.Integer,
				Text = "Number of days per week muscle-strengthening exercises",
				Description = null,
				Information = null,
				Order = 32,
				EnableWhen = null,
				AnswerOptions = null,
				OpenAnswerLabel = null,
				IsRequired = true,
				Min = 0,
				Max = 7
			};

			RecommendationRule muscleExercises = new RecommendationRule
			{
				Type = RecommendationRuleType.All,
				Operator = Operator.GreaterOrEquals,
				Value = "2",
			};

			muscleExercises.Questions.Add(question);

			RecommendationRule[] ruleArray = new RecommendationRule[1];
			ruleArray[0] = muscleExercises;

			var questionResponse = CreateQuestionResponseForMuscleExercises(question);
			QuestionResponse[] questionResponses = new QuestionResponse[1];
			questionResponses[0] = questionResponse;

			//Act
			GetCategoriesQueryHandler getCategoriesQueryHandler = new GetCategoriesQueryHandler();
			var actual = getCategoriesQueryHandler.IsRecommendedThemeCalculation(questionResponses, ruleArray);

			//Assert
			var expected = false;
			Assert.AreEqual(expected, actual);
		}

		private QuestionResponse CreateQuestionResponseForMuscleExercises(Question question)
		{

			var participant = new Participant
			{
				FirstName = "Jane",
				LastName = "Doe",
				Birth = new DateOnly(year: 1958, month: 10, day: 20),
				Gender = "Female",
				Weight = 76,
				Height = 178,
				MaritalStatus = "Married",
				IsActive = true
			};

			var questionnaire = new Questionnaire
			{
				Title = "Questions about exercise",
				Description = "Questions about exercise"
			};
			questionnaire.Questions.Add(question);

			var response = new QuestionResponse
			{
				Question = question,
				Answer = "6"
			};

			var questionnaireResponse = new QuestionnaireResponse
			{
				CreatedAt = new DateOnly(year: 2023, month: 07, day: 14),
				Questionnaire = questionnaire,
				Participant = participant,
			};
			questionnaireResponse.Responses.Add(response);
			response.QuestionnaireResponse = questionnaireResponse;
			return response;
		}
	}
}

