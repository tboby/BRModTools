using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using BRModTools;
using System.Windows.Interop;

namespace BRModTools
{
    public partial class BlockInfo : Form
    {
       // bool loaded = false;//GLControl
        DataTable data;
        String name; 
        DataView dv;
        Image texture;
        ModList modList;
        public BlockInfo(DataTable dataTable, String name, ModList modList)
        {
            data = dataTable;
            this.modList = modList;
            
            this.name = name;
            InitializeComponent();
            loadData();
           
        }

        void loadData()
        {
            dv = data.DefaultView;
            dv.RowFilter = "Name ='"+name+"'";
            MaterialBox.DataBindings.Add("Text",dv,"Name",true,DataSourceUpdateMode.OnPropertyChanged);
            TextureBox.DataBindings.Add("Text", dv, "TexturePath", true, DataSourceUpdateMode.OnPropertyChanged);
            ClassBox.DataBindings.Add("Text", dv, "Type", true, DataSourceUpdateMode.OnPropertyChanged);
            CategoryBox.DataBindings.Add("Text", dv, "Category", true, DataSourceUpdateMode.OnPropertyChanged);
            RenderBox.DataBindings.Add("Text", dv, "RenderType", true, DataSourceUpdateMode.OnPropertyChanged);
            MassBox.DataBindings.Add("Text", dv, "Mass", true, DataSourceUpdateMode.OnPropertyChanged);
            HpBox.DataBindings.Add("Text", dv, "HP", true, DataSourceUpdateMode.OnPropertyChanged);
            DescLongBox.DataBindings.Add("Text", dv, "DescLong", true, DataSourceUpdateMode.OnPropertyChanged);
            DescShortBox.DataBindings.Add("Text", dv, "DescShort", true, DataSourceUpdateMode.OnPropertyChanged);
            TagsBox.DataBindings.Add("Text", dv, "Tags", true, DataSourceUpdateMode.OnPropertyChanged);
            FlagsBox.DataBindings.Add("Text", dv, "Flags", true, DataSourceUpdateMode.OnPropertyChanged);
            GeometryBox.DataBindings.Add("Text", dv, "ModelPath", true, DataSourceUpdateMode.OnPropertyChanged);
            
            

            //Setup combo boxes
            ClassBox.Items.AddRange(Main.CLASS_TYPES);
            CategoryBox.Items.AddRange(Main.CATEGORY_TYPES);
            RenderBox.Items.AddRange(Main.RENDER_TYPES);//TODO add width calculator and auto
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void BlockInfo_Load(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dv.RowFilter = "";//TODO This might be REALLY bad because override.
            MaterialBox.Select();
            this.ActiveControl.DataBindings["Text"].WriteValue();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            String location = modList.modLocation;
            DirectoryInfo directory = new DirectoryInfo(location);
            String texturePath = directory.Parent.FullName + @"\Content\textures";
            texture = new Texture(texturePath, TextureBox.Text).Image;
            pictureBox1.Image = texture;
            BlockRender render = new BlockRender((Bitmap)texture);
            this.userControl11 = new UserControl1(render.getFaces(0));
            this.elementHost1.Child = this.userControl11;
            
        }
    }

    public class BlockRender
    {
        public static int RENDER_STANDARD = 0;
        public static int RENDER_RANDOM = 1;
        public static int RENDER_CYLINDER_RANDOM = 2;
        public static int RENDER_CYLINDERYAW_RANDOM = 3;

        public Bitmap[] sectionBit = new Bitmap[4];
        public BitmapSource[] sections = new BitmapSource[4]; //TL,TR,BL,BR
        public BlockRender(Bitmap input)
        {
            sections[0] = ToBitmapSource(input.Clone(new Rectangle(0, 0, 128, 128), input.PixelFormat));
            sections[1] = ToBitmapSource(input.Clone(new Rectangle(128, 0, 128, 128), input.PixelFormat));
            sections[2] = ToBitmapSource(input.Clone(new Rectangle(0, 128, 128, 128), input.PixelFormat));
            sections[3] = ToBitmapSource(input.Clone(new Rectangle(128, 128, 128, 128), input.PixelFormat));
        }

        public BitmapSource[] getFaces(int mode)
        {
            BitmapSource[] faces = {sections[0],sections[0],sections[1],sections[2],sections[3],sections[3]};
            return faces;
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
        public BitmapSource ToBitmapSource(System.Drawing.Bitmap source)
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

            return bitSrc;
        }
    }
}
