using Dapper;
using Microsoft.AspNetCore.Mvc;
using PatientHistory_HospitalService.DapperContext;
using PatientHistory_HospitalService.DTO.RequestDto;
using PatientHistory_HospitalService.DTO.ResponseDto;
using PatientHistory_HospitalService.Entity;
using PatientHistory_HospitalService.Service.Interface;
using System.Collections.Generic;

namespace PatientHistory_HospitalService.Service.Impl
{
    public class PatientHistoryServiceImpl : IPatientHistory
    {
        private readonly PatientHistoryContext _context;
        private readonly IHttpClientFactory httpClientFactory;

        public PatientHistoryServiceImpl(PatientHistoryContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ActionResult<List<object>>> AddPatientHistory(HistoryRequestDto historyRequestDto,int doctorId)
        {
            string HistoryQuery = "INSERT INTO History (PatientId,DoctorId, Issue, VisitsToDoctor) " +
                "VALUES (@PatientId,@DoctorId, @Issue , @VisitsToDoctor); ";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("PatientId",historyRequestDto.PatientId);
            dynamicParameters.Add("DoctorId",doctorId);
            dynamicParameters.Add("Issue", historyRequestDto.Issue);
            dynamicParameters.Add("VisitsToDoctor", historyRequestDto.VisitsToDoctor);
            USerEntity user = getUserById(historyRequestDto.PatientId);
            Console.WriteLine(user.UserId+" "+user.FirstName+" "+user.Email);
            var query = @"INSERT INTO PatientHistory (PatientId, PatientName,Email) VALUES (@PatientId, @PatientName,@Email)";

            DynamicParameters dynamicParameter = new DynamicParameters();
                dynamicParameter.Add("PatientId", user.UserId);
                dynamicParameter.Add("PatientName", user.FirstName);
                dynamicParameter.Add("Email", user.Email);

            Console.WriteLine("part 3");
           // History his;
            //PatientHistory patientHistory;
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(HistoryQuery, dynamicParameters);
                //  his = await connection.QueryFirstOrDefaultAsync<History>("SELECT * FROM History WHERE PatientId = @PatientId", new { PatientId = historyRequestDto.PatentId });
                var val = await GetPatientDetails(user.UserId);
                if (val == null)
                {
                    await connection.ExecuteAsync(query, dynamicParameter);
                }

                // patientHistory = await connection.QueryFirstOrDefaultAsync<PatientHistory>("SELECT * FROM PatientHistory WHERE PatientId = @PatientId", new { PatientId = user.UserId });
                return await GetHistory(user.UserId,doctorId);           
            }
           
        }

        public USerEntity getUserById(int patientId)
        {
            var httpclient = httpClientFactory.CreateClient("userByid");
            Console.WriteLine("part 4" + patientId);
            var responce = httpclient.GetAsync($"GetUserById?UserId={patientId}").Result;
            Console.WriteLine("part 4"+responce);

            if (responce.IsSuccessStatusCode)
            {
                return responce.Content.ReadFromJsonAsync<USerEntity>().Result;
            }
            throw new Exception("UserNotFound Create User FIRST OE TRY DIFFERENT EMAIL ID");
        }

        public async Task<ActionResult<List<object>>> GetHistory(int patientId, int doctorId)
        {
            try
            {
                PatientHistory patientHistory = await GetPatientDetails(patientId);
                List<HistoryResponseDto> patientHistoryList = await GetPatientHistory(patientId,doctorId);
                var result = new List<object> { patientHistory, patientHistoryList };
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return new List<object> { new { Error = $"An error occurred: {ex.Message}" } };
            }
        }


        private async Task<PatientHistory> GetPatientDetails(int patientId)
        {
            string selectPatientQuery = @"SELECT PatientId, PatientName, Email FROM PatientHistory WHERE PatientId = @PatientId;";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<PatientHistory>(selectPatientQuery, new { PatientId = patientId });
            }
        }

        private async Task<List<HistoryResponseDto>> GetPatientHistory(int patientId, int doctorId)
        {
            string selectHistoryQuery = @"SELECT Issue, VisitsToDoctor FROM History WHERE PatientId = @PatientId AND DoctorId = @DoctorId;";

            using (var connection = _context.CreateConnection())
            {
                return (await connection.QueryAsync<HistoryResponseDto>(selectHistoryQuery, new { PatientId = patientId, DoctorId = doctorId })).ToList();
            }
        }




    }
}
