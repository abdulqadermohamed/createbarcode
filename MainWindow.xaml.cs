using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using ZXing;
using ZXing.Common;
using Image = System.Windows.Controls.Image;

namespace createbarcode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bitmap bt { get; set; }   
        public string pathoffoldertoprintimage { get; set; }
        public void deletefolderafterrighthand()
        {
            try
            {


                var filePath = pathoffoldertoprintimage;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                }
            }
            catch
            {
            }


        }
        public MainWindow()
        {
            InitializeComponent();
        }         
        public static Bitmap CreateBarCode(string contents, int width, int height)
        {

            EncodingOptions options = null;
            BarcodeWriter writer = null;
            options = new EncodingOptions
            {
                Width = width,
                Height = height
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;
            writer.Options = options;
            Bitmap bitmap = writer.Write(contents);
            return bitmap;
        }
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            if (txt_barcode.Text!="")
            {


                if (string.IsNullOrEmpty(txt_barcode.Text.Trim()))
                    return;
                BitmapImage bitmap = BitmapToImageSource(CreateBarCode(txt_barcode.Text.Trim(), 225, 86));
                img_barcode.Source = bitmap;
                bt = CreateBarCode(txt_barcode.Text.Trim(), 100, 100);



                string foldersavebase = @"E:\Applications\Archiving\createbarcode\barcodefolder";
                string extenison = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                string fullpathofimage = String.Format("{0}\\{1}", foldersavebase, extenison);
                Directory.CreateDirectory(fullpathofimage);

               

                string filesaving = fullpathofimage+@"\2020.Png";
                pathoffoldertoprintimage =filesaving ;
                bt.Save(filesaving, ImageFormat.Png);
            }
            else
            {
                MessageBox.Show("insert number please");
            }
           
        }      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filesaving =pathoffoldertoprintimage;
            if (File.Exists(filesaving))
            {


                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(filesaving);
                bi.EndInit();

                var vis = new DrawingVisual();
                using (var dc = vis.RenderOpen())
                {
                    dc.DrawImage(bi, new Rect { Width = bi.Width, Height = bi.Height });
                }

                var pdialog = new PrintDialog();
                if (pdialog.ShowDialog() == true)
                {
                    pdialog.PrintVisual(vis, "My Image");
                }
                deletefolderafterrighthand();
            }
            else
            {
                MessageBox.Show("not found barcode to print");
            }
        }
    }
}

