using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.DevIl;

namespace BRModTools
{
    public class ImageTools
    {
        public static void loadImage()
        {
            //Image image = DevIL.DevIL.LoadBitmap(@"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\teste.dds");
            //Create image
            int img_name;
            Il.ilGenImages(1, out img_name);
            Il.ilBindImage(img_name);
            //Load iamge
            byte[] bytes = File.ReadAllBytes(@"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\test.png");
            Il.ilLoadL(Il.IL_PNG, bytes, bytes.Length);
            // Il.ilLoad(Il.IL_PNG, @"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\test.png");
            //Il.ilSetInteger(Il.IL_DXTC_FORMAT,Il.IL_DXT3);
            //Ilu.iluBuildMipmaps();
            //Ilut.ilutGLBuildMipmaps();
            //Il.ilSave(Il.IL_DDS, @"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\test.dds");
            //Il.ilSaveImage(@"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\test.dds");
           // Il.ilSaveDds(@"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\teste.dds");
            byte[] newbytes=null;
            GCHandle pinnedArray = GCHandle.Alloc(newbytes,GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            //Il.ilSaveF(Il.IL_DDS, pointer);
            Il.ilSaveL(Il.IL_DDS, newbytes, 0);

            File.WriteAllBytes(@"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\test.dds", newbytes);
            //Il.ilSaveImage(@"C:\Users\Thomas\Documents\Visual Studio 11\Projects\BRModTools\BRModTools\bin\Debug\test.dds");
        }
        public static Bitmap DDSDataToBMP(byte[] DDSData)
        {
            // Create a DevIL image "name" (which is actually a number)
            int img_name;
            Il.ilGenImages(1, out img_name);
            Il.ilBindImage(img_name);

            // Load the DDS file into the bound DevIL image
            Il.ilLoadL(Il.IL_DDS, DDSData, DDSData.Length);

            // Set a few size variables that will simplify later code

            int ImgWidth = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            int ImgHeight = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            Rectangle rect = new Rectangle(0, 0, ImgWidth, ImgHeight);

            // Convert the DevIL image to a pixel byte array to copy into Bitmap
            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE);

            // Create a Bitmap to copy the image into, and prepare it to get data
            Bitmap bmp = new Bitmap(ImgWidth, ImgHeight);
            BitmapData bmd =
              bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // Copy the pixel byte array from the DevIL image to the Bitmap
            Il.ilCopyPixels(0, 0, 0,
              Il.ilGetInteger(Il.IL_IMAGE_WIDTH),
              Il.ilGetInteger(Il.IL_IMAGE_HEIGHT),
              1, Il.IL_BGRA, Il.IL_UNSIGNED_BYTE,
              bmd.Scan0);

            // Clean up and return Bitmap
            Il.ilDeleteImages(1, ref img_name);
            bmp.UnlockBits(bmd);
            return bmp;
        }
    }
}
