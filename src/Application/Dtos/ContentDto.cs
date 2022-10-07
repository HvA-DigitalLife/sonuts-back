using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Dtos;

public class ContentDto : IMapFrom<Domain.Entities.Content>
{
	public Guid Id { get; set; }
	public ContentType Type { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Subtitle { get; set; } = default!;
	public string Description { get; set; } = default!;
}
