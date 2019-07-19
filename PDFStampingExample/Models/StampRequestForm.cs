using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PDFStampingExample.Models
{
    public class StampRequestForm
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public float LowerLeftX { get; set; }
        public float LowerLeftY { get; set; }
        public float UpperRightX { get; set; }
        public float UpperRightY { get; set; }
        [Required]
        public IFormFile Pdf { get; set; }
        public int RotationDegree { get; set;  }

    }
}
