namespace Sonuts.Domain.Enums;

public enum QuestionType
{
	/// <summary>
	/// Yes/No
	/// </summary>
	Boolean,

	/// <summary>
	/// Decimal number
	/// </summary>
	Decimal,

	/// <summary>
	/// Integer number
	/// </summary>
	Integer,

	/// <summary>
	/// Open
	/// </summary>
	String,

	/// <summary>
	/// Choose one
	/// </summary>
	Choice,

	/// <summary>
	/// Choose one including one open
	/// </summary>
	OpenChoice,

	/// <summary>
	/// Choose multiple
	/// </summary>
	MultiChoice,

	/// <summary>
	/// Choose multiple including one open
	/// </summary>
	MultiOpenChoice
}
