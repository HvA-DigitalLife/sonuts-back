using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Sonuts.Infrastructure.Common;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Fhir.Adapters;


using Task = System.Threading.Tasks.Task;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirParticipantDao : IParticipantDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirParticipantDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

  

	public async Task<Participant> Insert(Participant participant)
	{
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var response = await client.PostAsync("Patient", new StringContent(FhirParticipantAdapter.ToJson(participant), Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return FhirParticipantAdapter.FromJson(responseContent);
	}


	public async Task<Participant> Select(string id)
	{
		// load and parse questionnaire response instance
		var client = _httpClientFactory.CreateClient(HttpClientName.Fhir);
		var result = await client.GetStringAsync("Patient/" + id);

		return FhirParticipantAdapter.FromJson(result);
	}


	public async Task<bool> Update(Participant participant)
	{
		await Task.Delay(1);
		return true;
	}

	public async Task<bool> Delete(int participantId)
	{
		await Task.Delay(1);
		return true;
	}
}
