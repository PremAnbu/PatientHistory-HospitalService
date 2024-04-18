using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientHistory_HospitalService.DTO.RequestDto;
using PatientHistory_HospitalService.Service.Interface;
using System.Security.Claims;

namespace PatientHistory_HospitalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientHistoryController:ControllerBase
    {   
        private readonly IPatientHistory patientHistory;
        private readonly HttpClient _httpClient;

        public PatientHistoryController(IPatientHistory patientHistory, HttpClient httpClient)
        {
           this.patientHistory = patientHistory;
            _httpClient = httpClient;
        }

        [Authorize(Roles ="Doctor")]
        [HttpPost("AddHistory")]
        public async Task<ActionResult<List<object>>> AddPatientHistory(HistoryRequestDto historyRequestDto )
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await patientHistory.AddPatientHistory(historyRequestDto,userId);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetHistory(int PatientId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await patientHistory.GetHistory(PatientId,userId);
        }
        
    }
}
