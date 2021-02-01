using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyUploadPage.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MyUploadPage.Controllers
{
    public class DocController : Controller
    {
        private readonly AppDbContext _db;

        public DocController(AppDbContext db)
        {
            _db = db;
        }

        [Route("api/doc/file/{id}/{FileName}")]
        public IActionResult GetFile(Guid id,string fileName)
        {
            var doc = _db.Docs.Find(id);
            return File(doc.Data,doc.ContentType,doc.FileName);
        }

        [Route("api/doc/image/{id}/{width}x{height}/{FileName?}")]
        public IActionResult GetImage(int id,int width,int height)
        {
            var doc = _db.Docs.Find(id);
            if (doc == null)
            {
                return NotFound();
            }

            Stream imageStream = new MemoryStream();

            using (Image<Rgba32> image = Image.Load(doc.Data))
            {
                image.Mutate(x => x
                     .Resize(width, height));
                image.SaveAsJpeg(imageStream);

            }

            imageStream.Position = 0;

            return File(imageStream, doc.ContentType);
        }
    }
}