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
using OfficeOpenXml;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
namespace BataAppHR.Controllers
{
    public class RekapTrainingController : Controller
    {
        private readonly FormDBContext db;

        private IHostingEnvironment Environment;
        private readonly ILogger<RekapTrainingController> _logger;
        public const string SessionKeyName = "FormList";
        public const string SessionKeyNameEdit = "FormListEdit";
        public const string SessionKeyNameFilter = "TypeCode";
        public IConfiguration Configuration { get; }

        public RekapTrainingController(FormDBContext db, ILogger<RekapTrainingController> logger, IHostingEnvironment _environment, IConfiguration configuration)
        {
            logger = logger;
            Environment = _environment;
            this.db = db;
            Configuration = configuration;

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index(rekapFront objPassed)
        {
            //var data = new List<dbrekapTrainingfixed>();
            //data = db.rekapdbFixed.Where(y => y.FLAG_AKTIF != "0").ToList().ToList().Select(y => new dbrekapTrainingfixed()
            //{
            //    id = y.id,
            //    TRN_ID = y.TRN_ID,
            //    Type = y.Type,
            //    Program = dbprograms.Where(x => x.ProgramId == y.Program).FirstOrDefault() != null ? dbprograms.Where(x => x.ProgramId == y.Program).FirstOrDefault().ProgramName : "",
            //    ProgramTxt = y.ProgramTxt,
            //    EDP = y.EDP,
            //    NoSertifikat = y.NoSertifikat,
            //    Periode = y.Periode,
            //    Week = y.Week,
            //    Date = y.Date,
            //    Participant = y.Participant,
            //    Trainer = getTrainerConcat(y.id),
            //    NoParticipant = y.NoParticipant,
            //    Days = y.Days,
            //    Hours = y.Hours,

            //}).OrderByDescending(y => y.Date).ToList();

            //var roles = ((ClaimsIdentity)User.Identity).Claims
            //   .Where(c => c.Type == ClaimTypes.Role)
            //   .Select(c => c.Value);
            var frontFld = new rekapFront();
            if(objPassed.isPassed == true)
            {
                if (!string.IsNullOrEmpty(objPassed.error))
                {
                    frontFld.error = objPassed.error;
                }
            }
            var data = new List<dbrekapTrainingfixed>();
            var typeCode = HttpContext.Session.GetString(SessionKeyNameFilter);
            var dbprograms = db.programDb.ToList();
            if (!string.IsNullOrEmpty(typeCode) && !string.IsNullOrEmpty(typeCode))
            {
                data = db.rekapdbFixed.Where(y => y.Date.Value.Year == DateTime.Now.Year).Where(y => y.FLAG_AKTIF != "0" && y.Type == typeCode).ToList().ToList().Select(y => new dbrekapTrainingfixed()
                {
                    id = y.id,
                    TRN_ID = y.TRN_ID,
                    Type = y.Type,
                    Program = dbprograms.Where(x => x.ProgramId == y.Program).FirstOrDefault() != null ? dbprograms.Where(x => x.ProgramId == y.Program).FirstOrDefault().ProgramName : "",
                    ProgramTxt = y.ProgramTxt,
                    EDP = y.EDP,
                    NoSertifikat = y.NoSertifikat,
                    Periode = y.Periode,
                    Week = y.Week,
                    Date = y.Date,
                    Participant = y.Participant,
                    Trainer = getTrainerConcat(y.id),
                    NoParticipant = y.NoParticipant,
                    Days = y.Days,
                    Hours = y.Hours,
                    Batch = y.Batch
                }).OrderByDescending(y => y.Date).ToList();
                ViewData["TypeCode"] = typeCode;

            }
          
            else
            {
                ViewData["TypeCode"] = "";
            }
            frontFld.rekapTrainingfixed = data;
            return View(frontFld);
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
        public JsonResult getTblFront(string type)
        {
            HttpContext.Session.SetString(SessionKeyNameFilter, type);
            var dbprograms = db.programDb.ToList();
            //Creating List    
            List<dbrekapTrainingfixed> TblDt = new List<dbrekapTrainingfixed>();
            TblDt = db.rekapdbFixed.Where(y => y.Date.Value.Year == DateTime.Now.Year).Where(y => y.FLAG_AKTIF != "0" && y.Type == type).ToList().ToList().Select(y => new dbrekapTrainingfixed()
            {
                id = y.id,
                TRN_ID = y.TRN_ID,
                Type = y.Type,
                Program = dbprograms.Where(x => x.ProgramId == y.Program).FirstOrDefault() != null ? dbprograms.Where(x => x.ProgramId == y.Program).FirstOrDefault().ProgramName : "",
                ProgramTxt = y.ProgramTxt,
                EDP = y.EDP,
                NoSertifikat = y.NoSertifikat,
                Periode = y.Periode,
                Week = y.Week,
                Date = y.Date,
                Participant = y.Participant,
                Trainer = getTrainerConcat(y.id),
                NoParticipant = y.NoParticipant,
                Days = y.Days,
                Hours = y.Hours,
                Batch = y.Batch
            }).OrderByDescending(y => y.Date).ToList();
            return Json(TblDt);
        }

        public JsonResult getTbl(string trn_id)
        {
            //Creating List    
            List<dbNilaiSSFixed> TblDt = new List<dbNilaiSSFixed>();
            var tblStaff = db.SSTable.ToList();
            TblDt = db.NilaissTblFixed.Where(y => y.FLAG_AKTIF == "1" && y.TRN_ID == trn_id).ToList().Select(y => new dbNilaiSSFixed()
            {
                ScoreId = y.ScoreId,
                SS_CODE = y.SS_CODE,
                NAMA_SS = tblStaff.Where(x => x.SS_CODE == y.SS_CODE).FirstOrDefault() != null ? tblStaff.Where(x => x.SS_CODE == y.SS_CODE).FirstOrDefault().NAMA_SS : "",
                TRN_ID = y.TRN_ID,
                NILAI = y.NILAI,
                SERTIFIKAT = y.SERTIFIKAT,
                NoSertifikat = y.NoSertifikat,
                FILE_SERTIFIKAT = y.FILE_SERTIFIKAT,
                ISPRESENT = y.ISPRESENT
            }).ToList();
            return Json(TblDt);
        }
        public IActionResult getdataSS(string edp)
        {
            List<VaksinModel> SSList = new List<VaksinModel>();
            if (!string.IsNullOrEmpty(edp))
            {
                SSList = db.SSTable.Where(y => y.FLAG_AKTIF == 1 && y.EDP_CODE == edp).ToList().Select(y => new VaksinModel()
                {
                    SS_CODE = y.SS_CODE,
                    NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
                }).ToList();
            }
            else
            {
                SSList = db.SSTable.Where(y => y.FLAG_AKTIF == 1).ToList().Select(y => new VaksinModel()
                {
                    SS_CODE = y.SS_CODE,
                    NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
                }).ToList();
            }
            return Json(SSList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(dbrekapTrainingfixed objPassed)
        {
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.ToList().Select(y => new XstoreModel()
            {
                edp = y.edp,
                store_location = y.edp + " - " + y.store_location
            }).ToList();
            List<dbTrainer> TrainerList = new List<dbTrainer>();
            TrainerList = db.trainerDb.ToList().Select(y => new dbTrainer()
            {
                idTrainer = y.idTrainer,
                NmTrainer = y.NmTrainer
            }).ToList();
            List<dbProgram> ProgramList = new List<dbProgram>();
            ProgramList = db.programDb.ToList().Select(y => new dbProgram()
            {
                ProgramId = y.ProgramId,
                ProgramName = y.ProgramName
            }).ToList();
            dbrekapTrainingfixed fld = new dbrekapTrainingfixed();
            fld.Date = objPassed.Date;
            fld.ddEdp = edplist;
            fld.ddTrainer = TrainerList;
            fld.Type = objPassed.Type;
            fld.Program = objPassed.Program;
            fld.ProgramTxt = objPassed.ProgramTxt;
            fld.EDP = objPassed.EDP;
            fld.Periode = objPassed.Periode;
            fld.Week = objPassed.Week;
            fld.Date = objPassed.Date;
            fld.idTrainer = objPassed.idTrainer;
            fld.Trainer = objPassed.Trainer;
            fld.Participant = objPassed.Participant;
            fld.NoParticipant = objPassed.NoParticipant;
            fld.Days = objPassed.Days;
            if(objPassed.Hours != null)
            {
                fld.Hours = objPassed.Hours;

            }
            else
            {
                fld.Hours = 0;

            }
            fld.TotalHours = objPassed.TotalHours;
            fld.ParticipantList = objPassed.ParticipantList;
            fld.Batch = objPassed.Batch;
            fld.TrainingDays = objPassed.TrainingDays;
            fld.TrainerList = objPassed.TrainerList;
            
            if (fld.Date != null)
            {
                fld.passingDate = fld.Date.Value.ToString("MM/dd/yyyy");
            }

            List<dbNilaiSSFixed> p = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            if (p == null || !objPassed.isPassed)
            {
                p = new List<dbNilaiSSFixed>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", p);
            }
            List<VaksinModel> SSList = new List<VaksinModel>();
            SSList = db.SSTable.ToList().Select(y => new VaksinModel()
            {
                SS_CODE = y.SS_CODE,
                NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
            }).ToList();
            fld.SSDD = SSList;
            List<dbRD> RDList = new List<dbRD>();
            RDList = db.RDTbl.ToList().Select(y => new dbRD()
            {
                RD_CODE = y.RD_CODE,
                NM_RD =  y.RD_CODE + " - " + y.NM_RD
            }).ToList();

            fld.ddRD = RDList;
            List<dbEmployee> empList = new List<dbEmployee>();
            empList = db.EmpTbl.ToList().Select(y => new dbEmployee()
            {
                EMP_CODE = y.EMP_CODE,
                NM_EMP = y.EMP_CODE + " - " + y.NM_EMP
            }).ToList();
            fld.ddEmp = empList;
            fld.ddProgram = ProgramList;
            return View(fld);
        }
        public IActionResult UploadPeserta([Bind] dbrekapTrainingfixed objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
                if (nilaiSSTbl == null)
                {
                    nilaiSSTbl = new List<dbNilaiSSFixed>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);
                }
                var file = objDetail.fileUploadPeserta;
               

                try
                {
                    if (file == null || file.Length == 0)
                    {
                        objDetail.error = "file not found";
                    }
                    else
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsXls");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        var filePath = Path.Combine(path2, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        bool hasHeader = true;
                        using (var excelPack = new ExcelPackage())
                        {
                            FileInfo fi = new FileInfo(filePath);

                            //Load excel stream
                            using (var stream = fi.OpenRead())
                            {
                                excelPack.Load(stream);
                            }

                            //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                            var ws = excelPack.Workbook.Worksheets[0];

                            //Get all details as DataTable -because Datatable make life easy :)
                            //DataTable excelasTable = new DataTable();
                            //var excelasTable = model.ApplicationUsers;

                            var start = ws.Dimension.Start;
                            var end = ws.Dimension.End;
                            var startrow = start.Row + 1;
                            for (int row = startrow; row <= end.Row; row++)
                            { // Row by row...
                                dbNilaiSSFixed field = new dbNilaiSSFixed();
                                object formid = ws.Cells[row, 1].Value;
                                object participantid = ws.Cells[row, 2].Value;
                                object nilai = ws.Cells[row, 3].Value;
                                object nosertifikat = ws.Cells[row, 4].Value;
                                object presence = ws.Cells[row, 5].Value;
                                object emptype = ws.Cells[row, 6].Value;
                                object certified = ws.Cells[row, 7].Value;
                                if(formid != null)
                                {
                                    field.TRN_ID = formid.ToString();
                                }
                                if (participantid != null)
                                {
                                    field.SS_CODE = participantid.ToString();
                                }
                                if (nilai != null)
                                {
                                    field.NILAI = Convert.ToInt32(nilai);
                                }
                                else
                                {
                                    field.NILAI = 0;
                                }
                                if(nosertifikat != null)
                                {
                                    field.NoSertifikat = nosertifikat.ToString();
                                }
                                if(presence != null)
                                {
                                    field.ISPRESENT = presence.ToString();
                                }
                                else
                                {
                                    field.ISPRESENT = "0";
                                }
                                if (emptype != null)
                                {
                                    field.EMP_TYPE = emptype.ToString();
                                }
                                if (certified != null)
                                {
                                    field.SERTIFIKAT = Convert.ToInt32(certified);
                                }
                                else
                                {
                                    field.SERTIFIKAT = 0;
                                }
                                if (participantid != null)
                                {
                                    nilaiSSTbl.Add(field);
                                }

                            }


                        }

                        FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                        if (filedlt.Exists)//check file exsit or not  
                        {
                            filedlt.Delete();
                        }
                    }
                    objDetail.Type = objDetail.TypeTempUpl;
                    objDetail.Program = objDetail.ProgramTempUpl;
                    objDetail.ProgramTxt = objDetail.ProgramTxtTempUpl;
                    objDetail.EDP = objDetail.EDPTempUpl;
                    objDetail.Periode = objDetail.PeriodeTempUpl;
                    objDetail.Week = objDetail.WeekTempUpl;
                    objDetail.Date = objDetail.DateTempUpl;
                    objDetail.idTrainer = objDetail.idTrainerTempUpl;
                    objDetail.Trainer = objDetail.TrainerTempUpl;
                    objDetail.Participant = objDetail.ParticipantTempUpl;
                    objDetail.NoParticipant = objDetail.NoParticipantTempUpl;
                    objDetail.Days = objDetail.DaysTempUpl;
                    objDetail.Hours = objDetail.HoursTempUpl;
                    objDetail.TotalHours = objDetail.TotalHoursTempUpl;
                    objDetail.ParticipantList = objDetail.ParticipantListTempUpl;
                    objDetail.TrainerList = objDetail.TrainerListTempUpl;
                    objDetail.TrainingDays = objDetail.TrainingDaysTempUpl;
                    objDetail.Batch = objDetail.BatchTempUpl;

                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);

                }
                catch (Exception ex)
                {
                    objDetail.error = ex.InnerException.ToString();
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                return RedirectToAction("Create", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Create", objDetail);
            }
          
        }
        public string getWk(DateTime trainingDate)
        {
            string wk = "";

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(mySqlConnectionStr))
            {
                conn.Open();
                string query = @"select week('" + trainingDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 5) +1 as wk";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        wk = reader["wk"].ToString();

                    }
                }
                conn.Close();
            }
            return wk;
        }
        public IActionResult UploadPesertaAll([Bind] rekapFront objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbrekapTrainingfixed> rekaptrainingtbl = new List<dbrekapTrainingfixed>();
                List<dbNilaiSSFixed> nilaiSSTbl = new List<dbNilaiSSFixed>();

                var file = objDetail.fileUploadPeserta;


                try
                {
                    if (file == null || file.Length == 0)
                    {
                        objDetail.error = "file not found";
                    }
                    else
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsXls");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        var filePath = Path.Combine(path2, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        bool hasHeader = true;
                        using (var excelPack = new ExcelPackage())
                        {
                            FileInfo fi = new FileInfo(filePath);

                            //Load excel stream
                            using (var stream = fi.OpenRead())
                            {
                                excelPack.Load(stream);
                            }

                            //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                            var ws = excelPack.Workbook.Worksheets[0];
                            var ws2 = excelPack.Workbook.Worksheets[1];
                            //Get all details as DataTable -because Datatable make life easy :)
                            //DataTable excelasTable = new DataTable();
                            //var excelasTable = model.ApplicationUsers;

                            var start = ws.Dimension.Start;
                            var end = ws.Dimension.End;
                            var startrow = start.Row + 1;
                            for (int row = startrow; row <= end.Row; row++)
                            {
                                object TYPE = ws.Cells[row, 1].Value;
                                object Program_Id = ws.Cells[row, 2].Value;
                                object EDP = ws.Cells[row, 3].Value;
                                object DateTraining = ws.Cells[row, 4].Value;
                                object Participant = ws.Cells[row, 5].Value;
                                object idtrainer = ws.Cells[row, 6].Value;
                                object Hours = ws.Cells[row, 7].Value;
                                object day = ws.Cells[row, 8].Value;
                                object title = ws.Cells[row, 9].Value;
                                object batch = ws.Cells[row, 10].Value;

                                if (Program_Id != null)
                                {
                                    dbrekapTrainingfixed fld = new dbrekapTrainingfixed();
                                    fld.Type = TYPE.ToString();
                                    fld.Program = Program_Id.ToString();
                                    fld.EDP = EDP.ToString();
                                    fld.Date = Convert.ToDateTime(DateTraining);
                                    fld.Participant = Participant.ToString();
                                    fld.idTrainerXls = idtrainer.ToString();
                                    fld.Hours = Convert.ToInt32(Hours);
                                    fld.TrainingDays = Convert.ToInt32(day);
                                    fld.ProgramTxt = title.ToString();
                                    fld.Batch = Convert.ToInt32(batch);
                                    rekaptrainingtbl.Add(fld);
                                }

                               
                               
                            }
                            var startdtl = ws2.Dimension.Start;
                            var enddtl = ws2.Dimension.End;
                            var startrowdtl = startdtl.Row + 1;
                            for (int rowdtl = startrowdtl; rowdtl <= enddtl.Row; rowdtl++)
                            {
                                object Program_iddtl = ws2.Cells[rowdtl, 1].Value;
                                object SS_CODE = ws2.Cells[rowdtl, 2].Value;
                                object NILAI = ws2.Cells[rowdtl, 3].Value;
                                object NoSertifikat = ws2.Cells[rowdtl, 4].Value;
                                object isPresent = ws2.Cells[rowdtl, 5].Value;
                                object EMP_tYPE = ws2.Cells[rowdtl, 6].Value;
                                object isCertified = ws2.Cells[rowdtl, 7].Value;
                                if (Program_iddtl != null)
                                {
                                    dbNilaiSSFixed field = new dbNilaiSSFixed();
                                    field.Program = Program_iddtl.ToString();
                                    field.SS_CODE = SS_CODE.ToString();
                                    field.NILAI = Convert.ToInt32(NILAI);
                                    if (NoSertifikat != null)
                                    {
                                        field.NoSertifikat = NoSertifikat.ToString();
                                    }
                                    field.ISPRESENT = isPresent.ToString();
                                    field.EMP_TYPE = EMP_tYPE.ToString();
                                    field.SERTIFIKAT = Convert.ToInt32(isCertified);
                                    nilaiSSTbl.Add(field);
                                }

                            }

                        }
                        string test = "";
                        foreach (var fldhdr in rekaptrainingtbl)
                        {
                            fldhdr.Batch = fldhdr.Batch;
                            var countparticipant = nilaiSSTbl.Where(y => y.Program == fldhdr.Program).ToList().Count();
                            var hours = fldhdr.Hours;
                            var totalhours = countparticipant * hours;
                            fldhdr.NoParticipant = countparticipant;
                            fldhdr.Hours = hours;
                            fldhdr.TotalHours = totalhours;
                            fldhdr.TrainingDays = fldhdr.TrainingDays;
                            fldhdr.ProgramTxt = fldhdr.ProgramTxt;

                            if (fldhdr.Date != null)
                            {
                                fldhdr.Periode = fldhdr.Date.Value.ToString("MMMM");
                            }
                            fldhdr.Week = getWk(Convert.ToDateTime(fldhdr.Date));
                            fldhdr.Entry_Date = DateTime.Now;
                            fldhdr.Entry_User = User.Identity.Name;
                            fldhdr.Update_Date = DateTime.Now;
                            fldhdr.Update_User = User.Identity.Name;
                            fldhdr.FLAG_AKTIF = "1";
                            db.rekapdbFixed.Add(fldhdr);
                            db.SaveChanges();
                            int id = fldhdr.id;
                            var trnid = db.rekapdbFixed.Find(id);
                            var chkidtrainer = fldhdr.idTrainerXls.Split(";");
                            if(chkidtrainer.Count() > 1)
                            {
                                foreach(var fldid in chkidtrainer)
                                {
                                    dbTrainerList fldtrainer = new dbTrainerList();
                                    fldtrainer.idFormRekap = id;
                                    fldtrainer.idTrainer = fldid;
                                    db.trainerlistTbl.Add(fldtrainer);
                                }
                               
                            }
                            else
                            {
                                dbTrainerList fldtrainer = new dbTrainerList();
                                fldtrainer.idFormRekap = id;
                                fldtrainer.idTrainer = fldhdr.idTrainerXls;
                                db.trainerlistTbl.Add(fldtrainer);

                            }

                            foreach (var flddtl in nilaiSSTbl)
                            {
                                if (flddtl.Program == fldhdr.Program)
                                {
                                    flddtl.TRN_ID = trnid.TRN_ID;
                                    flddtl.Entry_Date = DateTime.Now;
                                    flddtl.Entry_User = User.Identity.Name;
                                    flddtl.Update_Date = DateTime.Now;
                                    flddtl.Update_User = User.Identity.Name;
                                    flddtl.FLAG_AKTIF = "1";
                                    db.NilaissTblFixed.Add(flddtl);
                                }
                            }

                        }
                        db.SaveChanges();
                        FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                        if (filedlt.Exists)//check file exsit or not  
                        {
                            filedlt.Delete();
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    objDetail.error = ex.InnerException.ToString();
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                objDetail.isPassed = true;
                return RedirectToAction("Index", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                objDetail.isPassed = true;
                objDetail.error = "error";
                return RedirectToAction("Index", objDetail);
            }

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Creates([Bind] dbrekapTrainingfixed objRekap)
        {
            if (ModelState.IsValid)
            {
                if (objRekap.Date != null)
                {
                    objRekap.Periode = objRekap.Date.Value.ToString("MMMM");
                }
                objRekap.Entry_Date = DateTime.Now;
                objRekap.Update_Date = DateTime.Now;
                objRekap.Entry_User = User.Identity.Name;
                objRekap.Update_User = User.Identity.Name;
                objRekap.FLAG_AKTIF = "1";
                objRekap.TRN_ID = "";
                var listParticipant = objRekap.ParticipantList;
                string participants = "";
                foreach(var listfld in listParticipant)
                {
                    participants += listfld;
                    participants += ";";
                }
                objRekap.Participant = participants;
                try
                {
                    using (var context = db)
                    {

                        context.rekapdbFixed.Add(objRekap);
                        context.SaveChanges();
                        int id = objRekap.id; // Yes it's here
                    }
                    var ids = objRekap.id;
                    var trnid = db.rekapdbFixed.Find(ids);
                    List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
                    if (nilaiSSTbl != null)
                    {
                        if (!string.IsNullOrEmpty(objRekap.EDP))
                        {
                            var tblSS = db.SSTable.Where(y => y.EDP_CODE == objRekap.EDP).Select(y => y.SS_CODE).ToList();
                            nilaiSSTbl = nilaiSSTbl.Where(y => tblSS.Contains(y.SS_CODE)).ToList();
                        }
                        int idfld = 0;
                        foreach (var fld in nilaiSSTbl)
                        {
                            dbNilaiSSFixed form = fld;
                            form.TRN_ID = trnid.TRN_ID;
                            form.FLAG_AKTIF = "1";
                            form.Entry_Date = DateTime.Now;
                            form.Update_Date = DateTime.Now;
                            form.Entry_User = User.Identity.Name;
                            form.Update_User = User.Identity.Name;
                            form.ScoreId = idfld;
                            string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                            if (!Directory.Exists(path2))
                            {
                                Directory.CreateDirectory(path2);
                            }
                            var filelist = Directory.GetFiles(path2);
                            foreach (var files in filelist)
                            {
                                var filename = files.Split(path2 + "\\")[1];
                                if(filename == form.FILE_SERTIFIKAT)
                                {
                                    string wwwPath = this.Environment.WebRootPath;
                                    string contentPath = this.Environment.ContentRootPath;

                                    string path3 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                                    if (!Directory.Exists(path3))
                                    {
                                        Directory.CreateDirectory(path3);
                                    }
                                    string fname1 = fld.FILE_SERTIFIKAT;

                                    var filePath = Path.Combine(path3, fname1);
                                    System.IO.Directory.Move(files, filePath);
                                }
                            }
                            db.NilaissTblFixed.Add(form);
                        }
                        foreach(var trnlist in objRekap.TrainerList)
                        {
                            if (!string.IsNullOrEmpty(trnlist))
                            {
                                dbTrainerList fld = new dbTrainerList();
                                fld.idTrainer = trnlist;
                                fld.idFormRekap = ids;
                                db.trainerlistTbl.Add(fld);
                            }
                        }
                        //foreach (var dtl in formDtlDt)
                        //{
                        //    dtl.IS_DISCOUNT = "N";

                        //    var prodDt = db.mt_prod.Find(dtl.PROD_ID);
                        //    if (prodDt != null)
                        //    {
                        //        dtl.PRICE_ORI = prodDt.PRICE;

                        //    }

                        //}
                        //formDt.mt_trans_dtl = formDtlDt;
                        db.SaveChanges();
                    }


                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                //apprDal.AddApproval(objApproval);
                return RedirectToAction("Index");
            }
            return View(objRekap);
        }
        [Authorize]
        public IActionResult Edit(int id, dbrekapTrainingfixed objPassed)
        {
           
            List<XstoreModel> edplist = new List<XstoreModel>();
            edplist = db.xstore_organization.ToList().Select(y => new XstoreModel()
            {
                edp = y.edp,
                store_location = y.edp + " - " + y.store_location
            }).ToList();
            List<dbTrainer> TrainerList = new List<dbTrainer>();
            TrainerList = db.trainerDb.ToList().Select(y => new dbTrainer()
            {
                idTrainer = y.idTrainer,
                NmTrainer = y.NmTrainer
            }).ToList();
            List<dbProgram> ProgramList = new List<dbProgram>();
            ProgramList = db.programDb.ToList().Select(y => new dbProgram()
            {
                ProgramId = y.ProgramId,
                ProgramName = y.ProgramName
            }).ToList();
            if (objPassed.isPassed)
            {
                id = objPassed.id;
            }
            dbrekapTrainingfixed fld = db.rekapdbFixed.Find(id);
            var participants = fld.Participant;
            var participantsplit = participants.Split(";");
            List<string> listParticipant = new List<string>();
            var trainerlist = db.trainerlistTbl.Where(y => y.idFormRekap == id).ToList();
            List<string> trnlst = new List<string>();
            foreach(var flds in trainerlist)
            {
                trnlst.Add(flds.idTrainer);
            }
            fld.TrainerList = trnlst;
            if(participantsplit.Count() > 0)
            {
                foreach(var flds in participantsplit)
                {
                    if (!string.IsNullOrEmpty(flds))
                    {
                        listParticipant.Add(flds);
                    }
                }
            }
            fld.ParticipantList = listParticipant;
            List<dbNilaiSSFixed> p = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
            if (p == null)
            {
                p = db.NilaissTblFixed.Where(y => y.TRN_ID == fld.TRN_ID).ToList();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", p);
            }
            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                if (objPassed.isPassed)
                {
                    fld.Date = objPassed.Date;
                    fld.ddEdp = edplist;
                    fld.ddTrainer = TrainerList;
                    fld.Type = objPassed.Type;
                    fld.Program = objPassed.Program;
                    fld.ProgramTxt = objPassed.ProgramTxt;
                    fld.EDP = objPassed.EDP;
                    fld.Periode = objPassed.Periode;
                    fld.Week = objPassed.Week;
                    fld.idTrainer = objPassed.idTrainer;
                    fld.Trainer = objPassed.Trainer;
                    fld.Participant = objPassed.Participant;
                    fld.NoParticipant = objPassed.NoParticipant;
                    fld.Days = objPassed.Days;
                    fld.Hours = objPassed.Hours;
                    fld.TotalHours = objPassed.TotalHours;
                    fld.ParticipantList = objPassed.ParticipantList;
                    fld.Batch = objPassed.Batch;
                    fld.TrainingDays = objPassed.TrainingDays;
                    fld.TrainerList = objPassed.TrainerList;
                }
                else
                {
                    p = db.NilaissTblFixed.Where(y => y.TRN_ID == fld.TRN_ID).ToList();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", p);
                }
                if (fld.Date != null)
                {
                    fld.passingDate = fld.Date.Value.ToString("MM/dd/yyyy");
                }
                fld.ddEdp = edplist;
                fld.ddTrainer = TrainerList;
                List<VaksinModel> SSList = new List<VaksinModel>();
                SSList = db.SSTable.ToList().Select(y => new VaksinModel()
                {
                    SS_CODE = y.SS_CODE,
                    NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
                }).ToList();
                fld.SSDD = SSList;
                List<dbRD> RDList = new List<dbRD>();
                RDList = db.RDTbl.ToList().Select(y => new dbRD()
                {
                    RD_CODE = y.RD_CODE,
                    NM_RD = y.RD_CODE + " - " + y.NM_RD
                }).ToList();

                fld.ddRD = RDList;
                List<dbEmployee> empList = new List<dbEmployee>();
                empList = db.EmpTbl.ToList().Select(y => new dbEmployee()
                {
                    EMP_CODE = y.EMP_CODE,
                    NM_EMP = y.EMP_CODE + " - " + y.NM_EMP
                }).ToList();
                fld.ddEmp = empList;
                fld.ddProgram = ProgramList;
            }
            return View(fld);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edits(int id, [Bind] dbrekapTrainingfixed fld)
        {
          
            if (ModelState.IsValid)
            {
                var editFld = db.rekapdbFixed.Find(id);
                editFld.Type = fld.Type;
                editFld.Program = fld.Program;
                editFld.ProgramTxt = fld.ProgramTxt;
                editFld.EDP = fld.EDP;
                if(fld.Date != null)
                {
                    editFld.Periode = fld.Date.Value.ToString("MMMM");
                }
                editFld.Week = fld.Week;
                editFld.Date = fld.Date;
                editFld.Participant = fld.Participant;
                editFld.Trainer = fld.Trainer;
                editFld.idTrainer = fld.idTrainer;
                editFld.NoParticipant = fld.NoParticipant;
                editFld.Days = fld.Days;
                editFld.Hours = fld.Hours;
                editFld.TotalHours = fld.TotalHours;
                editFld.Batch = fld.Batch;
                editFld.TrainingDays = fld.TrainingDays;

                var listParticipant = fld.ParticipantList;
                string participants = "";
                foreach (var listfld in listParticipant)
                {
                    participants += listfld;
                    participants += ";";
                }
                editFld.Participant = participants;
                editFld.Update_Date = DateTime.Now;
                editFld.Update_User = User.Identity.Name;
                List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
                try
                {
                    //Processing UserData
                  
                    List<string> listtrans = new List<string>();
                    List<string> listExistingtrans = new List<string>();

                    var idToRemovetrans = new List<string>();
                    var idToAddtrans = new List<string>();

                    var ExistingTransDt = db.NilaissTblFixed.Where(y => y.TRN_ID == editFld.TRN_ID).ToList();
                    for (int i = 0; i < nilaiSSTbl.Count(); i++)
                    {
                        var idDtl = nilaiSSTbl[i].SS_CODE;
                        listtrans.Add(idDtl);
                        idToAddtrans.Add(idDtl);
                    }

                    foreach (var exist in ExistingTransDt)
                    {
                        var transExist = exist.SS_CODE;
                        listExistingtrans.Add(transExist);
                        idToRemovetrans.Add(transExist);

                    }

                    //removing logic 
                    for (int i = 0; i < listExistingtrans.Count(); i++)
                    {
                        var nopekExist = listExistingtrans[i];
                        for (int y = 0; y < listtrans.Count(); y++)
                        {
                            var nopekNew = listtrans[y];
                            if (nopekExist == nopekNew)
                            {
                                idToRemovetrans.Remove(nopekExist);
                            }
                        }
                    }
                  
                    var empDt = ExistingTransDt.Where(y => idToRemovetrans.Contains(y.SS_CODE)).ToList<dbNilaiSSFixed>();
                    foreach (var dtlemp in empDt)
                    {
                        foreach (var idtoremove in idToRemovetrans)
                        {
                            var formEmp = dtlemp;
                            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                            if (!string.IsNullOrEmpty(formEmp.FILE_SERTIFIKAT))
                            {
                                FileInfo file = new FileInfo(Path.Combine(path2, formEmp.FILE_SERTIFIKAT));
                                if (file.Exists)//check file exsit or not  
                                {
                                    file.Delete();
                                }
                            }
                            db.NilaissTblFixed.Remove(formEmp);
                        }
                    }
                    //adding logic
                    for(int i = 0; i < nilaiSSTbl.Count(); i++)
                    {
                        var dts = nilaiSSTbl[i];
                        var iddts = dts.ScoreId;
                        if (iddts != 0)
                        {
                            var formtrans = db.NilaissTblFixed.Find(iddts);
                            formtrans.SS_CODE = dts.SS_CODE;
                            formtrans.NILAI = dts.NILAI;
                            formtrans.SERTIFIKAT = dts.SERTIFIKAT;
                            formtrans.ISPRESENT = dts.ISPRESENT;
                            formtrans.NoSertifikat = dts.NoSertifikat;
                            formtrans.EMP_TYPE = dts.EMP_TYPE;
                            var filesertifikatOld = formtrans.FILE_SERTIFIKAT;
                            var filesertifikatnew = dts.FILE_SERTIFIKAT;
                            if (filesertifikatnew != filesertifikatOld)
                            {

                                string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                                if (!Directory.Exists(path2))
                                {
                                    Directory.CreateDirectory(path2);
                                }
                                string path3 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                                if (!string.IsNullOrEmpty(filesertifikatOld))
                                {
                                    FileInfo file = new FileInfo(Path.Combine(path2, filesertifikatOld));
                                    if (file.Exists)//check file exsit or not  
                                    {
                                        file.Delete();
                                    }
                                }

                                //string fname1 = filesertifikatnew;
                                //var filelist = Directory.GetFiles(path2);
                                //foreach (var files in filelist)
                                //{
                                //    var filename = files.Split(path2 + "\\")[1];
                                //    if (filename == fname1)
                                //    {
                                //        var filePath = Path.Combine(path3, fname1);
                                //        System.IO.Directory.Move(files, filePath);
                                //    }
                                //}
                                formtrans.FILE_SERTIFIKAT = filesertifikatnew;
                            }
                            db.NilaissTblFixed.Update(formtrans);
                        }
                        else
                        {
                            dbNilaiSSFixed form = dts;
                            form.TRN_ID = editFld.TRN_ID;
                            form.FLAG_AKTIF = "1";
                            form.Entry_Date = DateTime.Now;
                            form.Update_Date = DateTime.Now;
                            form.Entry_User = User.Identity.Name;
                            form.Update_User = User.Identity.Name;
                            string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                            if (!Directory.Exists(path2))
                            {
                                Directory.CreateDirectory(path2);
                            }
                            var filelist = Directory.GetFiles(path2);
                            foreach (var files in filelist)
                            {
                                var filename = files.Split(path2 + "\\")[1];
                                if (filename == form.FILE_SERTIFIKAT)
                                {
                                    string wwwPath = this.Environment.WebRootPath;
                                    string contentPath = this.Environment.ContentRootPath;

                                    string path3 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                                    if (!Directory.Exists(path3))
                                    {
                                        Directory.CreateDirectory(path3);
                                    }
                                    string fname1 = dts.FILE_SERTIFIKAT;

                                    var filePath = Path.Combine(path3, fname1);
                                    System.IO.Directory.Move(files, filePath);
                                }
                            }
                            db.NilaissTblFixed.Add(form);
                        
                        }
                    }

                    //Processing UserData

                    List<string> listtrainer = new List<string>();
                    List<string> listExistingTrainer = new List<string>();

                    var idToRemovetrainer = new List<string>();
                    var idToAddtrainer = new List<string>();

                    var ExistingTrainerDt = db.trainerlistTbl.Where(y => y.idFormRekap == editFld.id).ToList();
                    var trainerNewTbls = fld.TrainerList;
                    List<dbTrainerList> trainerNewTbl = new List<dbTrainerList>();
                    foreach(var fields in trainerNewTbls)
                    {
                        dbTrainerList col = new dbTrainerList();
                        var extfield = ExistingTrainerDt.Where(y => y.idTrainer == fields).FirstOrDefault();
                        if(extfield != null)
                        {
                            col.idFormRekap = editFld.id;
                            col.idTrainer = extfield.idTrainer;
                            col.idTrainerList = extfield.idTrainerList;
                        }
                        else
                        {
                            col.idFormRekap = editFld.id;
                            col.idTrainer = fields;
                        }
                        trainerNewTbl.Add(col);
                    }
                    for (int i = 0; i < trainerNewTbl.Count(); i++)
                    {
                        var idDtl = trainerNewTbl[i].idTrainer;
                        listtrainer.Add(idDtl);
                        idToAddtrainer.Add(idDtl);
                    }

                    foreach (var exist in ExistingTrainerDt)
                    {
                        var transExist = exist.idTrainer;
                        listExistingTrainer.Add(transExist);
                        idToRemovetrainer.Add(transExist);
                    }

                    //removing logic 
                    for (int i = 0; i < listExistingTrainer.Count(); i++)
                    {
                        var trainerExist = listExistingTrainer[i];
                        for (int y = 0; y < listtrainer.Count(); y++)
                        {
                            var trainerNew = listtrainer[y];
                            if (trainerExist == trainerNew)
                            {
                                idToRemovetrainer.Remove(trainerExist);
                            }
                        }
                    }

                    var trainerDts = ExistingTrainerDt.Where(y => idToRemovetrainer.Contains(y.idTrainer)).ToList<dbTrainerList>();
                    foreach (var dtlemp in trainerDts)
                    {
                        foreach (var idtoremove in idToRemovetrainer)
                        {
                            var formEmp = dtlemp;                          
                            db.trainerlistTbl.Remove(formEmp);
                        }
                    }
                    //adding logic
                    for (int i = 0; i < trainerNewTbl.Count(); i++)
                    {
                        var dts = trainerNewTbl[i];
                        var iddts = dts.idTrainerList;
                        if (iddts != 0)
                        {
                            var formtrans = db.trainerlistTbl.Find(iddts);
                            formtrans.idFormRekap = editFld.id;
                            formtrans.idTrainer = dts.idTrainer;
                            db.trainerlistTbl.Update(formtrans);
                        }
                        else
                        {
                            dbTrainerList form = dts;
                            db.trainerlistTbl.Add(form);

                        }
                    }

                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgEdit" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                return RedirectToAction("Index");
            }
            return View(fld);
        }

        public IActionResult UploadPesertaEdit([Bind] dbrekapTrainingfixed objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
                if (nilaiSSTbl == null)
                {
                    nilaiSSTbl = new List<dbNilaiSSFixed>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", nilaiSSTbl);
                }
                var file = objDetail.fileUploadPeserta;


                try
                {
                    if (file == null || file.Length == 0)
                    {
                        objDetail.error = "file not found";
                    }
                    else
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsXls");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        var filePath = Path.Combine(path2, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        bool hasHeader = true;
                        using (var excelPack = new ExcelPackage())
                        {
                            FileInfo fi = new FileInfo(filePath);

                            //Load excel stream
                            using (var stream = fi.OpenRead())
                            {
                                excelPack.Load(stream);
                            }

                            //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                            var ws = excelPack.Workbook.Worksheets[0];

                            //Get all details as DataTable -because Datatable make life easy :)
                            //DataTable excelasTable = new DataTable();
                            //var excelasTable = model.ApplicationUsers;

                            var start = ws.Dimension.Start;
                            var end = ws.Dimension.End;
                            var startrow = start.Row + 1;
                            for (int row = startrow; row <= end.Row; row++)
                            { // Row by row...
                                dbNilaiSSFixed field = new dbNilaiSSFixed();
                                object formid = ws.Cells[row, 1].Value;
                                object participantid = ws.Cells[row, 2].Value;
                                
                                object nilai = ws.Cells[row, 3].Value;
                                object nosertifikat = ws.Cells[row, 4].Value;
                                object presence = ws.Cells[row, 5].Value;
                                object emptype = ws.Cells[row, 6].Value;
                                object certified = ws.Cells[row, 7].Value;
                                if(formid != null)
                                {
                                    field.TRN_ID = formid.ToString();
                                }
                                else
                                {
                                    field.TRN_ID = objDetail.TRN_IDtempUpl;
                                }

                                if (participantid != null)
                                {
                                    field.SS_CODE = participantid.ToString();
                                }

                                if (nilai != null)
                                {
                                    field.NILAI = Convert.ToInt32(nilai);
                                }
                                else
                                {
                                    field.NILAI = 0;
                                }
                                if (nosertifikat != null)
                                {
                                    field.NoSertifikat = nosertifikat.ToString();
                                }
                                if (presence != null)
                                {
                                    field.ISPRESENT = presence.ToString();
                                }
                                else
                                {
                                    field.ISPRESENT = "0";
                                }
                                if (emptype != null)
                                {
                                    field.EMP_TYPE = emptype.ToString();
                                }
                                if (certified != null)
                                {
                                    field.SERTIFIKAT = Convert.ToInt32(certified);
                                }
                                else
                                {
                                    field.SERTIFIKAT = 0;
                                }
                                if(participantid != null)
                                {
                                    var validate = nilaiSSTbl.Where(y => y.SS_CODE == participantid.ToString()).FirstOrDefault();
                                    if (validate == null)
                                    {
                                        nilaiSSTbl.Add(field);
                                    }

                                }
                              
                            }


                        }

                        FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                        if (filedlt.Exists)//check file exsit or not  
                        {
                            filedlt.Delete();
                        }
                    }
                    objDetail.Type = objDetail.TypeTempUpl;
                    objDetail.Program = objDetail.ProgramTempUpl;
                    objDetail.ProgramTxt = objDetail.ProgramTxtTempUpl;
                    objDetail.EDP = objDetail.EDPTempUpl;
                    objDetail.Periode = objDetail.PeriodeTempUpl;
                    objDetail.Week = objDetail.WeekTempUpl;
                    objDetail.Date = objDetail.DateTempUpl;
                    objDetail.idTrainer = objDetail.idTrainerTempUpl;
                    objDetail.Trainer = objDetail.TrainerTempUpl;
                    objDetail.Participant = objDetail.ParticipantTempUpl;
                    objDetail.NoParticipant = objDetail.NoParticipantTempUpl;
                    objDetail.Days = objDetail.DaysTempUpl;
                    objDetail.Hours = objDetail.HoursTempUpl;
                    objDetail.TotalHours = objDetail.TotalHoursTempUpl;
                    objDetail.ParticipantList = objDetail.ParticipantListTempUpl;
                    objDetail.TrainerList = objDetail.TrainerListTempUpl;
                    objDetail.TrainingDays = objDetail.TrainingDaysTempUpl;
                    objDetail.Batch = objDetail.BatchTempUpl;
                    objDetail.TRN_ID = objDetail.TRN_IDtempUpl;
                    objDetail.id = objDetail.idtempUpl;
                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", nilaiSSTbl);

                }
                catch (Exception ex)
                {
                    objDetail.error = ex.InnerException.ToString();
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                return RedirectToAction("Edit", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Edit", objDetail);
            }

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            
            dbrekapTrainingfixed fld = db.rekapdbFixed.Find(id);
            
            if (fld == null)
            {
                return NotFound();
            }
            return View(fld);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRekap(int id)
        {
            dbrekapTrainingfixed fld = db.rekapdbFixed.Find(id);

            if (fld == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    //db.trainerDb.Remove(fld);
                    fld.FLAG_AKTIF = "0";
                    fld.Update_Date = DateTime.Now;
                    fld.Update_User = User.Identity.Name;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgEdit" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult ScoreDBPartial(string command)
        {
            dbRekapTraining Hdr = new dbRekapTraining();
            //var SSCode = HttpContext.Session.GetString(SessionKeyName2);
            List<dbNilaiSSFixed> p = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            if(p == null)
            {
                p = new List<dbNilaiSSFixed>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", p);
            }
            //List<dbNilaiSSFixed> p = Session["FormDtls"] as List<dbNilaiSSFixed>;

            string[] results = command.Split(';');
            string action = results[0];
            int index = Convert.ToInt32(results[1]);
            List<VaksinModel> SSList = new List<VaksinModel>();
            SSList = db.SSTable.ToList().Select(y => new VaksinModel()
            {
                SS_CODE = y.SS_CODE,
                NAMA_SS = y.EDP_CODE + " - " + y.SS_CODE + " - " + y.NAMA_SS
            }).ToList();
            if (action == "add")
            {
                p.Add(new dbNilaiSSFixed());
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", p);
            }
            else
            {
                p.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", p);
                //var idDelete = Request.QueryString["deleteid"];
                //if (!string.IsNullOrEmpty(idDelete))
                //{
                //    var id = Convert.ToDecimal(idDelete);
                //    if (id != 0)
                //    {
                //        List<mt_trans_dtl> dtForm = Session["FormDtls"] as List<mt_trans_dtl>;
                //        var removeField = dtForm.Where(y => y.DTL_ID == id).FirstOrDefault();

                //        dtForm.Remove(removeField);

                //    }
                //}
            }
            Hdr.nilaiSSFixedList = p;
            Hdr.SSDD = SSList;
            //var dtProd = db.mt_prod.OrderBy(y => y.PROD_NAME).ToList();
            //Hdr.prodDD = dtProd;

            return PartialView(Hdr);
        }

        //public IActionResult GetLengthForm()
        //{
        //    List<dbNilaiSSFixed> p = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
        //    if (p == null)
        //    {
        //        p = new List<dbNilaiSSFixed>();
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", p);
        //    }
        //    decimal? number = 0;
        //    number = p.Count();

        //}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDetail([Bind] dbrekapTrainingfixed objDetail)
        {
            if (ModelState.IsValid)
            {
                List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
                if (nilaiSSTbl == null)
                {
                    nilaiSSTbl = new List<dbNilaiSSFixed>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);
                }
                dbNilaiSSFixed fld = new dbNilaiSSFixed();
                if(objDetail.EMP_TYPE == "AM" || objDetail.EMP_TYPE == "DM" || objDetail.EMP_TYPE == "EMP")
                {
                    fld.SS_CODE = objDetail.EMP_CODE;

                }
                else
                if(objDetail.EMP_TYPE == "RD")
                {
                    fld.SS_CODE = objDetail.RD_CODE;
                }
                else
                if (objDetail.EMP_TYPE == "SS")
                {
                    fld.SS_CODE = objDetail.SS_CODE;
                }
                fld.NILAI = objDetail.NILAI;
                fld.NoSertifikat = objDetail.NoSertifikat;
                fld.EMP_TYPE = objDetail.EMP_TYPE;
                if (objDetail.isCertified)
                {
                    fld.SERTIFIKAT = 1;
                }
                else
                {
                    fld.SERTIFIKAT = 0;
                }
                if (objDetail.isPresent)
                {
                    fld.ISPRESENT = "1";
                }
                else
                {
                    fld.ISPRESENT = "0";
                }
                if (objDetail.fileSertifikat != null)
                {
                    string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat.FileName);
                    fld.FILE_SERTIFIKAT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }

                try
                {
                    nilaiSSTbl.Add(fld);
                    var fileSertifikat = objDetail.fileSertifikat;
                    if (fileSertifikat != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat.FileName);
                        string fname1 = fld.FILE_SERTIFIKAT;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileSertifikat.CopyTo(stream);
                        }
                    }
                    objDetail.Type = objDetail.TypeTemp;
                    objDetail.Program = objDetail.ProgramTemp;
                    objDetail.ProgramTxt = objDetail.ProgramTxtTemp;
                    objDetail.EDP = objDetail.EDPTemp;
                    objDetail.Periode = objDetail.PeriodeTemp;
                    objDetail.Week = objDetail.WeekTemp;
                    objDetail.Date = objDetail.DateTemp;
                    objDetail.idTrainer = objDetail.idTrainerTemp;
                    objDetail.Trainer = objDetail.TrainerTemp;
                    objDetail.Participant = objDetail.ParticipantTemp;
                    objDetail.NoParticipant = objDetail.NoParticipantTemp;
                    objDetail.Days = objDetail.DaysTemp;
                    objDetail.Hours = objDetail.HoursTemp;
                    objDetail.TotalHours = objDetail.TotalHoursTemp;
                    objDetail.ParticipantList = objDetail.ParticipantListTemp;
                    objDetail.TrainerList = objDetail.TrainerListTemp;
                    objDetail.TrainingDays = objDetail.TrainingDaysTemp;
                    objDetail.Batch = objDetail.BatchTemp;

                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);

                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                return RedirectToAction("Create", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Create", objDetail);
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDetail([Bind] dbrekapTrainingfixed objDetail)
        {
            if (ModelState.IsValid)
            {
                var splitdatamode = objDetail.datamode.Split("EditPeserta");
                if(splitdatamode.Length > 1)
                {
                    List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
                    foreach (var fld in nilaiSSTbl)
                    {
                        var idpeserta = "";
                        if (objDetail.EMP_TYPE_EDIT == "AM" || objDetail.EMP_TYPE_EDIT == "DM" || objDetail.EMP_TYPE_EDIT == "EMP")
                        {
                            idpeserta = objDetail.EMP_CODE_EDIT;
                        }
                        else if (objDetail.EMP_TYPE_EDIT == "RD")
                        {
                            idpeserta = objDetail.RD_CODE_EDIT;
                        }
                        else if (objDetail.EMP_TYPE_EDIT == "SS")
                        {
                            idpeserta = objDetail.SS_CODE_EDIT;
                        }
                        if (fld.SS_CODE == idpeserta)
                        {
                            //dbNilaiSSFixed fld = nilaiSSTbl.Where(y => y.SS_CODE == objDetail.SS_CODE_EDIT).FirstOrDefault();
                            fld.NILAI = objDetail.NILAI_EDIT;
                            fld.NoSertifikat = objDetail.NoSertifikat_EDIT;
                            fld.EMP_TYPE = objDetail.EMP_TYPE_EDIT;

                            if (objDetail.isCertified_EDIT)
                            {
                                fld.SERTIFIKAT = 1;
                            }
                            else
                            {
                                fld.SERTIFIKAT = 0;
                            }
                            if (objDetail.isPresent_EDIT)
                            {
                                fld.ISPRESENT = "1";
                            }
                            else
                            {
                                fld.ISPRESENT = "0";
                            }
                            if (objDetail.fileSertifikat_EDIT != null)
                            {
                                string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat_EDIT.FileName);
                                string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat_EDIT.FileName);
                                string wwwPath = this.Environment.WebRootPath;
                                string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                                if (!string.IsNullOrEmpty(fld.FILE_SERTIFIKAT))
                                {
                                    FileInfo file = new FileInfo(Path.Combine(path2, fld.FILE_SERTIFIKAT));
                                    if (file.Exists)//check file exsit or not  
                                    {
                                        file.Delete();
                                    }
                                }
                               
                                fld.FILE_SERTIFIKAT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                            }
                            try
                            {
                                //nilaiSSTbl.Add(fld);
                                var fileSertifikat = objDetail.fileSertifikat_EDIT;
                                if (fileSertifikat != null)
                                {
                                    string wwwPath = this.Environment.WebRootPath;
                                    string contentPath = this.Environment.ContentRootPath;

                                    string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                                    if (!Directory.Exists(path2))
                                    {
                                        Directory.CreateDirectory(path2);
                                    }
                                    string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat_EDIT.FileName);
                                    string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat_EDIT.FileName);
                                    string fname1 = fld.FILE_SERTIFIKAT;

                                    var filePath = Path.Combine(path2, fname1);
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        fileSertifikat.CopyTo(stream);
                                    }
                                }


                                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);

                            }
                            catch (Exception ex)
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                                if (!Directory.Exists(filePath))
                                {
                                    Directory.CreateDirectory(filePath);
                                }
                                using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                                {
                                    outputFile.WriteLine(ex.ToString());
                                }
                            }
                        }

                    }
                }
                else
                {
                    List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
                    var fld = nilaiSSTbl.Where(y => y.SS_CODE == objDetail.SS_CODE_EDIT).FirstOrDefault();
                    string wwwPath = this.Environment.WebRootPath;
                    string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                    if (!string.IsNullOrEmpty(fld.FILE_SERTIFIKAT))
                    {
                        FileInfo file = new FileInfo(Path.Combine(path2, fld.FILE_SERTIFIKAT));
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                 
                    nilaiSSTbl.Remove(fld);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);

                }

                objDetail.Type = objDetail.TypeTemp_EDIT;
                objDetail.Program = objDetail.ProgramTemp_EDIT;
                objDetail.ProgramTxt = objDetail.ProgramTxtTemp_EDIT;
                objDetail.EDP = objDetail.EDPTemp_EDIT;
                objDetail.Periode = objDetail.PeriodeTemp_EDIT;
                objDetail.Week = objDetail.WeekTemp_EDIT;
                objDetail.Date = objDetail.DateTemp_EDIT;
                objDetail.idTrainer = objDetail.idTrainerTemp_EDIT;
                objDetail.Trainer = objDetail.TrainerTemp_EDIT;
                objDetail.Participant = objDetail.ParticipantTemp_EDIT;
                objDetail.NoParticipant = objDetail.NoParticipantTemp_EDIT;
                objDetail.Days = objDetail.DaysTemp_EDIT;
                objDetail.Hours = objDetail.HoursTemp_EDIT;
                objDetail.TotalHours = objDetail.TotalHoursTemp_EDIT;
                objDetail.ParticipantList = objDetail.ParticipantListTemp_EDIT;
                objDetail.TrainingDays = objDetail.TrainingDaysTemp_EDIT;
                objDetail.TrainerList = objDetail.TrainerListTemp_EDIT;
                objDetail.Batch = objDetail.BatchTemp_EDIT;
                objDetail.isPassed = true;
                return RedirectToAction("Create", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Create", objDetail);
            }
        }
        public JsonResult getTblDtl()
        {

            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            foreach(var tbl in nilaiSSTbl)
            {
                if(tbl.EMP_TYPE == "AM" || tbl.EMP_TYPE == "DM" || tbl.EMP_TYPE == "EMP")
                {
                    if (db.EmpTbl.Where(y => y.EMP_CODE == tbl.SS_CODE).FirstOrDefault() != null)
                    {
                        tbl.NAMA_SS = db.EmpTbl.Where(y => y.EMP_CODE == tbl.SS_CODE).FirstOrDefault().NM_EMP;
                    }

                }
                else
                    if(tbl.EMP_TYPE == "SS")
                {
                    if(db.SSTable.Where(y => y.SS_CODE == tbl.SS_CODE).FirstOrDefault() != null)
                    {
                        tbl.NAMA_SS = db.SSTable.Where(y => y.SS_CODE == tbl.SS_CODE).FirstOrDefault().NAMA_SS;

                    }

                }
                else
                if(tbl.EMP_TYPE == "RD")
                {
                    if(db.RDTbl.Where(y => y.RD_CODE == tbl.SS_CODE).FirstOrDefault() != null)
                    {
                        tbl.NAMA_SS = db.RDTbl.Where(y => y.RD_CODE == tbl.SS_CODE).FirstOrDefault().NM_RD;
                    }

                }
            }
            return Json(nilaiSSTbl);
        }
        public JsonResult getTblDtlbyEDP(string edpcode)
        {

            //Creating List
            var tblSS = db.SSTable.Select(y => y.SS_CODE).ToList();
            if (!string.IsNullOrEmpty(edpcode))
            {
                tblSS = db.SSTable.Where(y => y.EDP_CODE == edpcode).Select(y => y.SS_CODE).ToList();
            }
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            nilaiSSTbl = nilaiSSTbl.Where(y => tblSS.Contains(y.SS_CODE)).ToList();
            foreach (var tbl in nilaiSSTbl)
            {
                tbl.NAMA_SS = db.SSTable.Where(y => y.SS_CODE == tbl.SS_CODE).FirstOrDefault().NAMA_SS;
            }
            return Json(nilaiSSTbl);
        }
        public JsonResult getTblDtlbyEDPEdit(string edpcode)
        {

            //Creating List
            var tblSS = db.SSTable.Select(y => y.SS_CODE).ToList();
            if (!string.IsNullOrEmpty(edpcode))
            {
                tblSS = db.SSTable.Where(y => y.EDP_CODE == edpcode).Select(y => y.SS_CODE).ToList();
            }
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
            nilaiSSTbl = nilaiSSTbl.Where(y => tblSS.Contains(y.SS_CODE)).ToList();
            foreach (var tbl in nilaiSSTbl)
            {
                tbl.NAMA_SS = db.SSTable.Where(y => y.SS_CODE == tbl.SS_CODE).FirstOrDefault().NAMA_SS;
            }
            return Json(nilaiSSTbl);
        }
        public JsonResult resetTblDtl()
        {

            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            nilaiSSTbl = new List<dbNilaiSSFixed>();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);
            return Json(nilaiSSTbl);
        }
        public JsonResult resetTblDtlEdit()
        {

            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
            nilaiSSTbl = new List<dbNilaiSSFixed>();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "FormList", nilaiSSTbl);
            return Json(nilaiSSTbl);
        }

        public JsonResult validatesscode(string sscode, string btnid)
        {
            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            string stat = "pass";
            if (btnid != "addPeserta")
            {
                ////nilaiSSTbl = nilaiSSTbl.Where(y => y.SS_CODE != sscode).ToList();
                //var fld = nilaiSSTbl.Where(y => y.SS_CODE == sscode).FirstOrDefault();
                //if(fld != null)
                //{
                    
                //}
                //var validate = nilaiSSTbl.Where(y => y.SS_CODE == sscode).ToList();
                //if (validate.Count() > 0)
                //{
                //    stat = "nopass";
                //}
            }
            else
            {
                var validate = nilaiSSTbl.Where(y => y.SS_CODE == sscode).ToList();
                if (validate.Count() > 0)
                {
                    stat = "nopass";
                }
            }
           
            return Json(stat);
        }
        public JsonResult getSSEditAdd(string sscode)
        {

            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormList");
            dbNilaiSSFixed fld = new dbNilaiSSFixed();
            if (nilaiSSTbl != null)
            {
                fld = nilaiSSTbl.Where(y => y.SS_CODE == sscode).FirstOrDefault();
            }
            return Json(fld);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDetailEdit([Bind] dbrekapTrainingfixed objDetail)
        {
            //if(objDetail.TotalHours == "NaN")
            //{
            //    objDetail.TotalHours = 0;
            //}
            if (ModelState.IsValid)
            {
                List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
                if (nilaiSSTbl == null)
                {
                    nilaiSSTbl = new List<dbNilaiSSFixed>();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", nilaiSSTbl);
                }
                dbNilaiSSFixed fld = new dbNilaiSSFixed();
                fld.TRN_ID = objDetail.TRN_IDtemp;
                if (objDetail.EMP_TYPE == "AM" || objDetail.EMP_TYPE == "DM" || objDetail.EMP_TYPE == "EMP")
                {
                    fld.SS_CODE = objDetail.EMP_CODE;
                }
                else
                if (objDetail.EMP_TYPE == "RD")
                {
                    fld.SS_CODE = objDetail.RD_CODE;
                }
                else
                if (objDetail.EMP_TYPE == "SS")
                {
                    fld.SS_CODE = objDetail.SS_CODE;
                }
                fld.NILAI = objDetail.NILAI;
                fld.NoSertifikat = objDetail.NoSertifikat;
                fld.EMP_TYPE = objDetail.EMP_TYPE;

                if (objDetail.isCertified)
                {
                    fld.SERTIFIKAT = 1;
                }
                else
                {
                    fld.SERTIFIKAT = 0;
                }
                if (objDetail.isPresent)
                {
                    fld.ISPRESENT = "1";
                }
                else
                {
                    fld.ISPRESENT = "0";
                }
                if (objDetail.fileSertifikat != null)
                {
                    string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat.FileName);
                    string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat.FileName);
                    fld.FILE_SERTIFIKAT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                }

                try
                {
                    nilaiSSTbl.Add(fld);
                    var fileSertifikat = objDetail.fileSertifikat;
                    if (fileSertifikat != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                        string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat.FileName);
                        string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat.FileName);
                        string fname1 = fld.FILE_SERTIFIKAT;

                        var filePath = Path.Combine(path2, fname1);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            fileSertifikat.CopyTo(stream);
                        }
                    }
                    objDetail.Type = objDetail.TypeTemp;
                    objDetail.Program = objDetail.ProgramTemp;
                    objDetail.ProgramTxt = objDetail.ProgramTxtTemp;
                    objDetail.EDP = objDetail.EDPTemp;
                    objDetail.Periode = objDetail.PeriodeTemp;
                    objDetail.Week = objDetail.WeekTemp;
                    objDetail.Date = objDetail.DateTemp;
                    objDetail.idTrainer = objDetail.idTrainerTemp;
                    objDetail.Trainer = objDetail.TrainerTemp;
                    objDetail.Participant = objDetail.ParticipantTemp;
                    objDetail.NoParticipant = objDetail.NoParticipantTemp;
                    objDetail.Days = objDetail.DaysTemp;
                    objDetail.Hours = objDetail.HoursTemp;
                    objDetail.TotalHours = objDetail.TotalHoursTemp;
                    objDetail.id = objDetail.idtemp;
                    objDetail.TRN_ID = objDetail.TRN_IDtemp;
                    objDetail.ParticipantList = objDetail.ParticipantListTemp;
                    objDetail.Batch = objDetail.BatchTemp;
                    objDetail.TrainingDays = objDetail.TrainingDaysTemp;
                    objDetail.TrainerList = objDetail.TrainerListTemp;

                    objDetail.isPassed = true;
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", nilaiSSTbl);

                }
                catch (Exception ex)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                    {
                        outputFile.WriteLine(ex.ToString());
                    }
                }
                //return RedirectToAction("Edit",objDetail);
                return RedirectToAction("Edit", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Edit", objDetail);
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDetailEdit([Bind] dbrekapTrainingfixed objDetail)
        {
            if (ModelState.IsValid)
            {
                var splitdatamode = objDetail.datamode.Split("EditPeserta");
                if (splitdatamode.Length > 1)
                {
                    List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
                    foreach (var fld in nilaiSSTbl)
                    {
                        var idpeserta = "";
                        if(objDetail.EMP_TYPE_EDIT == "AM" || objDetail.EMP_TYPE_EDIT == "DM" || objDetail.EMP_TYPE_EDIT == "EMP")
                        {
                            idpeserta = objDetail.EMP_CODE_EDIT;
                        } else if (objDetail.EMP_TYPE_EDIT == "RD")
                        {
                            idpeserta = objDetail.RD_CODE_EDIT;
                        } else if (objDetail.EMP_TYPE_EDIT == "SS")
                        {
                            idpeserta = objDetail.SS_CODE_EDIT;
                        }
                        if (fld.SS_CODE == idpeserta)
                        {
                            //dbNilaiSSFixed fld = nilaiSSTbl.Where(y => y.SS_CODE == objDetail.SS_CODE_EDIT).FirstOrDefault();
                            fld.NILAI = objDetail.NILAI_EDIT;
                            fld.NoSertifikat = objDetail.NoSertifikat_EDIT;
                            fld.EMP_TYPE = objDetail.EMP_TYPE_EDIT;

                            if (objDetail.isCertified_EDIT)
                            {
                                fld.SERTIFIKAT = 1;
                            }
                            else
                            {
                                fld.SERTIFIKAT = 0;
                            }
                            if (objDetail.isPresent_EDIT)
                            {
                                fld.ISPRESENT = "1";
                            }
                            else
                            {
                                fld.ISPRESENT = "0";
                            }
                            if (objDetail.fileSertifikat_EDIT != null)
                            {
                                string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat_EDIT.FileName);
                                string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat_EDIT.FileName);
                                string wwwPath = this.Environment.WebRootPath;
                                fld.FILE_SERTIFIKAT = fname + (DateTime.Now).ToString("ddMMyyyyhhmmss") + ext;
                            }
                            try
                            {
                                //nilaiSSTbl.Add(fld);
                                if(objDetail.ScoreId != 0)
                                {
                                    var fileSertifikat = objDetail.fileSertifikat_EDIT;
                                    if (fileSertifikat != null)
                                    {
                                        string wwwPath = this.Environment.WebRootPath;
                                        string contentPath = this.Environment.ContentRootPath;

                                        string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                                        if (!Directory.Exists(path2))
                                        {
                                            Directory.CreateDirectory(path2);
                                        }
                                        string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat_EDIT.FileName);
                                        string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat_EDIT.FileName);
                                        string fname1 = fld.FILE_SERTIFIKAT;

                                        var filePath = Path.Combine(path2, fname1);
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            fileSertifikat.CopyTo(stream);
                                        }
                                    }
                                }
                                else
                                {
                                    var fileSertifikat = objDetail.fileSertifikat_EDIT;
                                    if (fileSertifikat != null)
                                    {
                                        string wwwPath = this.Environment.WebRootPath;
                                        string contentPath = this.Environment.ContentRootPath;

                                        string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                                        if (!Directory.Exists(path2))
                                        {
                                            Directory.CreateDirectory(path2);
                                        }
                                        string ext = System.IO.Path.GetExtension(objDetail.fileSertifikat_EDIT.FileName);
                                        string fname = System.IO.Path.GetFileNameWithoutExtension(objDetail.fileSertifikat_EDIT.FileName);
                                        string fname1 = fld.FILE_SERTIFIKAT;

                                        var filePath = Path.Combine(path2, fname1);
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            fileSertifikat.CopyTo(stream);
                                        }
                                    }
                                }
                              


                                SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", nilaiSSTbl);

                            }
                            catch (Exception ex)
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\ErrorLog");
                                if (!Directory.Exists(filePath))
                                {
                                    Directory.CreateDirectory(filePath);
                                }
                                using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath, "ErrMsgAdd" + (DateTime.Now).ToString("dd-MM-yyyy HH-mm-ss") + ".txt")))
                                {
                                    outputFile.WriteLine(ex.ToString());
                                }
                            }
                        }

                    }
                }
                else
                {
                    List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
                    var fld = nilaiSSTbl.Where(y => y.SS_CODE == objDetail.SS_CODE_EDIT).FirstOrDefault();
                    string wwwPath = this.Environment.WebRootPath;
                    if (fld.ScoreId == 0)
                    {
                        string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
                        if (!string.IsNullOrEmpty(fld.FILE_SERTIFIKAT))
                        {
                            FileInfo file = new FileInfo(Path.Combine(path2, fld.FILE_SERTIFIKAT));
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                        }
                    }
                    //else
                    //{
                    //    string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
                    //    if (!string.IsNullOrEmpty(fld.FILE_SERTIFIKAT))
                    //    {
                    //        FileInfo file = new FileInfo(Path.Combine(path2, fld.FILE_SERTIFIKAT));
                    //        if (file.Exists)//check file exsit or not  
                    //        {
                    //            file.Delete();
                    //        }
                    //    }

                    //}
                    nilaiSSTbl.Remove(fld);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "FormListEdit", nilaiSSTbl);
                }

                objDetail.Type = objDetail.TypeTemp_EDIT;
                objDetail.Program = objDetail.ProgramTemp_EDIT;
                objDetail.ProgramTxt = objDetail.ProgramTxtTemp_EDIT;
                objDetail.EDP = objDetail.EDPTemp_EDIT;
                objDetail.Periode = objDetail.PeriodeTemp_EDIT;
                objDetail.Week = objDetail.WeekTemp_EDIT;
                objDetail.Date = objDetail.DateTemp_EDIT;
                objDetail.idTrainer = objDetail.idTrainerTemp_EDIT;
                objDetail.Trainer = objDetail.TrainerTemp_EDIT;
                objDetail.Participant = objDetail.ParticipantTemp_EDIT;
                objDetail.NoParticipant = objDetail.NoParticipantTemp_EDIT;
                objDetail.Days = objDetail.DaysTemp_EDIT;
                objDetail.Hours = objDetail.HoursTemp_EDIT;
                objDetail.TotalHours = objDetail.TotalHoursTemp_EDIT;
                objDetail.id = objDetail.idtemp_EDIT;
                objDetail.TRN_ID = objDetail.TRN_IDtemp_EDIT;
                objDetail.ParticipantList = objDetail.ParticipantListTemp_EDIT;
                objDetail.Batch = objDetail.BatchTemp_EDIT;
                objDetail.TrainingDays = objDetail.TrainingDaysTemp_EDIT;
                objDetail.TrainerList = objDetail.TrainerListTemp_EDIT;

                objDetail.isPassed = true;
                return RedirectToAction("Edit", objDetail);
                ////apprDal.AddApproval(objApproval);
                //return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Edit", objDetail);
            }
        }
        public JsonResult getTblDtlEdit()
        {

            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
            foreach (var tbl in nilaiSSTbl)
            {
                if (tbl.EMP_TYPE == "AM" || tbl.EMP_TYPE == "DM" || tbl.EMP_TYPE == "EMP")
                {
                    if (db.EmpTbl.Where(y => y.EMP_CODE == tbl.SS_CODE).FirstOrDefault() != null)
                    {
                        tbl.NAMA_SS = db.EmpTbl.Where(y => y.EMP_CODE == tbl.SS_CODE).FirstOrDefault().NM_EMP;
                    }

                }
                else
                    if (tbl.EMP_TYPE == "SS")
                {
                    if(db.SSTable.Where(y => y.SS_CODE == tbl.SS_CODE).FirstOrDefault() != null)
                    {
                        tbl.NAMA_SS = db.SSTable.Where(y => y.SS_CODE == tbl.SS_CODE).FirstOrDefault().NAMA_SS;
                    }

                }
                else
                    if (tbl.EMP_TYPE == "RD")
                {
                    if(db.RDTbl.Where(y => y.RD_CODE == tbl.SS_CODE).FirstOrDefault() != null)
                    {
                        tbl.NAMA_SS = db.RDTbl.Where(y => y.RD_CODE == tbl.SS_CODE).FirstOrDefault().NM_RD;

                    }

                }
            }
            return Json(nilaiSSTbl);
        }
        public JsonResult validatesscodeEdit(string sscode, string btnid)
        {
            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
            string stat = "pass";
            if (btnid != "addPeserta")
            {
                ////nilaiSSTbl = nilaiSSTbl.Where(y => y.SS_CODE != sscode).ToList();
                //var fld = nilaiSSTbl.Where(y => y.SS_CODE == sscode).FirstOrDefault();
                //if(fld != null)
                //{

                //}
                //var validate = nilaiSSTbl.Where(y => y.SS_CODE == sscode).ToList();
                //if (validate.Count() > 0)
                //{
                //    stat = "nopass";
                //}
            }
            else
            {
                var validate = nilaiSSTbl.Where(y => y.SS_CODE == sscode).ToList();
                if (validate.Count() > 0)
                {
                    stat = "nopass";
                }
            }

            return Json(stat);
        }
        public JsonResult getSSEditAddEdit(string sscode, string trn_id)
        {

            //Creating List    
            List<dbNilaiSSFixed> nilaiSSTbl = SessionHelper.GetObjectFromJson<List<dbNilaiSSFixed>>(HttpContext.Session, "FormListEdit");
            dbNilaiSSFixed fld = new dbNilaiSSFixed();
            if (nilaiSSTbl != null)
            {
                fld = nilaiSSTbl.Where(y => y.SS_CODE == sscode && y.TRN_ID == trn_id).FirstOrDefault();
            }
            return Json(fld);
        }
        public async Task<IActionResult> DownloadSertifikatTemp([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "TempSertifikat");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }
        public async Task<IActionResult> DownloadSertifikat([FromQuery(Name = "fname")] string filename)
        {

            if (filename == null)
                return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);
            string path2 = Path.Combine(this.Environment.WebRootPath, "UploadsSertifikat");
            var filePath = Path.Combine(path2, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        public string transpose()
        {
            string stat = "ok";
            var dbRekap = db.rekapdbFixed.ToList();
            try
            {
                foreach (var fld in dbRekap)
                {
                    var traininglist = fld.Trainer;
                    var splitList = traininglist.Split(",");
                    var hitung = splitList.Count();
                    if (splitList.Count() == 1)
                    {
                        dbTrainerList newdt = new dbTrainerList();
                        newdt.idFormRekap = fld.id;
                        newdt.idTrainer = splitList[0].Trim();
                        db.trainerlistTbl.Add(newdt);
                    }
                    else
                    {
                        foreach (var dts in splitList)
                        {
                            dbTrainerList newdt = new dbTrainerList();
                            newdt.idFormRekap = fld.id;
                            newdt.idTrainer = dts.Trim();
                            db.trainerlistTbl.Add(newdt);
                        }
                    }
                }
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                stat = ex.ToString();
            }
         

            return stat;
        }
    }
}
