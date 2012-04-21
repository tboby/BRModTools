using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.DevIl;
using System.Drawing.Imaging;
using System.Security.AccessControl;

namespace BRModTools
{
    public partial class Main : Form
    {
        public static string[] CLASS_TYPES = { "Helm", "Script", "Usable", "Standard", "Prefab", "PrefabFragment", "Power", "Propulsion", "Launcher", "Extra", "Paint", "Overlay", "Damage", "Void", "Null" };
        public static string[] RENDER_TYPES = { "Standard", "Cylinder_Random", "CylinderYaw_Random", "CylinderYaw_Random2", "CylinderYaw_Random3", "Cylinder_Rarity", "Random", "Rarity", "Random3_Rarity", "Bulkhead", "Frame_Poxel", "Frame_Geometry", "Geometry", "Interior", "Plating", "None" };
        public static string[] CATEGORY_TYPES = { "S_Bulkheads", "S_Plating", "S_Windows", "S_Transit", "S_Doors", "S_Natural", "H_Interiors", "H_Lights", "H_Terminals", "H_Furniture", "H_LifeSupport", "H_Hydroponics", "E_Maneuvering", "E_Mains", "E_Interstellar", "E_Docking", "D_Armor", "D_Shields", "D_Sensors", "D_Emitters", "D_Security", "W_Mounts", "W_Weapons", "W_Equipment", "W_Ammunition", "U_Power", "U_Cables", "U_Radiators", "U_Industrial", "None" };
        ModList modList;
        DataTable data;
        DataTable dataTex;
        Boolean first = true;
        String activeMod;
        public Main()
        {
            
            InitializeComponent();
        }
        private void refreshSelect()
        {
            selectModBox.DataSource = modList.getModnames();
            activeMod = modList.getActiveMod();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            modList.cloneMod(selected);
            refreshSelect();
        }

        private void remove_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            if (selected == activeMod)
            {
                MessageBox.Show("You can't delete the Active Mod!", "Invalid mod");
            }
            else if (selected == "Vanilla")
            {
                MessageBox.Show("You can't rename the Vanilla files!", "Invalid mod");
            }
            else
            {
                modList.Remove(selected);
                refreshSelect();
            }
        }

