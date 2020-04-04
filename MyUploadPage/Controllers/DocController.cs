using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyUploadPage.Data;

namespace MyUploadPage.Controllers
{
    public class DocController : Controller
    {
        private readonly AppDbContext _db;

        public DocController(AppDbContext db)
        {
            _db = db;
        }

        [Route("api/doc/{id}/{filename?}")]
        public IActionResult GetFile(int id,string fileName)
        {
            var doc = _db.Docs.Find(id);
            return File(doc.Data,doc.ContentType,doc.FileName);
        }
    }
}