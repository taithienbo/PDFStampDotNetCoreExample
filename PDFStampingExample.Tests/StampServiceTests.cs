using System;
using System.IO;
using PDFStampingExample.Models;
using PDFStampingExample.Services;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace PDFStampingExample.Tests
{
    public class StampServiceTest
    {
        private const string SamplePdfFilePath = "./Resources/sample.pdf";
        private readonly ITestOutputHelper _output;
        private IStampService _stampService;

        public StampServiceTest(ITestOutputHelper output)
        {
            _output = output;
            _stampService = new StampService();
        }

        [Fact]
        public void ApplyStamp()
        {
            using (var inputPdfStream = new FileStream(SamplePdfFilePath, FileMode.Open))
            using (var outStream = new FileStream(SamplePdfFilePath.Replace("pdf", "out.pdf"), FileMode.Create))
            {
                StampRequestForm stampRequest = new StampRequestForm();
                stampRequest.Line1 = "This is a sample stamp";
                stampRequest.Line2 = DateTime.Now.ToShortDateString();
                stampRequest.Line3 =  "https://taithienbo.com" ;
                stampRequest.Pdf = new FormFile(inputPdfStream, 0, inputPdfStream.Length,
                    null, Path.GetFileName(inputPdfStream.Name))
                {
                    Headers = new HeaderDictionary(), 
                    ContentType = "application/pdf"
                };
                // position the stamp text near the top right. 
                stampRequest.LowerLeftX = 402;
                stampRequest.LowerLeftY = 600;
                stampRequest.UpperRightX = 575;
                stampRequest.UpperRightY = 900;

                var resultPdfStream = _stampService.ApplyStamp(stampRequest);
                Assert.NotNull(resultPdfStream);
                resultPdfStream.Seek(0, SeekOrigin.Begin);
                resultPdfStream.CopyTo(outStream);
            }
        }
    }
}
