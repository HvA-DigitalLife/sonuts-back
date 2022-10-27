using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Dtos;

public class FaqDto : IMapFrom<Faq>
{
	public string Question { get; set; } = default!;
	public string Answer { get; set; } = default!;
}
