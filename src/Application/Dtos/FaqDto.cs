using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class FaqDto : IMapFrom<Faq>
{
	public required string Question { get; set; }
	public required string Answer { get; set; }
}
