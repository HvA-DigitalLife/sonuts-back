using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class ContentDto : IMapFrom<Domain.Entities.Content>
{
	public Guid Id { get; set; }
	public required ContentType Type { get; set; }
	public required string Title { get; set; }
	public required string Subtitle { get; set; }
	public required string Description { get; set; }
}
