using CsvHelper.Configuration;
using Sonuts.Domain.Entities;

namespace Sonuts.Infrastructure.Files.Maps;

public sealed class QuestionResponseMap : ClassMap<QuestionResponse>
{
	public QuestionResponseMap()
	{
		Map(qr => qr.QuestionnaireResponse.Participant.Id)
			.Name($"{nameof(Participant)}.{nameof(Participant.Id)}")
			.Index(0);

		Map(qr => qr.QuestionnaireResponse.Questionnaire.Id)
			.Name($"{nameof(Questionnaire)}.{nameof(Participant.Id)}")
			.Index(1);

		Map(qr => qr.Question.Id)
			.Name($"{nameof(Question)}.{nameof(Question.Id)}")
			.Index(2);

		Map(qr => qr.Question.Text)
			.Name($"{nameof(Question)}.{nameof(Question.Text)}")
			.Index(3);

		Map(qr => qr.Question.Type)
			.Name($"{nameof(Question)}.{nameof(Question.Type)}")
			.Index(4);

		Map(qr => qr.Answer)
			.Index(5);

		Map(qr => qr.QuestionnaireResponse.CreatedAt)
			.Index(6);
	}
}
