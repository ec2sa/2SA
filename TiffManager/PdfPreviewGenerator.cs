using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace ScanManager
{
    public class PdfPreviewGenerator:IPreviewGenerator
    {
        private string tmpPath;

        public PdfPreviewGenerator(int size,string tmpPath)
        {
            PreviewSize = size;
            PreviewFormat = ImageFormat.Png;
            this.tmpPath = tmpPath;
        }

        private Image ConvertPdfToPng(string fileName){
            PDFConvert converter = new PDFConvert();
           
                converter.RenderingThreads = 1;
                converter.TextAlphaBit = -1;
                converter.GraphicsAlphaBit = -1;
                converter.OutputToMultipleFile = false;
                converter.FirstPageToConvert = 0;
                converter.LastPageToConvert = 0;
                converter.FitPage = false;
                converter.JPEGQuality = 50;
                converter.OutputFormat = "png16m";
                string outputFile = fileName + ".png";

                if (!converter.Convert(fileName, outputFile))
                {
                    return null;
                }
                if (!File.Exists(outputFile))
                    return null;
                Image res = Image.FromFile(outputFile);
                return res;

        }

        #region IPreviewGenerator Members

        private Image GetPreview(System.Drawing.Image scan)
        {
            int desiredHeight;
            int desiredWidht;
            if (scan.Height <= scan.Width)
            {
                desiredWidht = PreviewSize;
                desiredHeight = (int)(PreviewSize/1.41M);
            }
            else
            {
                desiredHeight = PreviewSize;
                desiredWidht = (int)(PreviewSize / 1.41M);
            }
          

            try
            {
                return scan.GetThumbnailImage(desiredWidht, desiredHeight, null, IntPtr.Zero);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error while generating scan preview: " + e.Message);
                return null;
            }
        }

        public Image GetPreview(byte[] scanBytes)
        {
            string tempInFileName=Path.Combine(tmpPath,Guid.NewGuid().ToString());
            File.WriteAllBytes(tempInFileName, scanBytes);
            Image tmpImg=ConvertPdfToPng(tempInFileName);
            if (tmpImg == null)
                return null;
            else
                return GetPreview(tmpImg);

            //using (MemoryStream ms = new MemoryStream(scanBytes))
            //{
            //    Image tmpImg = Image.FromStream(ms);
            //    return GetPreview(tmpImg);
            //}
        }

        public int PreviewSize { get; set; }

        public ImageFormat PreviewFormat { get; set; }

        #endregion
    }
}
