using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyUploadPage.Pages
{
    public class DownloadModel : PageModel
    {
        
        public IActionResult OnGet(string fileName, [FromServices] IWebHostEnvironment env)
        {
            // Identity
            var filePath = Path.Combine(env.ContentRootPath, "Files", fileName);
            return new PhysicalFileResult(filePath, "image/jpeg");
        }
    }
}