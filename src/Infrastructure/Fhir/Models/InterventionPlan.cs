namespace Sonuts.Infrastructure.Fhir.Models;

public class InterventionPlan
{

	public string? Id { get; set; }

	public string? ParticipantId { get; set; }

	public string StartDate { get; set; } = default!;

	public string EndDate { get; set; } = default!;

	public IEnumerable<string> RecommendedGuidelines { get; set; } = new List<string>();

	public IEnumerable<string> SelectedGuidelines { get; set; } = new List<string>();

	public IEnumerable<Activity> CalendarTasks { get; set; } = new List<Activity>();
}
