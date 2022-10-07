using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class QuestionDto : IMapFrom<Question>
{
	public Guid Id { get; set; }
	public QuestionType Type { get; set; } = default!;
	public string Text { get; set; } = default!;
	public string? Description { get; set; }
	public int Order { get; set; } = default!;
	public EnableWhenDto? EnableWhen { get; set; }
	public ICollection<AnswerOptionDto>? AnswerOptions { get; set; } = new List<AnswerOptionDto>();
	public bool IsRequired { get; set; }
}
