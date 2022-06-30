namespace Sonuts.Domain.Enums;

public enum QuestionType
{
	//TODO: Remove old types
	Activity,
	Open,
	Range,
	//Integer,
	//Decimal,
	MultipleChoice,
	MultipleOpen,
	
	// FHIR
	Boolean,
	Decimal,
	Integer,
	String,
	Choice,
	OpenChoice,
	MultiChoice, //TODO: Check in fhir
	MultiOpenChoice //TODO: Check in fhir
}
