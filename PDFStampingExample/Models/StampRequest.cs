using System;
using System.IO;

namespace PDFStampingExample.Models
{
    public class StampRequest
    {
        public string[] Lines { get; set; }
        public Rectangle Container { get; set; }
        public int RotationDegree { get; set; }

        public Stream Pdf { get; set; }

    }
}
