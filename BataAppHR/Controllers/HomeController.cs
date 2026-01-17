    using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BataAppHR.Models;
using BataAppHR.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BataAppHR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FormDBContext db;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, FormDBContext db, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.db = db;
            _userManager = userManager;


        }
        public async Task<IActionResult> ResetTest()
        {
            var user = await _userManager.FindByEmailAsync("aditya.tresnaprana@bata.com");

            if (user == null)
            {
                return NotFound("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                "Welcome123.."
            );

            if (!result.Succeeded)
            {
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return Ok("Password reset successful");
        }
        public IActionResult Index()
        {
            var data = new List<dbSliderImg>();
            data = db.SlideTbl.Where(y => y.FLAG_AKTIF == "1").OrderBy(y => y.IMG_DESC).ToList().Select(y => new dbSliderImg()
            {
                ID = y.ID,
                IMG_DESC = y.IMG_DESC,
                FILE_NAME = y.FILE_NAME,
                imgUrl = y.SLIDE_IMG_BLOB != null ? returnImgUrl(y.SLIDE_IMG_BLOB):""
            }).ToList();

            return View(data);
        }
        public string returnImgUrl(byte[] imgBlob)
        {
            string imageBase64Data = Convert.ToBase64String(imgBlob);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            return imageDataURL;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}
