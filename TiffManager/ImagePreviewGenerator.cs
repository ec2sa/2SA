using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScanManager
{
    public class ImagePreviewGenerator : IPreviewGenerator
    {
        public int PreviewSize { get; set; }
       
        public ImageFormat PreviewFormat{get;set;}

        public ImagePreviewGenerator(int size)
        {
            PreviewSize = size;
            PreviewFormat = ImageFormat.Png;
        }

        public Image GetPreview(byte[] scanBytes)
        {
            using (MemoryStream ms = new MemoryStream(scanBytes))
            {
                Image tmpImg = Image.FromStream(ms);
                return GetPreview(tmpImg);
            }
        }

        private Image GetPreview(Image scan)
        {
            //decimal highestDimension=scan.Height;
            //decimal ratio;
            int desiredHeight;
            int desiredWidht;
            if (scan.Height <= scan.Width)
            {
                desiredWidht = PreviewSize;
                desiredHeight = (int)(PreviewSize / 1.41M);
            }
            else
            {
                desiredHeight = PreviewSize;
                desiredWidht = (int)(PreviewSize / 1.41M);
            }
                //highestDimension=scan.Width;
                //ratio=PreviewSize/highestDimension;

            //int desiredHeight= (int)(scan.Height*ratio);
            //int desiredWidht=(int)(scan.Width*ratio);
            
            try
            {
                return scan.GetThumbnailImage(desiredWidht, desiredHeight, null, IntPtr.Zero);
            }catch(Exception e){
                System.Diagnostics.Debug.WriteLine("Error while generating scan preview: " + e.Message);
                return null;
            }
            
        }
    }
}
