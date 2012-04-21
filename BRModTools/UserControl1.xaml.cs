using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BRModTools
{
    
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public ImageSource[] faces{get;set;}
        public ImageSource Top { get; set; }
        public BitmapImage Full;
        public ImageSource test { get; set; }
        public BitmapSource Thumb { get; set; }
        public ImageSource topTexture { get; set; }
        //public ImageBrush bottomBrush = new ImageBrush(new BitmapImage(new Uri(@"C:\\Users\\Thomas\\Documents\\Visual Studio 11\\Projects\\WpfTest2\\WpfTest2\\bark.jpg")));
        public UserControl1(BitmapSource[] faces)
        {
            InitializeComponent();
            this.DataContext = this;
            Init(faces);
        }
        public void Init(BitmapSource[] faces)
        {
            this.faces = faces;
            
        }
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                // return bitmap; <-- leads to problems, stream is closed/closing ...
                return new Bitmap(bitmap);
            }
        }
        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                return bi;
            }
        }

       
    }

    public class BrushConverter : IValueConverter
    {
        


        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage _i = (BitmapImage)value;
            ImageBrush _b = new ImageBrush();
            if (_i != null)
            {
                _b.ImageSource = _i;
            }
            return _b;

        }

        public object ConvertI(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            InteropBitmap _i = (InteropBitmap)value;
            ImageBrush _b = new ImageBrush();
            if (_i != null)
            {
                _b.ImageSource = _i;
            }
            return _b;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BrushConverterI : IValueConverter
    {



        #region IValueConverter Members


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            InteropBitmap _i = (InteropBitmap)value;
            ImageBrush _b = new ImageBrush();
            if (_i != null)
            {
                _b.ImageSource = _i;
            }
            return _b;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class utils
    {
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
            }

            return bitSrc;
        }
    }

}
