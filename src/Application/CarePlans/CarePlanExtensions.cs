using Microsoft.EntityFrameworkCore;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.CarePlans;

internal static class CarePlanExtensions
{
	/// <param name="carePlans"></param>
	/// <param name="participantId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>Current care plan</returns>
	internal static async Task<CarePlan?> Current(this DbSet<CarePlan> carePlans, Guid participantId, CancellationToken cancellationToken = default) =>
		await carePlans.OrderByDescending(carePlan => carePlan.Start)
			.FirstOrDefaultAsync(carePlan => carePlan.Participant.Id.Equals(participantId), cancellationToken);
}
