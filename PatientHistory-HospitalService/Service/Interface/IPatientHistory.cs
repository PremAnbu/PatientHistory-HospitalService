using Microsoft.AspNetCore.Mvc;
using PatientHistory_HospitalService.DTO.RequestDto;

namespace PatientHistory_HospitalService.Service.Interface
{
    public interface IPatientHistory
    {
        public Task<ActionResult<List<object>>> AddPatientHistory(HistoryRequestDto historyRequestDto,int userId);
        public Task<ActionResult<List<object>>> GetHistory(int PatientId,int userId);

    }
}
