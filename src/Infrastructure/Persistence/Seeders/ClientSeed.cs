using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Persistence.Seeders;

internal static class ClientSeed
{
	internal static async Task Seed(IApplicationDbContext context)
	{
		var shouldSave = false;

		if (await context.Clients.FirstOrDefaultAsync(client => client.Id.Equals(Guid.Parse("05004bd2-18d9-402f-9a1b-673fcf1d46e7"))) is null)
		{
			shouldSave = true;

			context.Clients.Add(new Client
			{
				Id = Guid.Parse("05004bd2-18d9-402f-9a1b-673fcf1d46e7"),
				Secret = "sonuts",
				Name = "Mobile App"
			});
		}

		if (shouldSave) await context.SaveChangesAsync();
	}
}
