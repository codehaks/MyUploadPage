using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MyUploadPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public IList<string> Files { get; set; }
        public void OnGet([FromServices] IWebHostEnvironment env)
        {
            var folderPath = Path.Combine(env.ContentRootPath, "Files");
            Files = System.IO.Directory.EnumerateFiles(folderPath).ToList();
        }
    }
}
