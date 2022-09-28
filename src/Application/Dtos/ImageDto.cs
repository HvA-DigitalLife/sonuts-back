using AutoMapper;
using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class ImageDto : IMapFrom<Image>
{
	public string Uri { get; set; } = default!;

	public void Mapping(Profile profile) =>
		profile.CreateMap<Image, ImageDto>()
			.ForMember(imageDto => imageDto.Uri,
				member => member.MapFrom(image => $"/images/{image.Name ?? image.Id.ToString()}.{image.Extension}"));
}
