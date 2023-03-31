using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataAppHR.Models
{
    public class ReportTraining
    {
        public string Type { get; set; }
        public string TrainingProgram { get; set; }
        public string EDP { get; set; }
        public string STORE_LOCATION { get; set; }
        public string Genesis { get; set; }
        public string Periode { get; set; }
        public string week { get; set; }
        public DateTime? Date { get; set; }
        public string participants { get; set; }
        public string trainer { get; set; }
        public int? NoParticipant { get; set; }
        public decimal? Hours { get; set; }
        public decimal? TotalHours { get; set; }
        public string SS_NAME { get; set; }
        public string presence { get; set; }

        public string certified { get; set; }
        public string participantid { get; set; }

        public int nilai { get; set; }
        public string emp_type { get; set; }



    }
}
