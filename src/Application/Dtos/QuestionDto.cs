using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class QuestionDto : IMapFrom<Question>
{
	public QuestionType Type { get; set; } = default!;
	public string Text { get; set; } = default!;
	public string? Description { get; set; }
	public int Order { get; set; } = default!;
	public int? MaxAnswers { get; set; }
	public QuestionDependencyDto? QuestionDependency { get; set; }
	public ICollection<AnswerOptionDto> AnswerOptions { get; set; } = new List<AnswerOptionDto>();
}