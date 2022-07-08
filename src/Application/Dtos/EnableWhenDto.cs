using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class EnableWhenDto : IMapFrom<EnableWhen>
{
	public Guid QuestionId { get; set; } = default!;
	public Operator Operator { get; set; } = default!;
	public string Answer { get; set; } = default!;
}
