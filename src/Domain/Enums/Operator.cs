using System.ComponentModel;

namespace Sonuts.Domain.Enums;

public enum Operator //http://hl7.org/fhir/R4/valueset-questionnaire-enable-operator.html
{
	[Description("=")]
	Equals,

	[Description("!=")]
	NotEquals,

	[Description(">")]
	GreaterThan,

	[Description("<")]
	LessThan,

	[Description(">=")]
	GreaterOrEquals,

	[Description("<=")]
	LessOrEquals
}
