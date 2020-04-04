using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyUploadPage.Data;
using MyUploadPage.Models;

namespace MyUploadPage.Pages.Docs
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<Doc> Docs { get; set; }

        public void OnGet()
        {
            Docs = _db.Docs.ToList();
        }
    }
}