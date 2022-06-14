using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities.Owned;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class QuestionDependencyDto : IMapFrom<QuestionDependency>
{
	public Guid QuestionId { get; set; } = default!;
	public Operator Operator { get; set; } = default!;
	public string Value { get; set; } = default!;
}
