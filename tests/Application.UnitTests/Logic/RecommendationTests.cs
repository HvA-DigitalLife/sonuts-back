using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Logic.Categories.Queries;
using Sonuts.Domain.Common;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;
using Sonuts.Infrastructure.Persistence;

namespace Sonuts.Application.UnitTests.Logic
{
	public class RecommendationTests
	{
		public RecommendationTests()
		{
		}

		[SetUp]
		public void Init()
		{
		}

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

			var questionResponse = CreateQuestionResponseForBalancingExercises();
			QuestionResponse[] questionResponses = new QuestionResponse[1];
			questionResponses[0] = questionResponse;

			var expected = true;
			//Act

			GetCategoriesQueryHandler getCategoriesQueryHandler = new GetCategoriesQueryHandler();
			var actual = getCategoriesQueryHandler.IsRecommendedThemeCalculation(questionResponses, ruleArray);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		public ApplicationDbContext GetMemoryContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "InMemoryDatabase")
			.Options;
			return new ApplicationDbContext(options);
		}

		private QuestionResponse CreateQuestionResponseForBalancingExercises()
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

	}
	}

