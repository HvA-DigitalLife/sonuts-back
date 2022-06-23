using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Sonuts.Infrastructure.Fhir.Interfaces;
using Sonuts.Infrastructure.Fhir.Models;
using Task = System.Threading.Tasks.Task;

namespace Sonuts.Infrastructure.Fhir.Daos;

public class FhirParticipantDao : IParticipantDao
{

	private readonly IHttpClientFactory _httpClientFactory;

	public FhirParticipantDao(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

	// var identifier = "bla"; // keycloak identifier

	// // fhir json payload created from post
	// var payload = new JsonObject
	// {

	//     ["resourceType"] = "Patient",
	//     ["text"] =  new JsonObject
	//     {
	//         ["status"] = "generated",
	//         ["div"] = "<div xmlns=\"http://www.w3.org/1999/xhtml\">MiB Patient</div>"
	//     },
	//     ["identifier"] = new JsonArray(
	//         new JsonObject 
	//         {
	//             ["use"] = "usual",
	//             ["system"] = "urn:ietf:rfc:3986",
	//             ["value"] = "urn:uuid:" + identifier,
	//         }
	//     ),
	//     ["active"] = true,
	//     ["name"] = new JsonArray(
	//         new JsonObject 
	//         {
	//             ["use"] = "anonymous",
	//             ["text"] = participantDTO.Pseudonym
	//         }
	//     ),
	//     ["maritalStatus"] = new JsonObject {
	//         ["coding"] = new JsonArray(
	//             new JsonObject {
	//                 ["system"] = "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus",
	//                 ["code"] = "M",
	//                 ["display"] = participantDTO.MaritalStatus
	//             }
	//         ),
	//         ["text"] = participantDTO.MaritalStatus
	//     },
	//     ["gender"] = participantDTO.Gender,
	//     // ["birthDate"] = participantDTO.Age, CALCULATE
	//     //["generalPractitioner"] = new JsonArray(..list of caregivers..)
	// };    

	public async Task<Participant> Insert(Participant participant)
	{
		// create patient object
		Patient fhirObject = new Patient();
		var fhirName = new HumanName().WithGiven(participant.Pseudonym);
		fhirObject.Name.Add(fhirName);

		// serialize
		var serializer = new FhirJsonSerializer();
		var payload = await serializer.SerializeToStringAsync(fhirObject);

		// send to fhir server
		var fhirClient = _httpClientFactory.CreateClient("Fhir");
		var response = await fhirClient.PostAsync("Patient", new StringContent(payload, Encoding.UTF8, "application/json"));

		var responseContent = await response.Content.ReadAsStringAsync();

		return participant;
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
