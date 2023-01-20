using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class AnswerOptionDto : IMapFrom<AnswerOption>
{
	public Guid Id { get; set; }
	public required string Value { get; set; }
	public required int Order { get; set; }
}
