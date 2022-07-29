using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal class ContentSeed
{
	internal static async Task Seed(IApplicationDbContext context)
	{
		var shouldSave = false;
		
		if (!await context.Content.AnyAsync())
		{
			shouldSave = true;

			context.Content.Add(new Content
			{
				Type = ContentType.Introduction,
				Title = "Welkom {name}",
				Subtitle = "Uitleg",
				Description = "Welkom bij de applicatie van Mensen in Beweging!\r\n\r\nDeze applicatie heeft als doel je te ondersteunen om gezonder te gaan eten en meer te gaan bewegen!"
			});
			context.Content.Add(new Content
			{
				Type = ContentType.Intake,
				Title = "Intake",
				Subtitle = "Uitleg",
				Description = "De intake bestaat uit drie onderdelen. In totaal ben je ongeveer 30 minuten bezig met de intake. Er zijn vragen over je persoonlijkheid, voeding en beweging. Na deze intake wordt er samen met jou een plan  gemaakt."
			});
			context.Content.Add(new Content
			{
				Type = ContentType.Themes,
				Title = "Intake",
				Subtitle = "Uitleg",
				Description = "De intake bestaat uit drie onderdelen. In totaal ben je ongeveer 30 minuten bezig met de intake. Er zijn vragen over je persoonlijkheid, voeding en beweging. Na deze intake wordt er samen met jou een plan  gemaakt."
			});
			context.Content.Add(new Content
			{
				Type = ContentType.ThemeChoice,
				Title = "Hartelijk dank!",
				Subtitle = "Keuze thema’s",
				Description = "Zowel voor het onderdeel bewegen als voeding krijg je te zien op welke thema’s je al aan de richtlijn voldoet en waar nog ruimte is voor verbetering. Kies op de volgende pagina 3 themas waar je aan zou willen werken de komende tijd."
			});
			context.Content.Add(new Content
			{
				Type = ContentType.Schedule,
				Title = "Planning",
				Subtitle = "Geplande activiteiten",
				Description = "Op de volgende pagina zie je welke activiteiten je voor komende week hebt gepland om je doelen."
			});
		}

		if (shouldSave) await context.SaveChangesAsync();
	}
}
