using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyUploadPage.Pages
{
    public class UploadModel : PageModel
    {
        [BindProperty]
        public IFormFile UploadFile { get; set; }

        public IActionResult OnPost([FromServices] IWebHostEnvironment env)
        {

            var filePath = Path.Combine(env.ContentRootPath, "Files", UploadFile.FileName);
            using (var stream = System.IO.File.Create(filePath)) 
            {
                UploadFile.CopyTo(stream);
            }
            return RedirectToPage("Index");
        }
    }
}