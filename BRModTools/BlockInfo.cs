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
using OpenTK;
using OpenTK.Graphics.OpenGL;

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
            TextureBox.DataBindings.Add("Text", dv, "Texture", true, DataSourceUpdateMode.OnPropertyChanged);
            ClassBox.DataBindings.Add("Text", dv, "Class", true, DataSourceUpdateMode.OnPropertyChanged);
            CategoryBox.DataBindings.Add("Text", dv, "Category", true, DataSourceUpdateMode.OnPropertyChanged);
            RenderBox.DataBindings.Add("Text", dv, "Render", true, DataSourceUpdateMode.OnPropertyChanged);
            MassBox.DataBindings.Add("Text", dv, "Mass", true, DataSourceUpdateMode.OnPropertyChanged);
            HpBox.DataBindings.Add("Text", dv, "HP", true, DataSourceUpdateMode.OnPropertyChanged);
            DescLongBox.DataBindings.Add("Text", dv, "DescLong", true, DataSourceUpdateMode.OnPropertyChanged);
            DescShortBox.DataBindings.Add("Text", dv, "DescShort", true, DataSourceUpdateMode.OnPropertyChanged);
            TagsBox.DataBindings.Add("Text", dv, "Tags", true, DataSourceUpdateMode.OnPropertyChanged);
            FlagsBox.DataBindings.Add("Text", dv, "Flags", true, DataSourceUpdateMode.OnPropertyChanged);
            GeometryBox.DataBindings.Add("Text", dv, "Geometry", true, DataSourceUpdateMode.OnPropertyChanged);
            
            

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
            String location = modList.location;
            DirectoryInfo directory = new DirectoryInfo(location);
            String texturePath = directory.Parent.FullName + @"\Content\textures";
            texture = new Texture(texturePath, TextureBox.Text).Image;
            pictureBox1.Image = texture;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }

        /*private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.SkyBlue);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            glControl1.SwapBuffers();
        }*/
    }
}
