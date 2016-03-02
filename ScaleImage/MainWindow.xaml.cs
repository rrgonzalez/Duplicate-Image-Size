using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace ScaleImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string imageDir;
        private Bitmap bmpImage;
        private ScaleTransform scaleTransform;
        private const double scaleFactor = 2;

        public MainWindow()
        {
            InitializeComponent();
            scaleTransform = new ScaleTransform(scaleFactor, scaleFactor);
            RenderOptions.SetBitmapScalingMode(imageRender, BitmapScalingMode.HighQuality ) ;
        }

        private void buttonOpenImg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg"; 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                imageDir = dlg.FileName;             
                imageRender.Source = new BitmapImage(new Uri(imageDir));
                bmpImage = ConvertBitmapImageToBitmap(new BitmapImage(new Uri(imageDir)));
            }

        }

        private void buttonScaleImg_Click(object sender, RoutedEventArgs e)
        {
            ImageFormat format = ImageFormat.Jpeg;
            string newImageDir = imageDir.Substring(0, imageDir.Length - 3) + "- SIZE x2.jpg";
            Bitmap tempBmp = ScaleImage(bmpImage);
            tempBmp.Save(newImageDir, format);
        }

        public Bitmap ScaleImage(Bitmap image)
        {
            int newWidth = (int)(image.Width * scaleFactor);
            int newHeight = (int)(image.Height * scaleFactor);

            Bitmap result = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        private Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        //private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        //{
        //    using (var memory = new MemoryStream())
        //    {
        //        bitmap.Save(memory, ImageFormat.Png);
        //        memory.Position = 0;

        //        var bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = memory;
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.EndInit();

        //        return bitmapImage;
        //    }
        //}
    }
}
