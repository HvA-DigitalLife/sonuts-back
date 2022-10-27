using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

/// <summary>
/// Question
/// </summary>
public class QuestionDto : IMapFrom<Question>
{
	public Guid Id { get; set; }
	public QuestionType Type { get; set; } = default!;
	/// <summary>
	/// Question Title
	/// </summary>
	public string Text { get; set; } = default!;
	/// <summary>
	/// Question description
	/// </summary>
	public string? Description { get; set; }
	/// <summary>
	/// Question number for ordering
	/// </summary>
	public int Order { get; set; } = default!;
	public EnableWhenDto? EnableWhen { get; set; }
	public ICollection<AnswerOptionDto>? AnswerOptions { get; set; } = new List<AnswerOptionDto>();
	public bool IsRequired { get; set; }
}
