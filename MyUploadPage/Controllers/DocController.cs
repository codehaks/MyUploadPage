using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
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
        public IActionResult GetFile(int id, string fileName)
        {
            var doc = _db.Docs.Find(id);
            return File(doc.Data, doc.ContentType, doc.FileName);
        }

        public IActionResult GetFileStream()
        {

            using TransactionScope transactionScope2 = new TransactionScope();

            SqlConnection sqlConnection3 = new SqlConnection("Data Source=.;Initial Catalog = FileSystemDB; Integrated Security = True");

            SqlCommand sqlCommand3 = sqlConnection3.CreateCommand();
            sqlCommand3.CommandText = "Select FileData.PathName() As Path,GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext From PictureTable Where PkId = (Select Max(PkId) From PictureTable)";
            sqlConnection3.Open();
            SqlDataReader reader = sqlCommand3.ExecuteReader();
            reader.Read();
            string filePath = (string)reader["Path"];
            byte[] transactionContext2 = (byte[])reader["TransactionContext"];
            SqlFileStream sqlFileStream2 = new SqlFileStream
                (filePath, transactionContext2, FileAccess.Read);
            byte[] data = new byte[sqlFileStream2.Length];
            sqlFileStream2.Read(data, 0, Convert.ToInt16(sqlFileStream2.Length));
            Guid valueInserted = new Guid(data);
            sqlFileStream2.Close();


            return Ok();
        }

        [Route("api/doc/image/{id}/{width}x{height}/{FileName?}")]
        public IActionResult GetImage(int id, int width, int height)
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