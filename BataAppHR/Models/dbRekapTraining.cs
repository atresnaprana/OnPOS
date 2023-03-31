using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class dbRekapTraining
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string TRN_ID { get; set; }
        public string Type { get; set; }
        public string Program { get; set; }
        public string ProgramTxt { get; set; }
        public string EDP { get; set; }
        public string Periode { get; set; }
        public string Week { get; set; }
        public DateTime? Date { get; set; }
        public string Participant { get; set; }
        public string Trainer { get; set; }
        public string idTrainer { get; set; }
        public int NoParticipant { get; set; }
        public int Days { get; set; }
        public decimal? Hours { get; set; }
        public int TotalHours { get; set; }
    

        public DateTime Entry_Date { get; set; }
        public string Entry_User { get; set; }
        public DateTime Update_Date { get; set; }
        public string Update_User { get; set; }
        public string FLAG_AKTIF { get; set; }
        [NotMapped]
        public List<XstoreModel> ddEdp { get; set; }
        [NotMapped]
        public List<dbTrainer> ddTrainer { get; set; }
        [NotMapped]
        public List<dbNilaiSSFixed> nilaiSSFixedList { get; set; }
        [NotMapped]
        public List<VaksinModel> SSDD { get; set; }
        [NotMapped]
        public string SS_CODE { get; set; }
        [NotMapped]
        public int NILAI { get; set; }
        [NotMapped]
        public string NoSertifikat { get; set; }
        [NotMapped]
        public bool isCertified { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileSertifikat { get; set; }
        [NotMapped]
        public string TypeTemp { get; set; }
        [NotMapped]
        public string ProgramTemp { get; set; }
        [NotMapped]
        public string ProgramTxtTemp { get; set; }
        [NotMapped]
        public string EDPTemp { get; set; }
        [NotMapped]
        public string PeriodeTemp { get; set; }
        [NotMapped]
        public string WeekTemp { get; set; }
        [NotMapped]
        public DateTime? DateTemp { get; set; }
        [NotMapped]
        public string ParticipantTemp { get; set; }
        [NotMapped]
        public string TrainerTemp { get; set; }
        [NotMapped]
        public string idTrainerTemp { get; set; }
        [NotMapped]
        public int NoParticipantTemp { get; set; }
        [NotMapped]
        public int DaysTemp { get; set; }
        [NotMapped]
        public decimal? HoursTemp { get; set; }
        [NotMapped]
        public int TotalHoursTemp { get; set; }

        [NotMapped]
        public string SS_CODE_EDIT { get; set; }
        [NotMapped]
        public int NILAI_EDIT { get; set; }
        [NotMapped]
        public string NoSertifikat_EDIT { get; set; }
        [NotMapped]
        public bool isCertified_EDIT { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileSertifikat_EDIT { get; set; }
        [NotMapped]
        public string TypeTemp_EDIT { get; set; }
        [NotMapped]
        public string ProgramTemp_EDIT { get; set; }
        [NotMapped]
        public string ProgramTxtTemp_EDIT { get; set; }
        [NotMapped]
        public string EDPTemp_EDIT { get; set; }
        [NotMapped]
        public string PeriodeTemp_EDIT { get; set; }
        [NotMapped]
        public string WeekTemp_EDIT { get; set; }
        [NotMapped]
        public DateTime? DateTemp_EDIT { get; set; }
        [NotMapped]
        public string ParticipantTemp_EDIT { get; set; }
        [NotMapped]
        public string TrainerTemp_EDIT { get; set; }
        [NotMapped]
        public string idTrainerTemp_EDIT { get; set; }
        [NotMapped]
        public int NoParticipantTemp_EDIT { get; set; }
        [NotMapped]
        public int DaysTemp_EDIT { get; set; }
        [NotMapped]
        public decimal? HoursTemp_EDIT { get; set; }
        [NotMapped]
        public int TotalHoursTemp_EDIT { get; set; }
    }
}
