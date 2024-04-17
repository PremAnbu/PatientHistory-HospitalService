using Microsoft.AspNetCore.Mvc;
using PatientHistory_HospitalService.Service.Interface;

namespace PatientHistory_HospitalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientHistoryController
    {
        private readonly IPatientHistory patientHistory;
        private readonly HttpClient _httpClient;

        public PatientHistoryController(IPatientHistory patientHistory, HttpClient httpClient)
        {
            patientHistory = patientHistory;
            _httpClient = httpClient;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> AddPatientHistory( loginRequestModel)
        {

            var token = await patientHistory.AddPatientHistory(loginRequestModel);
        }
    }
}
