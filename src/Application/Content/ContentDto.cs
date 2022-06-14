using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Content;

public class ContentDto : IMapFrom<Domain.Entities.Content>
{
	public ContentType Type { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Description { get; set; } = default!;
}
