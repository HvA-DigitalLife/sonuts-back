using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class QuestionDependencyDto : IMapFrom<EnableWhen>
{
	public required Guid QuestionId { get; set; }
	public required Operator Operator { get; set; }
	public required string Value { get; set; }
}
