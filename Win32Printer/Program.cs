using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Windows.UI.Xaml.Shapes;
using ImageMagick;

namespace Win32Printer
{
    class Program
    {
        static void Main(string[] args)
        {

            var settings = new MagickReadSettings
            {
                Density = new Density(300)
            };
            var images = new MagickImageCollection();
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("FileToPrint"))
            {
                // Image img = Image.FromFile(ApplicationData.Current.LocalSettings.Values["FileToPrint"] as string);
                //FileStream fs = new FileStream(@"C:\test.pdf", FileMode.Open);
                //Image img = Image.FromStream(fs);
                Image img = Image.from
                images.Read(@"C:\test.pdf", settings);

                PrintDocument doc = new PrintDocument();
                doc.PrintPage += new PrintPageEventHandler((sender, e)=>
                {
                    foreach(var image in images)
                    {
                        
                        image = ResizeImage(image, e.Graphics.VisibleClipBounds.Size);
                        e.Graphics.DrawImage(image, Point.Empty);
                        e.HasMorePages = false;
                    }
                    
                });
                doc.EndPrint += new PrintEventHandler((sender, e) =>
                {
                    resetEvent.Set();
                });
                doc.Print();
            }
            resetEvent.WaitOne();
        }

        public static Image ResizeImage(Image img, SizeF targetSize)
        {
            float scale = Math.Min(targetSize.Width / img.Width, targetSize.Height / img.Height);
            Size newSize = new Size((int)(img.Width * scale) - 1, (int)(img.Height * scale) - 1);
            return new Bitmap(img, newSize);
        }
    }
}
