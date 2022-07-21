using FluentValidation;
using MediatR;

namespace Sonuts.Application.Goals.Commands;
public class ChangeGoalMomentCommand : IRequest<Unit>
{
	public Guid Id { get; set; }
}


internal class ChangeGoalMomentCommandValidator : AbstractValidator<ChangeGoalMomentCommand>
{
	public ChangeGoalMomentCommandValidator()
	{
	}
}

internal class ChangeGoalMomentCommandHandler : IRequestHandler<ChangeGoalMomentCommand, Unit>
{
	public ChangeGoalMomentCommandHandler()
	{
	}

	public async Task<Unit> Handle(ChangeGoalMomentCommand request, CancellationToken cancellationToken)
	{
		return await Unit.Task;
	}
}
