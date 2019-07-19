using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PDFStampingExample.Models;
using PDFStampingExample.Services;

namespace PDFStampingExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : Controller
    {
        private readonly IStampService _stampService; 

        public PDFController(IStampService stampService)
        {
            _stampService = stampService; 
        }

        [HttpPost]
        [Route("stamp")]
        [Produces(contentType: "application/pdf")]
        public FileStreamResult Stamp([FromForm] StampRequestForm stampRequestForm)
        { 
            Stream outStream = _stampService.ApplyStamp(stampRequestForm);
            outStream.Position = 0;
            return new FileStreamResult(outStream, "application/pdf");
        }

    }
}
