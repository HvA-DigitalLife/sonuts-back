using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class QuestionDto : IMapFrom<Question>
{
	public Guid Id { get; set; }
	public required QuestionType Type { get; set; }
	public required string Text { get; set; }
	public string? Description { get; set; }
	public required int Order { get; set; }
	public EnableWhenDto? EnableWhen { get; set; }
	public ICollection<AnswerOptionDto>? AnswerOptions { get; set; } = new List<AnswerOptionDto>();
	public bool IsRequired { get; set; }
}
