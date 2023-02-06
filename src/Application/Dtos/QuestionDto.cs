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
	public required QuestionType Type { get; set; }
	/// <summary>
	/// Question Title
	/// </summary>
	public required string Text { get; set; }
	/// <summary>
	/// Question description
	/// </summary>
	public string? Description { get; set; }
	/// <summary>
	/// Question number for ordering
	/// </summary>
	public required int Order { get; set; }
	public EnableWhenDto? EnableWhen { get; set; }
	public ICollection<AnswerOptionDto>? AnswerOptions { get; set; } = new List<AnswerOptionDto>();
	public bool IsRequired { get; set; }
}
