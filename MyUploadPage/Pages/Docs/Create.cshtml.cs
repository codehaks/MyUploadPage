using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyUploadPage.Data;
using MyUploadPage.Models;

namespace MyUploadPage.Pages.Docs
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;

        public CreateModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {

        }

        [BindProperty]
        public IFormFile UploadFile { get; set; }

        public IActionResult OnPost()
        {
            var doc = new Doc
            {
                FileName = UploadFile.FileName,
                ContentType = UploadFile.ContentType,
                Extention = new FileInfo(UploadFile.FileName).Extension,
                Length = (int)UploadFile.Length,
                Data = null

            };

            using var stream = new MemoryStream();
            UploadFile.CopyTo(stream);
            stream.Position = 0;
            doc.Data = stream.ToArray();

            _db.Docs.Add(doc);
            _db.SaveChanges();
            return Redirect("Index");
        }
    }
}