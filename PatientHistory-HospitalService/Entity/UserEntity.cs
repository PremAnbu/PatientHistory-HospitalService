﻿namespace PatientHistory_HospitalService.Entity
{
    public class USerEntity
    {
        public int UserId {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string Role { get; set; }
    }
}
