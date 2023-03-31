using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class dbrekapTrainingfixed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string TRN_ID { get; set; }
        public string Type { get; set; }
        public string Program { get; set; }
        public string ProgramTxt { get; set; }
        public string EDP { get; set; }
        public string Periode { get; set; }
        public string Week { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        public DateTime? Date { get; set; }
        public string Participant { get; set; }
        public string Trainer { get; set; }
        public string idTrainer { get; set; }
        public int? NoParticipant { get; set; }
        public decimal? Days { get; set; }
        public decimal? Hours { get; set; }
        public decimal? TotalHours { get; set; }

        public DateTime? Entry_Date { get; set; }
        public string Entry_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_User { get; set; }
        public string FLAG_AKTIF { get; set; }
        public int? TrainingDays { get; set; }
        public int? Batch { get; set; }
        [NotMapped]
        public List<string> ParticipantList { get; set; }
        [NotMapped]
        public List<XstoreModel> ddEdp { get; set; }
        [NotMapped]
        public List<dbProgram> ddProgram { get; set; }
        [NotMapped]
        public List<dbRD> ddRD { get; set; }
        [NotMapped]
        public List<dbEmployee> ddEmp { get; set; }
        [NotMapped]
        public List<dbTrainer> ddTrainer { get; set; }
        [NotMapped]
        public List<dbNilaiSSFixed> nilaiSSFixedList { get; set; }
        [NotMapped]
        public List<VaksinModel> SSDD { get; set; }
        [NotMapped]
        public string SS_CODE { get; set; }
        [NotMapped]
        public string RD_CODE { get; set; }
        [NotMapped]
        public string EMP_CODE { get; set; }
        [NotMapped]
        public string idTrainerXls { get; set; }
        [NotMapped]
        public int NILAI { get; set; }
        [NotMapped]
        public string NoSertifikat { get; set; }
        [NotMapped]
        public bool isCertified { get; set; }
        [NotMapped]
        public bool isPresent { get; set; }
        [NotMapped]
        public string EMP_TYPE { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileSertifikat { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileUploadPeserta { get; set; }
        [NotMapped]
        public string TypeTemp { get; set; }
        [NotMapped]
        public string ProgramTemp { get; set; }
        [NotMapped]
        public string ProgramTxtTemp { get; set; }
        [NotMapped]
        public string EDPTemp { get; set; }
        [NotMapped]
        public List<string> ParticipantListTemp { get; set; }
        [NotMapped]
        public string PeriodeTemp { get; set; }
        [NotMapped]
        public string WeekTemp { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? DateTemp { get; set; }
        [NotMapped]
        public string ParticipantTemp { get; set; }
        [NotMapped]
        public string TrainerTemp { get; set; }
        [NotMapped]
        public string idTrainerTemp { get; set; }
        [NotMapped]
        public int? NoParticipantTemp { get; set; }
        [NotMapped]
        public decimal? DaysTemp { get; set; }
        [NotMapped]
        public decimal? HoursTemp { get; set; }
        [NotMapped]
        public decimal? TotalHoursTemp { get; set; }
        [NotMapped]
        public int? TrainingDaysTemp { get; set; }
        [NotMapped]
        public int? BatchTemp { get; set; }
        [NotMapped]
        public List<string> ParticipantListTemp_EDIT { get; set; }
        [NotMapped]
        public string SS_CODE_EDIT { get; set; }
        [NotMapped]
        public string RD_CODE_EDIT { get; set; }
        [NotMapped]
        public string EMP_CODE_EDIT { get; set; }
        [NotMapped]
        public int NILAI_EDIT { get; set; }
        [NotMapped]
        public string EMP_TYPE_EDIT { get; set; }
        [NotMapped]
        public string NoSertifikat_EDIT { get; set; }
        [NotMapped]
        public bool isCertified_EDIT { get; set; }
        [NotMapped]
        public bool isPresent_EDIT { get; set; }
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
        [DataType(DataType.Date)]
        public DateTime? DateTemp_EDIT { get; set; }
        [NotMapped]
        public string ParticipantTemp_EDIT { get; set; }
        [NotMapped]
        public string TrainerTemp_EDIT { get; set; }
        [NotMapped]
        public string idTrainerTemp_EDIT { get; set; }
        [NotMapped]
        public int? NoParticipantTemp_EDIT { get; set; }
        [NotMapped]
        public decimal? DaysTemp_EDIT { get; set; }
        [NotMapped]
        public decimal? HoursTemp_EDIT { get; set; }
        [NotMapped]
        public decimal? TotalHoursTemp_EDIT { get; set; }
        [NotMapped]
        public int? TrainingDaysTemp_EDIT { get; set; }
        [NotMapped]
        public int? BatchTemp_EDIT { get; set; }
        [NotMapped]
        public string datamode { get; set; }
        [NotMapped]
        public bool isPassed { get; set; }
        [NotMapped]
        public int idtemp { get; set; }
        [NotMapped]
        public int idtempUpl { get; set; }
        [NotMapped]
        public int idtemp_EDIT { get; set; }
        [NotMapped]
        public string TRN_IDtemp { get; set; }
        [NotMapped]
        public string TRN_IDtempUpl { get; set; }
        [NotMapped]
        public string TRN_IDtemp_EDIT { get; set; }
      
        [NotMapped]
        public int ScoreId { get; set; }
        [NotMapped]
        public string passingDate { get; set; }
        [NotMapped]
        public List<string> TrainerList { get; set; }
        [NotMapped]
        public List<string> TrainerListTemp { get; set; }
        [NotMapped]
        public List<string> TrainerListTemp_EDIT { get; set; }

        [NotMapped]
        public string TypeTempUpl { get; set; }
        [NotMapped]
        public string ProgramTempUpl { get; set; }
        [NotMapped]
        public string ProgramTxtTempUpl { get; set; }
        [NotMapped]
        public string EDPTempUpl { get; set; }
        [NotMapped]
        public List<string> ParticipantListTempUpl { get; set; }
        [NotMapped]
        public string PeriodeTempUpl { get; set; }
        [NotMapped]
        public string WeekTempUpl { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? DateTempUpl { get; set; }
        [NotMapped]
        public string ParticipantTempUpl { get; set; }
        [NotMapped]
        public string TrainerTempUpl { get; set; }
        [NotMapped]
        public string idTrainerTempUpl { get; set; }
        [NotMapped]
        public int? NoParticipantTempUpl { get; set; }
        [NotMapped]
        public decimal? DaysTempUpl { get; set; }
        [NotMapped]
        public decimal? HoursTempUpl { get; set; }
        [NotMapped]
        public decimal? TotalHoursTempUpl { get; set; }
        [NotMapped]
        public int? TrainingDaysTempUpl { get; set; }
        [NotMapped]
        public int? BatchTempUpl { get; set; }
        [NotMapped]
        public List<string> TrainerListTempUpl { get; set; }
        [NotMapped]
        public string error { get; set; }

    }
}
