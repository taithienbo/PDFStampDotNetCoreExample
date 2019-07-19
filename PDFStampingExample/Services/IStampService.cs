using System;
using System.IO;
using PDFStampingExample.Models;

namespace PDFStampingExample.Services
{
    public interface IStampService
    {
        Stream ApplyStamp(StampRequestForm stampRequest);
    }
}