        private void selectMod_Click(object sender, EventArgs e)
        {
            String exeLoc = Assembly.GetExecutingAssembly().Location;
            FileInfo exe = new FileInfo(exeLoc);
            DirectoryInfo dr = exe.Directory;
            FileInfo brExe = new FileInfo(dr.FullName+@"\blockaderunner.exe");
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (brExe.Exists) { folderBrowserDialog1.SelectedPath=dr.FullName+@"\mods"; }
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != "" && result == DialogResult.OK)
            {
                modFolderSelect(folderBrowserDialog1.SelectedPath);

                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                editMaterial.Enabled = true;
                create.Enabled = true;
                remove.Enabled = true;
                setActiveMod.Enabled = true;
                updateTable();
            }
        }

        void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            List<String> modNames = modList.getModnames();
            int index = 0;
            int actual = 0;
            foreach (String name in modNames)
            {
                if (activeMod == name)
                {
                    actual = index;
                }
                index++;
            }
            if (e.Index == actual)
            {
                e.Graphics.DrawString(selectModBox.Items[e.Index].ToString(), new Font("Arial", 9, FontStyle.Bold | FontStyle.Italic), Brushes.Black, e.Bounds);
            }
            else
            {
                e.Graphics.DrawString(selectModBox.Items[e.Index].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, e.Bounds);
            }
        }


        private void modFolderSelect(String location)
        {
            modList = new ModList(location);
            modList.init(); //init first so it can fail
            selectModBox.DrawMode = DrawMode.OwnerDrawFixed;
            selectModBox.DataSource = modList.getModnames();
            ArrayList textures = modList.getTextures((string)selectModBox.SelectedItem);
            //tableLayoutPanel1.RowCount = textures.Count;
            foreach (Texture texture in textures)
            {
                PictureBox box = new PictureBox();
                box.Image = texture.Image;
                box.SizeMode = PictureBoxSizeMode.AutoSize;
                box.Size = new Size(256, 256);
                TextBox text = new TextBox();
                text.Text=texture.Name;
                //Make textbox a selectable label
                text.ReadOnly = true;
                text.BackColor = System.Drawing.SystemColors.Control;
                text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            }
            dataGridView2.DataSource = modList.getTextureTable((string)selectModBox.SelectedItem);
            refreshSelect();
        }

        private void selectModBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTable();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            if (selected == activeMod)
            {
                MessageBox.Show("You can't rename the Active Mod!", "Invalid mod");
                
            }
            else if(selected == "Vanilla")
            {
                MessageBox.Show("You can't rename the Vanilla files!", "Invalid mod");
            }
            else{
                
                string input = Microsoft.VisualBasic.Interaction.InputBox("Rename Mod", "Enter the new mod name");
                if (!input.Equals(""))
                {
                    modList.rename(selected, input);
                }
                refreshSelect();
            }
        }

        private void updateTable()
        {
            String selected = (String)selectModBox.SelectedValue;
            data = modList.getTable(selected);
            dataTex = modList.getTextureTable(selected);
            dataGridView1.DataSource = data;
            dataGridView2.DataSource = dataTex;
            if (first)
            {
                dataGridView1.Columns[0].Frozen = true;
                dataGridView1.ReadOnly = true; //Read only tag!

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }

                first = false;
                dataGridView2.Columns.Remove("Texture");
                DataGridViewImageColumn imageColumn1 = new DataGridViewImageColumn();
                imageColumn1.DataPropertyName = "Texture";
                imageColumn1.HeaderText = "Texture";
                dataGridView2.Columns.Add(imageColumn1);
            }
        }

        
        

        private void button2_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            modList.Save(selected);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (((string)selectModBox.SelectedItem) != "Vanilla")
            {
                data.DefaultView.Sort = String.Empty;
                DataRow row = data.Rows[0];
                Object[] items = row.ItemArray;
                items[0] = "CHANGEME";
                data.Rows.Add(items);
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Selected = true;
            }
            else
            {
                vanillaReadOnlyError();
            }
        }

        void vanillaReadOnlyError()
        {
            MessageBox.Show("You can't modify the Vanilla files, make a clone!", "Invalid Mod");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (((string)selectModBox.SelectedItem) != "Vanilla")
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            else
            {
                vanillaReadOnlyError();
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            materialDialog(sender, e);
        }

        private void editMaterial_Click(object sender, EventArgs e)
        {
            if (((string)selectModBox.SelectedItem) != "Vanilla")
            {
                materialDialog(sender, e);
            }
            else
            {
                vanillaReadOnlyError();
            }
        }

        private void materialDialog(object sender, EventArgs e)
        {
            BindingManagerBase bm = dataGridView1.BindingContext[dataGridView1.DataSource, dataGridView1.DataMember];
            DataRow dr = ((DataRowView)bm.Current).Row;
                //DataRow dataRow = data.Rows[row];
                //String name = (String)dataRow.ItemArray[0];
                //TODO REPLACE THIS METHOD WITH REAL DATA BINDING
                String name = (string)dr.ItemArray[0];
                BlockInfo info = new BlockInfo(data, name,modList);
                info.ShowDialog();
                dr.EndEdit();
        }

        private void setActiveMod_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            if (selected != activeMod)
            {
                modList.Activate(selected);
                activeMod = selected;
            }
           
            refreshSelect();
        }
    }


    [Serializable()]
    public class ModList
    {
        ModComparator myComparer = new ModComparator();
        ArrayList mods = new ArrayList();
        //Textures textures;
        public String modLocation { get; set; }
        public String location { get; set; }
        Mod activeMod;
        public ModList(String location)
        {
            this.location = location;
            this.modLocation = location+@"\Mods";
            DirectoryInfo directory = new DirectoryInfo(modLocation);
            FileInfo[] InfoFile = directory.GetFiles("ModInfo.xml");
            
            DirectoryInfo[] modData = directory.GetDirectories();
            foreach (DirectoryInfo mod in modData)
            {
                mods.Add(new Mod(mod.Name,mod.FullName));
            }
            //textures = new Textures(location+@"\Content\textures");//Right place?
            if (InfoFile.Length == 0)
            {
                firstRun();
            }
            else
            {
                String activeMod = ModParser.readInfo(modLocation + @"\ModInfo.xml");
                this.activeMod = getMod(activeMod);
            }
        }
        public void firstRun()
        {
            //Copy permissions 
            System.Security.AccessControl.DirectorySecurity sec = System.IO.Directory.GetAccessControl(modLocation);
            FileSystemAccessRule accRule = new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow);
            sec.AddAccessRule(accRule);

            //File.Copy(location+@"\Content\Data\solids.xml",modLocation+@"\Vanilla\Content\Data\solids.xml");
            //File.Copy(location+@"\Content",modLocation+@"\Vanilla");
            CopyFolder.CopyAll(new DirectoryInfo(location+@"\Content"), new DirectoryInfo(modLocation + @"\Vanilla"));
            mods.Add(new Mod("Vanilla", modLocation + @"\Vanilla"));
            String output = ModWriter.writeInfo("Vanilla");

            FileStream fs = File.Create(modLocation+@"\ModInfo.xml");
            StreamWriter file = new StreamWriter(fs);
            file.WriteLine(output);
            file.Close();

            activeMod = getMod("Vanilla");
            
        }
        public void init()
        {
            try
            {
                foreach (Mod mod in mods)
                {
                    mod.loadBlocks();
                }
            }
            catch (FormatException)
            {
                throw new FormatException("ModList threw a format exception");
            }
            
        }
        public String getActiveMod()
        {
            return activeMod.Name;
        }
        public DataTable getTextureTable(String name)
        {
            return getMod(name).getTextureTable();
        }

        public ArrayList getTextures(String name)
        {
            return getMod(name).textures.getImages();
        }

        public void Save(String name)
        {
            Mod mod = getMod(name);
            mod.Save();
        }
        public List<String> getModnames()
        {
            List<String> items = new List<String>();
            foreach (Mod mod in mods)
            {
                items.Add(mod.Name);
            }
            return items;
        }
        public void cloneMod(String sourceString)
        {
            Mod source = getMod(sourceString);
            source.Save();
            while (mods.BinarySearch(new Mod(sourceString, null), myComparer) > -1)
            {
                sourceString += "-c";
            }
            CopyFolder.CopyAll(new DirectoryInfo(source.location), new DirectoryInfo(modLocation + "\\" + sourceString));
            Mod clone = new Mod(sourceString, modLocation + "\\" + sourceString);
            mods.Add(clone);
            clone.loadBlocks();
        }
        public void rename(String name, String newName)
        {
            Mod mod = getMod(name);
            while (mods.BinarySearch(new Mod(newName, null), myComparer) > -1)
            {
                newName += "-d";
            }
            mod.Name = newName;
            Directory.Move(modLocation+"\\"+name,modLocation+"\\"+newName);
            mod.location = modLocation + "\\" + newName;
        }
        public Mod getMod(String name)
        {
            mods.Sort(myComparer);
            Mod search = new Mod(name,null);
            
            return (Mod)mods[mods.BinarySearch(search, myComparer)];
        }
        public void Remove(String name)
        {
            Mod mod = getMod(name);
            mod.Delete();
            mods.RemoveAt(mods.BinarySearch(mod,myComparer));
        }
        public DataTable getTable(String name)
        {
            Mod mod = getMod(name);
            return mod.getTable();
        }

        internal void Activate(string selected)
        {
            Mod newMod = getMod(selected);
            Directory.Delete(location + @"\Content",true);
            CopyFolder.CopyAll(new DirectoryInfo(newMod.location), new DirectoryInfo(location+@"\Content"));
            String modInfo = ModWriter.writeInfo(newMod.Name);
            FileStream fs = File.Create(modLocation + @"\ModInfo.xml");
            StreamWriter file = new StreamWriter(fs);
            file.WriteLine(modInfo);
            file.Close();
            activeMod = newMod;
        }
    }

    public class ModComparator : IComparer
    {

        public int Compare(object x, object y)
        {
            Mod x2 = (Mod)x;
            Mod y2 = (Mod)y;
            String paramX = x2.Name;
            String paramY = y2.Name;
            return String.Compare(paramX, paramY);
        }
    }
    [Serializable()]
    public class Mod
    {
        String[] solidParams = { "Category", "Type", "DescLong", "DescShort", "Flags", "HP", "Mass", "RenderType", "Tags", "TexturePath", "ModelPath" };

        public String Name { get; set; }
        public String location { get; set; }
        private DataTable Blocks;
        public Textures textures { get; set; }
        public Mod(String name, String location)
        {
            this.Name=name;
            this.location = location;
        }
        public void loadBlocks()
        {
            String path = location+ @"\Data\solids.xml";
            StreamReader stream = new StreamReader(path);
            try
            {
                Blocks = ModParser.solidsTable(path,null);
                
            }
            catch (FormatException)
            {
                MessageBox.Show("The mod " + Name + " has an invalid .xml!");
                throw new FormatException("Mod " + Name + " threw a formatexception");
            }
            stream.Close();
            textures = new Textures(location + @"\Textures");
        }
        public DataTable getTable()
        {
            return Blocks;
        }
        public void Rename(String name)
        {
            Name = name;

            //TODO copy files
        }
        public DataTable getTextureTable()
        {
            return textures.getTable();
        }
        public Image getTexture(String name)
        {
            return textures.getTexture(name);
        }
        public void Save()
        {
            String xml = ModWriter.outputTable(Blocks);
            System.IO.Directory.CreateDirectory(location);
            FileStream fs = File.Create(location + @"\Data\solids.xml");
            StreamWriter file = new StreamWriter(fs);
            //StreamWriter file = new StreamWriter(location + @"\" + Name + @"\solids.xml");
            file.WriteLine(xml);
            file.Close();
         }
        public void Delete()
        {
            Directory.Delete(location,true);
        }
    }
    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// 
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>

    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
    [Serializable()]
    public class Textures
    {
        ArrayList images;
        DataTable dataTable;
        public Textures(String path)
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("Texture Name");
            dataTable.Columns.Add("File Name");
            dataTable.Columns.Add("Texture", typeof(Image));

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files;
            try { files = dir.GetFiles("*diffuse.dds"); }catch(FileNotFoundException e){throw e;}
            images = new ArrayList(); ;
            foreach (FileInfo file in files)
            {
                File.ReadAllBytes(file.FullName);
                Image bitmap = ImageTools.DDSDataToBMP(File.ReadAllBytes(file.FullName));
                //Image bitmap = DevIL.DevIL.LoadBitmap(file.FullName);
                String actualName = Path.GetFileNameWithoutExtension(file.Name);
                actualName=actualName.Replace("-diffuse", "");
                Texture texture = new Texture(bitmap, file.Name);
                images.Add(texture);
                // Texture2D.FromStream(gds.GraphicsDevice, 
                //images.Add(DevIL.DevIL.LoadBitmap(file.FullName));
                dataTable.Rows.Add(actualName,file.Name, bitmap);
            }
           
        }
        public DataTable getTable()
        {
            return dataTable;
        }
        public Image getTexture(String name)
        {
            foreach (Texture texture in images)
            {
                if (texture.Name.Equals(name))
                {
                    return texture.Image;
                }
            }
            return new Bitmap(256, 256); ;//TODO Add error image;
        }

        public ArrayList getImages()
        {

            return images;
        }
        //http://forums.create.msdn.com/forums/p/12527/66117.aspx
        /// <summary>
        /// Converts an in-memory image in DDS format to a System.Drawing.Bitmap
        /// object for easy display in Windows forms.
        /// </summary>
        /// <param name="DDSData">Byte array containing DDS image data</param>
        /// <returns>A Bitmap object that can be displayed</returns>
        
    }
    [Serializable()]
    public class Texture
    {
        public Image Image { get; set; }
        public String Name { get; set; }
        public String RealName { get; set; }
        public Texture(Image image, String name)
        {
            this.Image = image;
            this.Name = name;
        }
        public Texture(String path, String name)
        {
            String textureFile = path+"\\"+name+"-diffuse.dds";
            try
            {
                Image = ImageTools.DDSDataToBMP(File.ReadAllBytes(textureFile));
            }
            catch (Exception)
            {
                Image = null;
            }
            /*DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles("*diffuse.dds");
            foreach (FileInfo file in files)
            {
                String actualName = Path.GetFileNameWithoutExtension(file.Name);
                actualName = actualName.Replace("-diffuse", "");
                if (actualName == name)
                {
                    File.ReadAllBytes(file.FullName);
                    Image = ImageTools.DDSDataToBMP(File.ReadAllBytes(file.FullName));
                    name = file.Name;
                    RealName = actualName;
                }
            }*/
        }
    }
    public static class CopyFolder
    {
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
