namespace Sonuts.Infrastructure.Fhir.Models;

public class Activity
{

	public string? Id { get; set; }


	public string? DomainId { get; set; }

	public string? GuidelineId { get; set; }

	public string? GoalId { get; set; }

	public string? Description { get; set; }

	public string? Date { get; set; }

	public string? ReminderDate { get; set; }

	/// <summary>
	/// List of dynamic inputfields
	/// </summary> 
	public IEnumerable<ActivityDataValue> DataValues { get; set; } = new List<ActivityDataValue>();   
}
