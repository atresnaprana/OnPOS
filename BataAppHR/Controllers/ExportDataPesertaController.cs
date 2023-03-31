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
    public class ExportDataPesertaController : Controller
    {
        private readonly FormDBContext db;
        public const string SessionKeyName = "EDPCode";
        private IHostingEnvironment Environment;
        private readonly ILogger<ExportDataTrainingController> _logger;

        public ExportDataPesertaController(FormDBContext db, ILogger<ExportDataPesertaController> logger, IHostingEnvironment _environment)
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
                var dataRekap = db.rekapdbFixed.Where(y => y.Date.Value.Year == DateTime.Now.Year).ToList();
                List<string> formidlist = new List<string>();
                foreach(var dt in dataRekap)
                {
                    formidlist.Add(dt.TRN_ID);
                }
                data = db.NilaissTblFixed.Where(y => formidlist.Contains(y.TRN_ID)).ToList().Select(y => new ReportTraining()
                {
                    Type = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Type : "",
                    TrainingProgram = getProgramName(y.TRN_ID),
                    EDP = getEDPTrainee(y.EMP_TYPE, y.SS_CODE),
                    STORE_LOCATION = getStoreName(y.EMP_TYPE, y.SS_CODE),
                    participantid = y.SS_CODE,
                    SS_NAME =getParticipantName(y.EMP_TYPE, y.SS_CODE),
                    participants = y.EMP_TYPE,
                    Genesis = getGenesis(y.TRN_ID),
                    Periode = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Periode : "",
                    week = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Week : "",
                    Date = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Date : null,
                    trainer = getTrainerConcat(y.TRN_ID),
                    NoParticipant = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().NoParticipant : null,
                    Hours = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().Hours : null,
                    TotalHours = dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault() != null ? dataRekap.Where(x => x.TRN_ID == y.TRN_ID).FirstOrDefault().TotalHours : null,
                }).OrderBy(y => y.TrainingProgram).ToList();
            }
            return View(data);
        }
        private string getProgramName(string trainingid)
        {
            string programname = "";
            var getid = db.rekapdbFixed.Where(y => y.TRN_ID == trainingid).FirstOrDefault();
            string program = "";
            if (getid != null)
            {
                program = getid.Program;
            }
            var dataprogram = db.programDb.Where(y => y.ProgramId == program).FirstOrDefault();
            if(dataprogram != null)
            {
                programname = dataprogram.ProgramName;
            }
            return programname;
        }
        private string getStoreLoc(string trnid)
        {
            string storeloc = "";
            var rekapData = db.rekapdbFixed.Where(y => y.TRN_ID == trnid).FirstOrDefault();
            if(rekapData != null)
            {
                var xstoredata = db.xstore_organization.ToList();
                var fld = xstoredata.Where(y => y.edp == rekapData.EDP).FirstOrDefault();
                if(fld != null)
                {
                    storeloc = fld.store_location;
                }
            }
            return storeloc;
        }
        private string getGenesis(string trnid)
        {
            string storegen = "";
            var rekapData = db.rekapdbFixed.Where(y => y.TRN_ID == trnid).FirstOrDefault();
            if (rekapData != null)
            {
                var xstoredata = db.xstore_organization.ToList();
                var fld = xstoredata.Where(y => y.edp == rekapData.EDP).FirstOrDefault();
                if (fld != null)
                {
                    storegen = fld.genesis_Flag;
                }
            }
            return storegen;
        }
        public string getTrainerConcat(string trnid)
        {
            string trainers = "";
            var getid = db.rekapdbFixed.Where(y => y.TRN_ID == trnid).FirstOrDefault();
            int formid = 0;
            if(getid != null)
            {
                formid = getid.id;
            }
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
        private string getParticipantName(string emptype, string participantid)
        {
            string participantname = "";
            
            var dataRD = db.RDTbl.ToList();
            var dataSS = db.SSTable.ToList();
            var dataEmp = db.EmpTbl.ToList();
            if(emptype == "SS")
            {
                var ssfld = dataSS.Where(y => y.SS_CODE == participantid).FirstOrDefault();
                if(ssfld != null)
                {
                    participantname = ssfld.NAMA_SS;
                }
            }
            else if(emptype == "RD")
            {
                var rdfld = dataRD.Where(y => y.RD_CODE == participantid).FirstOrDefault();
                if(rdfld != null)
                {
                    participantname = rdfld.NM_RD;
                }
            }
            else
            {
                var empFld = dataEmp.Where(y => y.EMP_CODE == participantid).FirstOrDefault();
                if(empFld != null)
                {
                    participantname = empFld.NM_EMP;
                }
            }

            
            return participantname;
        }
        private string getEDPTrainee(string emptype, string participantid)
        {
            string EDPName = "";

            var dataRD = db.RDTbl.ToList();
            var edpDt = db.xstore_organization.ToList();
            var dataSS = db.SSTable.ToList();
            var dataEmp = db.EmpTbl.ToList();
            if (emptype == "SS")
            {
                var ssfld = dataSS.Where(y => y.SS_CODE == participantid).FirstOrDefault();
                if (ssfld != null)
                {
                    EDPName = ssfld.EDP_CODE;
                }
            }
            else if (emptype == "RD")
            {
                var rdfld = edpDt.Where(y => y.RD_CODE == participantid).ToList();
                if(rdfld.Count() > 0)
                {
                    if(rdfld.Count() > 1)
                    {
                        foreach(var fields in rdfld)
                        {
                            EDPName += fields.edp;
                            EDPName += ";";
                        }
                    }
                    else
                    {
                        EDPName = rdfld[0].edp;

                    }
                }
                
            }
            else
            {
                EDPName = "HO";

            }


            return EDPName;
        }
        private string getStoreName(string emptype, string participantid)
        {
            string EDPName = "";
            string storeName = "";
            var dataRD = db.RDTbl.ToList();
            var edpDt = db.xstore_organization.ToList();
            var dataSS = db.SSTable.ToList();
            var dataEmp = db.EmpTbl.ToList();
            if (emptype == "SS")
            {
                var ssfld = dataSS.Where(y => y.SS_CODE == participantid).FirstOrDefault();
                if (ssfld != null)
                {
                    EDPName = ssfld.EDP_CODE;
                    if (!string.IsNullOrEmpty(EDPName))
                    {
                        var storeFld = edpDt.Where(y => y.edp == EDPName).FirstOrDefault();
                        if(storeFld != null)
                        {
                            storeName = storeFld.store_location;
                        }
                    }
                }
            }
            else if (emptype == "RD")
            {
                var rdfld = edpDt.Where(y => y.RD_CODE == participantid).ToList();
                if (rdfld.Count() > 0)
                {
                    if (rdfld.Count() > 1)
                    {
                        foreach (var fields in rdfld)
                        {
                            EDPName += fields.edp;
                            if (!string.IsNullOrEmpty(EDPName))
                            {
                                var storeFld = edpDt.Where(y => y.edp == EDPName).FirstOrDefault();
                                if (storeFld != null)
                                {
                                    storeName += storeFld.store_location;
                                    storeName += ";";

                                }
                            }
                        }
                    }
                    else
                    {
                        EDPName = rdfld[0].edp;
                        if (!string.IsNullOrEmpty(EDPName))
                        {
                            var storeFld = edpDt.Where(y => y.edp == EDPName).FirstOrDefault();
                            if (storeFld != null)
                            {
                                storeName = storeFld.store_location;
                            }
                        }

                    }
                }

            }
            else
            {
                storeName = "Head Office";

            }


            return storeName;
        }
    }
   
}
