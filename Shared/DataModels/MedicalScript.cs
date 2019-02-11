
namespace ArVision.Pharma.Shared.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class MedicalScript
    {
        public int Id { get; set; }
        public string ClinicAddress { get; set; }
        public string ClinicTel { get; set; }
        public string ClinicFax { get; set; }
        public string DoctorName { get; set; }
        public string ScriptCreationDate { get; set; }
        public string PatientName { get; set; }
        public string PatientDOB { get; set; }
        public string PatientOHIP { get; set; }
        public string PatientPhone { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyAddress { get; set; }
        public string PharmacyPhone { get; set; }
        public string PharmacyFax { get; set; }
        public string DoseName { get; set; }
        public string DoseFromDate { get; set; }
        public string DoseToDate { get; set; }
        public string DoseTotalDays { get; set; }
        public string DoseTotalAmount { get; set; }
        public string PharmacistNurseNotes { get; set; }
        public string PharmacistNurseNotesDays { get; set; }
        public string MD { get; set; }
        public string Verification { get; set; }
        public string HealthCard { get; set; }
    }
}
