using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OCRLibrary
{
    public interface IImageSplitter
    {
       List<Bitmap> GetPagesFromImage(Image img);
       
        List<Bitmap> GetPagesFromImage(string filename);

        List<Bitmap> GetPagesFromBinaryContent(byte[] imageContent);

       bool TryLoad(string fileName, out Image img);

       bool TryLoad(byte[] scanContent, out Image img);

    }

    

    public interface IOCRTool<T>
    {
        List<T> GetTextContentFromPages(List<Bitmap> pages);
    }
}
