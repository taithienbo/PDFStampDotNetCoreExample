using System;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PDFStampingExample.Models;

namespace PDFStampingExample.Services
{
    public class StampService : IStampService
    {
        private const int FontSize = 7;
        private readonly BaseFont Font = BaseFont.CreateFont();
        private readonly BaseColor Color = BaseColor.Red;
        private const int VerticalSpaceBetweenLines = 15;

        public StampService()
        {
        }

        /// <summary>
        /// Stamp a PDF with the specific message and position as specified in the <see cref="StampRequest"/>
        /// https://stackoverflow.com/questions/2372041/c-sharp-itextsharp-pdf-creation-with-watermark-on-each-page#
        /// </summary>
        /// <param name="stampRequest"></param>
        /// <returns></returns>
        public Stream ApplyStamp(StampRequestForm stampRequest)
        {
            MemoryStream pdfOutStream;
            Stream pdfInstream = null;
            PdfReader reader = null;
            PdfStamper stamper = null;
            try
            {
                pdfOutStream = new MemoryStream();
                pdfInstream = stampRequest.Pdf.OpenReadStream();
                reader = new PdfReader(pdfInstream);
                stamper = new PdfStamper(reader, pdfOutStream);
                var dc = stamper.GetOverContent(1);
                AddWaterMark(dc, reader, stampRequest);
                return pdfOutStream;
            }
            finally
            {
                if (pdfInstream != null)
                {
                    pdfInstream.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
                if (stamper != null)
                {
                    stamper.Close();
                }
            }
        }

        private void AddWaterMark(PdfContentByte dc, PdfReader reader, StampRequestForm stampRequest)
        {
            var gstate = new PdfGState
            {
                FillOpacity = 0.61f,
                StrokeOpacity = 0.61f
            };
            dc.SaveState();
            dc.SetGState(gstate);
            dc.SetColorFill(Color);
            dc.BeginText();
            dc.SetFontAndSize(Font, FontSize);
            var x = (stampRequest.LowerLeftX + stampRequest.UpperRightX) / 2;
            var y = ((stampRequest.LowerLeftY + stampRequest.UpperRightY) / 2);
            var lines = new string[] {stampRequest.Line1, stampRequest.Line2,
                                      stampRequest.Line3}
            .Where(line => !string.IsNullOrEmpty(line));
            foreach (var line in lines)
            {
                dc.ShowTextAligned(Element.ALIGN_CENTER, line, x, y, stampRequest.RotationDegree);
                y -= VerticalSpaceBetweenLines;
            }
            dc.EndText();
            dc.RestoreState();
        }
    }
}
