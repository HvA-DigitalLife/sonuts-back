using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal static class MotivationalMessageSeed
{
	internal static async Task Seed(IApplicationDbContext context)
	{
		var shouldSave = false;

		if (!await context.MotivationalMessages.AnyAsync())
		{
			shouldSave = true;

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Oefening baart kunst!",
				MinPercentage = 0,
				MaxPercentage = 50
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Durf! Zet je spullen vast klaar!",
				MinPercentage = 0,
				MaxPercentage = 50
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Pak de draad weer op!",
				MinPercentage = 0,
				MaxPercentage = 50
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Doen geeft voldoening!",
				MinPercentage = 0,
				MaxPercentage = 50
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Morgen een nieuwe kans!",
				MinPercentage = 0,
				MaxPercentage = 50
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Leren is vallen en opstaan! ",
				MinPercentage = 0,
				MaxPercentage = 50
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Tip:  Geef niet op. Jij kunt het ook!",
				MinPercentage = 0,
				MaxPercentage = 0
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Knap!",
				MinPercentage = 51,
				MaxPercentage = 100
			});
			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Super!",
				MinPercentage = 51,
				MaxPercentage = 100
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Goed gedaan!",
				MinPercentage = 51,
				MaxPercentage = 100
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Ga zo door!",
				MinPercentage = 51,
				MaxPercentage = 100
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Top!",
				MinPercentage = 51,
				MaxPercentage = 100
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Applaus voor jezelf!",
				MinPercentage = 51,
				MaxPercentage = 100
			});

			context.MotivationalMessages.Add(new MotivationalMessage
			{
				Message = "Geef jezelf een compliment!",
				MinPercentage = 51,
				MaxPercentage = 100
			});
		}

		if (shouldSave)
			await context.SaveChangesAsync();
	}
}
