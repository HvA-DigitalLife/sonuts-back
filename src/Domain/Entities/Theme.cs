namespace Sonuts.Domain.Entities;

public class Theme : BaseEntity
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public Category Category { get; set; } = default!;
	public Image Image { get; set; } = default!;
	public FrequencyType FrequencyType { get; set; } = default!;
	public int FrequencyGoal { get; set; } = default!;
	public string CurrentQuestion { get; set; } = default!;
	public string GoalQuestion { get; set; } = default!;
	public QuestionDependency? QuestionDependency { get; set; }
	public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
