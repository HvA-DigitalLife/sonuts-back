namespace Sonuts.Infrastructure.Fhir.Models;

public class Goal
{
	public string? Id { get; set; }

	public string? Title { get; set; }

	public string? Text { get; set; }

	public List<GoalDataField> DataFields { get; set; } = new List<GoalDataField>();

}
