using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyUploadPage.Models
{
    public class Doc
    {
        
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Extention { get; set; }
        public int Length { get; set; }
        public byte[] Data { get; set; }

    }
}
