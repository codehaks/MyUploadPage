using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyUploadPage.Data;

namespace MyUploadPage.Pages.Docs
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _db;

        public DetailsModel(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet(int id)
        {
            var doc = _db.Docs.Find(id);
            return File(doc.Data, doc.ContentType, doc.FileName);
        }
    }
}