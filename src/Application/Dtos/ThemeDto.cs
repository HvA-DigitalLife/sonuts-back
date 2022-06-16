using AutoMapper;
using Sonuts.Application.Categories;
using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class ThemeDto : IMapFrom<Theme>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public CategoryDto Category { get; set; } = default!;
	public ImageDto Image { get; set; } = default!;
	public FrequencyType FrequencyType { get; set; } = default!;
	public int FrequencyGoal { get; set; } = default!;
	public string CurrentQuestion { get; set; } = default!;
	public string GoalQuestion { get; set; } = default!;
	public bool IsRecommend { get; set; }
	public ICollection<ActivityDto> Activities { get; set; } = new List<ActivityDto>();

	public void Mapping(Profile profile) =>
		profile.CreateMap<Theme, ThemeDto>()
			.ForMember(themeDto => themeDto.IsRecommend, expression => expression
				.MapFrom(theme => theme.QuestionDependency != null && theme.QuestionDependency.Operator == Operator.Equals));
}
