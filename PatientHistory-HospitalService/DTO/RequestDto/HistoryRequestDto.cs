namespace PatientHistory_HospitalService.DTO.RequestDto
{
    public class HistoryRequestDto
    {
        public int PatientId { get; set; }
        public string Issue { get; set; }
        public DateTime VisitsToDoctor { get; set; }
    }
}
