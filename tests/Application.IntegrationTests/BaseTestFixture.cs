using NUnit.Framework;

namespace Sonuts.Application.IntegrationTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
	[SetUp]
	public async Task TestSetUp()
	{
		await ResetState();
	}
}
