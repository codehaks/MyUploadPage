using System;
using System.IO;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using MyUploadPage.Data;
using SixLabors.ImageSharp;
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

        [Route("api/doc/file/stream/{docId}")]
        public IActionResult GetFileStream(string docId)
        {

            using TransactionScope ts = new TransactionScope();

            SqlConnection scon = new SqlConnection(@"Data Source=CODEHAKS\MSSQL2019;Initial Catalog=UploadDb02;integrated security=true");

            SqlCommand cmd = scon.CreateCommand();
            cmd.CommandText = "Select FileName,ContentType, Data.PathName() As Path,GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext From Docs Where Id ='" + docId+"'";

            scon.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string filePath = (string)reader["Path"];
            string fileName = (string)reader["FileName"];
            string contentType = (string)reader["ContentType"];
            var tranContext = (byte[])reader["TransactionContext"];

            var sqlFileStream = new SqlFileStream(filePath, tranContext, FileAccess.Read);

            byte[] data = new byte[sqlFileStream.Length];

            sqlFileStream.Read(data, 0, Convert.ToInt32(sqlFileStream.Length));
            sqlFileStream.Close();


            return File(data,contentType,fileName,true);
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