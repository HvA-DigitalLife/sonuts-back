using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class VideoDto : IMapFrom<Video>
{
	public string Url { get; set; } = default!;
}
