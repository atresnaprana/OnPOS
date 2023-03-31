using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BataAppHR.Models;
using BataAppHR.Data;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BataAppHR.Controllers
{
    public class UploadUserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UploadUserController> _logger;
        private IHostingEnvironment Environment;
        const string SessionName = "_Name";


        public UploadUserController(ILogger<UploadUserController> logger, IHostingEnvironment _environment, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            Environment = _environment;
            _userManager = userManager;

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterLotsOfPeople(IFormFile file)
        {
            RegisterLotsModel model = new RegisterLotsModel();
            model.ApplicationUsers = new List<UserToRegister>();
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
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
                    await file.CopyToAsync(stream);
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
                        UserToRegister field = new UserToRegister();
                        object Username = ws.Cells[row, 1].Value; 
                        object Email = ws.Cells[row, 2].Value; 
                        object Password = ws.Cells[row, 3].Value;
                        field.UserName = Username.ToString();
                        field.Email = Email.ToString();
                        field.Password = Password.ToString();
                        model.ApplicationUsers.Add(field);

                        //for (int col = start.Column; col <= end.Column; col++)
                        //{ // ... Cell by cell...
                        //    object cellValue = ws.Cells[row, col].Text; // This got me the actual value I needed.
                        //}
                    }
                    //foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    //{
                    //    //Get colummn details
                    //    if (!string.IsNullOrEmpty(firstRowCell.Text))
                    //    {
                    //        string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
                    //        //excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
                    //    }
                    //}
                    //var startRow = hasHeader ? 2 : 1;
                    ////Get row details
                    //for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    //{
                    //    //var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                    //    //DataRow row = excelasTable.Rows.Add();
                    //    //foreach (var cell in wsRow)
                    //    //{
                    //    //    row[cell.Start.Column - 1] = cell.Text;
                    //    //}
                    //}
                    //Get everything as generics and let end user decides on casting to required type
                    //var generatedType = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(excelasTable));
                    //return (T)Convert.ChangeType(generatedType, typeof(T));
                }
                var successful = new List<string>();
                var failed = new List<string>();
                foreach (var toRegister in model.ApplicationUsers)
                {
                    var user = new IdentityUser { UserName = toRegister.UserName.Trim(), Email = toRegister.Email.Trim(), EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(user, toRegister.Password);
                    var validateisexist = _userManager.FindByEmailAsync(toRegister.Email.Trim());
                    if(validateisexist == null)
                    {
                        if (result.Succeeded)
                        {
                            successful.Add(toRegister.UserName);
                        }
                        else
                        {
                            failed.Add(toRegister.UserName + " Errorsystem");
                        }
                    }
                    else
                    {
                        var isinRole = _userManager.IsInRoleAsync(user, "User");

                        if (!isinRole.Result)
                        {
                            var result1 = await _userManager.AddToRoleAsync(user, "User");
                            if (result1.Succeeded)
                            {
                                successful.Add(toRegister.UserName + " added role");
                            }
                            else
                            {
                                failed.Add(toRegister.UserName + " failed to add role");

                            }
                        }
                        else
                        {
                            failed.Add(toRegister.UserName + " exist + role added");

                        }

                    }

                }
                FileInfo filedlt = new FileInfo(Path.Combine(path2, file.FileName));
                if (filedlt.Exists)//check file exsit or not  
                {
                    filedlt.Delete();
                }
                return Json(new { SuccessfullyRegistered = successful, FailedToRegister = failed });
            }

        }
    }
}
