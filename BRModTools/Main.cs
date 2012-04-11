using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRModTools
{
    public partial class Main : Form
    {
        string[] CLASS_TYPES = { "Helm", "Script", "Usable", "Standard", "Prefab", "PrefabFragment", "Power", "Propulsion", "Launcher", "Extra", "Paint", "Overlay", "Damage", "Void", "Null" };
        string[] RENDER_TYPES = { "Standard", "Cylinder_Random", "CylinderYaw_Random", "CylinderYaw_Random2", "CylinderYaw_Random3", "Cylinder_Rarity", "Random", "Rarity", "Random3_Rarity", "Bulkhead", "Frame_Poxel", "Frame_Geometry", "Geometry", "Interior", "Plating", "None" };
        string[] CATEGORY_TYPES = { "S_Bulkheads", "S_Plating", "S_Windows", "S_Transit", "S_Doors", "S_Natural", "H_Interiors", "H_Lights", "H_Terminals", "H_Furniture", "H_LifeSupport", "H_Hydroponics", "E_Maneuvering", "E_Mains", "E_Interstellar", "E_Docking", "D_Armor", "D_Shields", "D_Sensors", "D_Emitters", "D_Security", "W_Mounts", "W_Weapons", "W_Equipment", "W_Ammunition", "U_Power", "U_Cables", "U_Radiators", "U_Industrial", "None" };
        ModList modList;
        DataTable data;
        Boolean first = true;
        public Main()
        {
            InitializeComponent();
        }
        private void refreshSelect()
        {
            selectModBox.DataSource = modList.getModnames();
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
            modList.Remove(selected);
            refreshSelect();
        }

        private void selectMod_Click(object sender, EventArgs e)
        {
            String exeLoc = Assembly.GetExecutingAssembly().Location;
            FileInfo exe = new FileInfo(exeLoc);
            DirectoryInfo dr = exe.Directory;
            FileInfo brExe = new FileInfo(dr.FullName+@"\blockaderunner.exe");
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (brExe.Exists) { folderBrowserDialog1.SelectedPath=dr.FullName+@"\mods"; }
            folderBrowserDialog1.ShowDialog();
            try
            {
                if (folderBrowserDialog1.SelectedPath != "")
                {
                    modFolderSelect(folderBrowserDialog1.SelectedPath);
                }
                button1.Enabled = true;
                button2.Enabled = true;
                create.Enabled = true;
                remove.Enabled = true;
                updateTable();
             
            }
            catch (Exception)
            {
            }

        }

        private void modFolderSelect(String location)
        {
            modList = new ModList(location);
            modList.init(); //init first so it can fail
            selectModBox.DataSource = modList.getModnames();
        }

        private void selectModBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTable();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Rename Mod", "Enter the new mod name");
            if (!input.Equals(""))
            {
                String selected = (String)selectModBox.SelectedValue;
                modList.rename(selected,input);
            }
            refreshSelect();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            /*if (e.ColumnIndex == myColumn.Index)
            {
                object eFormattedValue = e.FormattedValue;
                if (!myColumn.Items.Contains(eFormattedValue))
                {
                    myColumn.Items.Add(eFormattedValue);
                }
            }*/
        }
        private void updateTable()
        {
            String selected = (String)selectModBox.SelectedValue;
            data = modList.getTable(selected);
            dataGridView1.DataSource = data;
            if (first)
            {
                dataGridView1.Columns.Remove("Class");
                dataGridView1.Columns.Remove("Category");
                dataGridView1.Columns.Remove("Render");
                DataGridViewComboBoxColumn comboboxColumn1;
                DataGridViewComboBoxColumn comboboxColumn2;
                DataGridViewComboBoxColumn comboboxColumn3;
                comboboxColumn1 = CreateComboBoxColumn(1);
                comboboxColumn2 = CreateComboBoxColumn(2);
                comboboxColumn3 = CreateComboBoxColumn(3);
                dataGridView1.Columns.Insert(1, comboboxColumn1);
                dataGridView1.Columns.Insert(2, comboboxColumn2);
                dataGridView1.Columns.Insert(8, comboboxColumn3);
                dataGridView1.Columns[0].Frozen = true;
                first = false;
            }
        }

        private DataGridViewComboBoxColumn CreateComboBoxColumn(int columnNum) //1 = Cat, 2 Class, 3 Render
        {
            DataGridViewComboBoxColumn column =
                new DataGridViewComboBoxColumn();
            {
                switch (columnNum)
                {
                    case 1:
                        column.DataPropertyName = "Category";
                        column.HeaderText = "Category";
                        column.MaxDropDownItems = 3;
                        column.Items.AddRange(CATEGORY_TYPES);
                        column.ValueMember = "Category";
                        column.DropDownWidth = DropDownWidth(CATEGORY_TYPES);

                        break;
                    case 2:
                        column.DataPropertyName = "Class";
                        column.HeaderText = "Class";
                        column.MaxDropDownItems = 15;
                        column.Items.AddRange(CLASS_TYPES);
                        column.ValueMember = "Class";
                        column.DropDownWidth = DropDownWidth(CLASS_TYPES);

                        break;
                    case 3:
                        column.DataPropertyName = "Render";
                        column.HeaderText = "Render";
                        column.MaxDropDownItems = 29;
                        column.Items.AddRange(RENDER_TYPES);
                        column.ValueMember = "Render";
                        column.DropDownWidth = DropDownWidth(RENDER_TYPES);

                        break;
                }
            }
            column.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            return column;
        }
        //http://stackoverflow.com/questions/4842160/auto-width-of-comboboxs-content
        int DropDownWidth(String[] items)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            modList.Save(selected);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataRow row=data.Rows[0];
            Object[] items = row.ItemArray;
            items[0] = "CHANGEME";
            data.Rows.Add(items);
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
            dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Selected = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }
    }


    [Serializable()]
    public class ModList
    {
        ModComparator myComparer = new ModComparator();
        ArrayList mods = new ArrayList();
        public ModList(String location)
        {
            DirectoryInfo directory = new DirectoryInfo(location);
            DirectoryInfo[] modData = directory.GetDirectories();
            foreach (DirectoryInfo mod in modData)
            {
                mods.Add(new Mod(mod.Name,mod.Parent.FullName));
            }
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
            mods.Sort(myComparer);
            Mod source = getMod(sourceString);
            Mod clone = ObjectCopier.Clone<Mod>(source);
            while(mods.BinarySearch(new Mod(sourceString,null),myComparer)>-1)
            {
                sourceString += "-c";
            }
            clone.Rename(sourceString);
            mods.Add(clone);
            clone.Save();
        }
        public void rename(String name, String newName)
        {
            Mod mod = getMod(name);
            while (mods.BinarySearch(new Mod(newName, null), myComparer) > -1)
            {
                name += "-d";
            }
            mod.Name = newName;
        }
        public Mod getMod(String name)
        {
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
        String[] solidParams = { "Category", "Class", "DescLong", "DescShort", "Flags", "Geometry", "HP", "Mass", "Render", "Tags", "Texture" };

        public String Name { get; set; }
        private String location;
        private DataTable Blocks;
        public Mod(String name, String location)
        {
            this.Name=name;
            this.location = location;
        }
        public void loadBlocks()
        {
            StreamReader stream = new StreamReader(location + @"\" + Name+@"\solids.xml");
            try
            {
                Solids solids = ModParser.inputSolids(stream);
                solids.TableCreater();
                Blocks = solids.BlockTable;
            }
            catch (FormatException)
            {
                MessageBox.Show("The mod " + Name + " has an invalid .xml!");
                throw new FormatException("Mod " + Name + " threw a formatexception");
            }
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
        public Solids exportTable()
        {
            Solids output = new Solids();
            foreach (DataRow row in Blocks.Rows)
            {
                if (!row[0].ToString().Equals(""))
                {
                    Block block = output.addBlock((String)row["Name"], solidParams);

                    foreach (DataColumn column in Blocks.Columns)
                    {
                        block.setParam(column.Caption, (String)row[column.Caption]);
                    }
                }


            }
            return output;
        }
        public void Save()
        {
            Solids export = exportTable();
            String xml = ModWriter.outputSolids(export);
            System.IO.Directory.CreateDirectory(location + "\\" + Name);
            FileStream fs = File.Create(location + @"\" + Name + @"\solids.xml");
            StreamWriter file = new StreamWriter(fs);
            //StreamWriter file = new StreamWriter(location + @"\" + Name + @"\solids.xml");
            file.WriteLine(xml);
            file.Close();
         }
        public void Delete()
        {
            File.Delete(location + @"\" + Name + @"\solids.xml");
            Directory.Delete(location + "\\" + Name);
        }
    }
}
