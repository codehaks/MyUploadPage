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
        public IList<IFormFile> UploadFiles { get; set; }

        public async Task<IActionResult> OnPost([FromServices] IWebHostEnvironment env)
        {


            foreach (var file in UploadFiles)
            {
                var filePath = Path.Combine(env.ContentRootPath, "Files", file.FileName);

                using var stream = System.IO.File.Create(filePath);

                await file.CopyToAsync(stream);
            }

            return RedirectToPage("Index");
        }
    }
}