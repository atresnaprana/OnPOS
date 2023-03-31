using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BataAppHR.Data;
using BataAppHR.Models;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
namespace BataAppHR.Controllers
{
    public class ExportDataTrainingController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        private IHostingEnvironment Environment;
        private readonly ILogger<ExportDataTrainingController> _logger;

        public ExportDataTrainingController(FormDBContext db, ILogger<ExportDataTrainingController> logger, IHostingEnvironment _environment)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var data = new List<ReportTraining>();
            var xstoretbl = db.xstore_organization.ToList();
            if (User.IsInRole("Admin"))
            {
                data = db.rekapdbFixed.Where(y => y.Date.Value.Year == DateTime.Now.Year).ToList().Select(y => new ReportTraining()
                {
                    Type = y.Type,
                    TrainingProgram = getProgramName(y.Program),
                    EDP = y.EDP,
                    STORE_LOCATION = xstoretbl.Where(x => x.edp == y.EDP).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP).FirstOrDefault().store_location : "",
                    Genesis = xstoretbl.Where(x => x.edp == y.EDP).FirstOrDefault() != null ? xstoretbl.Where(x => x.edp == y.EDP).FirstOrDefault().genesis_Flag : "",
                    Periode = y.Periode,
                    week = y.Week,
                    Date = y.Date,
                    participants = y.Participant,
                    trainer = getTrainerConcat(y.id),
                    NoParticipant = y.NoParticipant,

                    Hours = y.Hours,
                    TotalHours = y.TotalHours,
                }).OrderBy(y => y.TrainingProgram).ToList();
            }
            return View(data);
        }
        private string getProgramName(string program)
        {
            string programname = "";
            var dataprogram = db.programDb.Where(y => y.ProgramId == program).FirstOrDefault();
            if (dataprogram != null)
            {
                programname = dataprogram.ProgramName;
            }

            return programname;
        }
        public string getTrainerConcat(int formid)
        {
            string trainers = "";
            var trainerdt = db.trainerlistTbl.Where(y => y.idFormRekap == formid).ToList();
            var trainerlist = db.trainerDb.ToList();

            if (trainerdt.Count() > 0)
            {
                if (trainerdt.Count() == 1)
                {
                    trainers = trainerlist.Where(y => y.idTrainer == trainerdt[0].idTrainer).FirstOrDefault().NmShort;
                }
                else
                {
                    foreach (var fld in trainerdt)
                    {
                        trainers += trainerlist.Where(y => y.idTrainer == fld.idTrainer).FirstOrDefault().NmShort;
                        trainers += ";";
                    }
                }
            }
            return trainers;
        }
    }
}
