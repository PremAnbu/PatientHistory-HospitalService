using Microsoft.Data.SqlClient;
using System.Data;

namespace PatientHistory_HospitalService.DapperContext
{
    public class PatientHistoryContext
    {
        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        public PatientHistoryContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PatienHistoryConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
