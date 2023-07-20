using System.Globalization;
using CsvHelper.Configuration;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Files.Maps;

public sealed class ParticipantMap : ClassMap<Participant>
{
	public ParticipantMap()
	{
		AutoMap(CultureInfo.InvariantCulture);
	}
}
