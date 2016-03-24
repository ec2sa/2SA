using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScanManager
{
    public class PreviewGeneratorFactory
    {
        public static IPreviewGenerator Create(string mimeType,int? size,string tmpPath)
        {
            
            if (mimeType.Contains("image"))
                return new ImagePreviewGenerator(size ?? 150);
            if (mimeType.Contains("pdf"))
                return new PdfPreviewGenerator(size ?? 150,tmpPath);
            return null;
        }
    }
}
