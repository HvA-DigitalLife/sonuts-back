namespace Sonuts.Domain.Entities;

public class Theme : BaseEntity //http://hl7.org/fhir/R4/plandefinition.html#PlanDefinition
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public Category Category { get; set; } = default!;
	public Image Image { get; set; } = default!;
	public FrequencyType FrequencyType { get; set; } = default!;
	public int FrequencyGoal { get; set; } = default!;
	public string CurrentQuestion { get; set; } = default!;
	public string GoalQuestion { get; set; } = default!;
	//public EnableWhen? QuestionDependency { get; set; } //TODO: recommendation
	public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
