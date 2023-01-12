using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class QuestionResponseDto : IMapFrom<QuestionResponse>
{
	public Guid Id { get; set; }
	public required QuestionDto Question { get; set; }
	public required string Answer { get; set; }
}
