using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class AnswerOptionDto : IMapFrom<AnswerOption>
{
	public string Text { get; set; } = default!;
	public int Order { get; set; } = default!;
}
