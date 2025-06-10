using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.EmailModel
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
