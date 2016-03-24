using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace ScanManager
{
    public interface IPreviewGenerator
    {
       // Image GetPreview(System.Drawing.Image scan);

        Image GetPreview(byte[] scan);
        ImageFormat PreviewFormat { get; set; }
        int PreviewSize { get; set; }
    }
}
