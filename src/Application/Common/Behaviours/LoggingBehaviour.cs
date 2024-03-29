using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Sonuts.Application.Common.Interfaces;

namespace Sonuts.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
	private readonly ILogger<TRequest> _logger;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;

	public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
	{
		_logger = logger;
		_currentUserService = currentUserService;
		_identityService = identityService;
	}

	public async Task Process(TRequest request, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		var userId = _currentUserService.UserId ?? string.Empty;
		var userName = string.IsNullOrEmpty(userId) ? string.Empty : await _identityService.GetUserNameAsync(userId) ?? string.Empty;

		if (_logger.IsEnabled(LogLevel.Information))
			_logger.LogInformation("Sonuts request: {RequestName} {@UserId} {@UserName} {@Request}",
			requestName, userId, userName, request);
	}
}
