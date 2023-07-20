using CsvHelper.Configuration;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Files.Maps;

public sealed class ExecutionMap : ClassMap<Execution>
{
	public ExecutionMap()
	{
		Map(e => e.Goal.CarePlan.Participant.Id)
			.Name($"{nameof(Participant)}.{nameof(Participant.Id)}")
			.Index(0);

		Map(e => e.IsDone)
			.Convert(c => c.Value.IsDone ? "Yes" : "No")
			.Index(1);

		Map(e => e.Amount)
			.Convert(e => $"{e.Value.Amount}%")
			.Index(2);

		Map(e => e.Reason)
			.Index(3);

		Map(e => e.CreatedAt)
			.Index(4);

		Map(e => e.Goal.Moment.Day)
			.Name("Planned Day")
			.Index(5);

		Map(e => e.Goal.Activity.Theme.Id)
			.Name($"{nameof(Theme)}.{nameof(Theme.Id)}")
			.Index(6);

		Map(e => e.Goal.Activity.Theme.Name)
			.Name($"{nameof(Theme)}.{nameof(Theme.Name)}")
			.Index(7);

		Map(e => e.Goal.Activity.Id)
			.Name($"{nameof(Activity)}.{nameof(Activity.Id)}")
			.Index(8);

		Map(e => e.Goal.Activity.Name)
			.Name($"{nameof(Activity)}.{nameof(Activity.Name)}")
			.Index(9);
	}
}
