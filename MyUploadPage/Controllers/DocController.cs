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

        private static async Task StreamCopyFilesAsync()
        {
            byte[] buffer = new byte[16 * 1024];
            var files = System.IO.Directory.GetFiles(@"E:\Projects\Data\1m_faces_00\1m_faces_00");

            var c = 0;
            foreach (var file in files)
            {
                Console.WriteLine(file);
                long totalBytes = new FileInfo(file).Length;

                using var output = System.IO.File.Create(@"E:\Projects\Data\faces2\" + c + ".jpg");

                Stream input = new FileStream(file, FileMode.Open);
                long totalReadBytes = 0;
                int readBytes;

                while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await output.WriteAsync(buffer, 0, readBytes);
                    totalReadBytes += readBytes;
                }

                c++;



            }
        }

        [Route("api/doc/file/stream/{docId}")]
        public async Task<IActionResult> GetFileStream(string docId)
        {

            using TransactionScope transactionScope2 = new TransactionScope();

            SqlConnection sqlConnection3 = new SqlConnection(@"Data Source=CODEHAKS\MSSQL2019;Initial Catalog=UploadDb02;integrated security=true");

            SqlCommand sqlCommand3 = sqlConnection3.CreateCommand();
            sqlCommand3.CommandText = "Select FileName,ContentType, Data.PathName() As Path,GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext From Docs Where Id ='" + docId+"'";

            sqlConnection3.Open();
            SqlDataReader reader = sqlCommand3.ExecuteReader();
            reader.Read();
            string filePath = (string)reader["Path"];
            string fileName = (string)reader["FileName"];
            string contentType = (string)reader["ContentType"];
            var tranContext = (byte[])reader["TransactionContext"];

            var sqlFileStream = new SqlFileStream(filePath, tranContext, FileAccess.Read);

            //-------------------
            byte[] buffer = new byte[16 * 1024];
            long totalBytes = sqlFileStream.Length;

            var output = new MemoryStream();

            long totalReadBytes = 0;
            int readBytes;

            while ((readBytes = sqlFileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                await output.WriteAsync(buffer, 0, readBytes);
                totalReadBytes += readBytes;
            }

            //-------------------

            //byte[] data = new byte[sqlFileStream.Length];
            //sqlFileStream.Read(data, 0, Convert.ToInt32(sqlFileStream.Length));

            sqlFileStream.Close();

            output.Position = 0;
            return File(output,contentType,fileName,true);
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