using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class EnableWhenDto : IMapFrom<EnableWhen>
{
	public required Guid DependentQuestionId { get; set; }
	public required Operator Operator { get; set; }
	public required string Answer { get; set; }
}
