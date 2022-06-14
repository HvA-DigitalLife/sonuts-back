using System.Runtime.Serialization;
using AutoMapper;
using NUnit.Framework;
using Sonuts.Application.Common.Mappings;

namespace Sonuts.Application.UnitTests.Common.Mappings;

public class MappingTests
{
	private readonly IConfigurationProvider _configuration;
	private readonly IMapper _mapper;

	public MappingTests()
	{
		_configuration = new MapperConfiguration(config =>
			config.AddProfile<MappingProfile>());

		_mapper = _configuration.CreateMapper();
	}

	[Test]
	public void ShouldHaveValidConfiguration()
	{
		_configuration.AssertConfigurationIsValid();
	}

	[Test]
	[TestCase(typeof(bool), typeof(bool))]
	public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
	{
		var instance = GetInstanceOf(source);

		_mapper.Map(instance, source, destination);
	}

	private static object GetInstanceOf(Type type)
	{
		if (type.GetConstructor(Type.EmptyTypes) != null)
			return Activator.CreateInstance(type)!;

		// Type without parameterless constructor
		return FormatterServices.GetUninitializedObject(type);
	}
}
