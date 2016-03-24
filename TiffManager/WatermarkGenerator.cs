using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Winnovative.PdfCreator;

namespace ScanManager
{
    public class WatermarkGenerator
    {
        public static byte[] AddWatermarkToPDF(byte[] pdf, string watermarkText, int fontSize, System.Drawing.Color color, int distanceFromRight, int distanceFromTop)
        {
            byte[] result = null;
            LicensingManager.LicenseKey = "lL+mtKWhtKOnrbSjuqS0p6W6paa6ra2trQ==";

            MemoryStream stream = new MemoryStream();
            stream.Write(pdf, 0, pdf.Length);

            Document document = new Document(stream);

            if (document.Pages.Count > 0)
            {
                PdfPage firstPage = document.Pages[0];

                PdfFont watermarkTextFont = document.AddFont(StdFontBaseFamily.HelveticaBold);
                watermarkTextFont.Size = fontSize;

                TextElement watermarkTextElement = new TextElement(0, 0, watermarkText, watermarkTextFont);
                watermarkTextElement.ForeColor = color;
                watermarkTextElement.Transparency = 100;


                Template watermarkTemplate = document.AddTemplate(new System.Drawing.RectangleF(firstPage.ClientRectangle.Width - firstPage.MeasureString(watermarkText, watermarkTextFont).Width - distanceFromRight, distanceFromTop, firstPage.MeasureString(watermarkText, watermarkTextFont).Width, firstPage.MeasureString(watermarkText, watermarkTextFont).Height));
                watermarkTemplate.AddElement(watermarkTextElement);
                
                    foreach (PdfPage page in document.Pages)
                    {
                        page.CustomHeaderTemplate = watermarkTemplate;
                        page.ShowHeaderTemplate = true;
                    }

                result = document.Save();
            }

            document.Close();

            return result;
            // return null;
            #region ITextSharp
            //PdfReader pr = new PdfReader(pdf);
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using(PdfStamper stamper = new PdfStamper(pr, ms)){

            //    for (int i = 1; i <= pr.NumberOfPages; i++)
            //    {
            //        Rectangle rect = pr.GetPageSizeWithRotation(i);

            //        PdfContentByte originalPageContent = stamper.GetUnderContent(i);
            //        originalPageContent.BeginText();
            //        BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
            //        originalPageContent.SetFontAndSize(font, fontSize);

            //        originalPageContent.SetRGBColorFill(color.R, color.G, color.B);
            //        originalPageContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, watermarkText, rect.Width-distanceFromRight, rect.Top-distanceFromTop, 0);
            //        originalPageContent.EndText();
            //        stamper.FormFlattening = true;
            //    }
            //    }
            //       return ms.ToArray();
            //return null;
            //}

            #endregion
        }
    }
}
