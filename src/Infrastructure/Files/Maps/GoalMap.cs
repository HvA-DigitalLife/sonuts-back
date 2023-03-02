using CsvHelper.Configuration;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Files.Maps;

public sealed class GoalMap : ClassMap<Goal>
{
	public GoalMap()
	{
		Map(g => g.CarePlan.Participant.Id)
			.Name($"{nameof(Participant)}.{nameof(Participant.Id)}")
			.Index(0);

		Map(g => g.CarePlan.Id)
			.Name($"{nameof(CarePlan)}.{nameof(CarePlan.Id)}")
			.Index(1);

		Map(g => g.CarePlan.Start)
			.Name($"{nameof(CarePlan)}.{nameof(CarePlan.Start)}")
			.Index(2);

		Map(g => g.CarePlan.End)
			.Name($"{nameof(CarePlan)}.{nameof(CarePlan.End)}")
			.Index(3);

		Map(g => g.Activity.Id)
			.Name($"{nameof(Activity)}.{nameof(Activity.Id)}")
			.Index(4);

		Map(g => g.Activity.Name)
			.Name($"{nameof(Activity)}.{nameof(Activity.Name)}")
			.Index(5);

		Map(g => g.CustomName)
			.Index(6);

		Map(g => g.Moment.EventName)
			.Name($"{nameof(Moment)}.{nameof(Moment.EventName)}")
			.Index(7);

		Map(g => g.Moment.Day)
			.Name($"{nameof(Moment)}.{nameof(Moment.Day)}")
			.Index(8);

		Map(g => g.Moment.Type)
			.Name($"{nameof(Moment)}.{nameof(Moment.Type)}")
			.Index(9);

		Map(g => g.Moment.Time)
			.Name($"{nameof(Moment)}.{nameof(Moment.Time)}")
			.Index(10);

		Map(g => g.FrequencyAmount)
			.Index(11);

		Map(g => g.Reminder)
			.Index(12);
	}
}
