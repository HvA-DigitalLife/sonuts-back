using System.Globalization;
using CsvHelper.Configuration;
using Sonuts.Application.Logic.Participants.Queries;

namespace Sonuts.Infrastructure.Files.Maps;

public sealed class ParticipantMap : ClassMap<OverviewParticipantDto>
{
	public ParticipantMap()
	{
		AutoMap(CultureInfo.InvariantCulture);
	}
}
