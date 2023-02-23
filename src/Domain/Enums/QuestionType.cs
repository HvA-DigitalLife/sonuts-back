namespace Sonuts.Domain.Enums;

/// <summary>
/// Question Type
/// </summary>
public enum QuestionType
{
	/// <summary>
	/// Display information
	/// </summary>
	Display,

	/// <summary>
	/// Yes/No
	/// </summary>
	Boolean,

	/// <summary>
	/// Open
	/// </summary>
	String,

	/// <summary>
	/// Integer number
	/// </summary>
	Integer,

	/// <summary>
	/// Decimal number
	/// </summary>
	Decimal,

	/// <summary>
	/// Time (hour:minute)
	/// </summary>
	Duration,

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
