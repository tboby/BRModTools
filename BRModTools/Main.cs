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
using System.Text.RegularExpressions;

namespace BRModTools
{
    public partial class Main : Form
    {
        public static string[] CLASS_TYPES = { "Helm", "Script", "Usable", "Standard", "Prefab", "PrefabFragment", "Power", "Propulsion", "Launcher", "Extra", "Paint", "Overlay", "Damage", "Void", "Null" };
        public static string[] RENDER_TYPES = { "Standard", "Cylinder_Random", "CylinderYaw_Random", "CylinderYaw_Random2", "CylinderYaw_Random3", "Cylinder_Rarity", "Random", "Rarity", "Random3_Rarity", "Bulkhead", "Frame_Poxel", "Frame_Geometry", "Geometry", "Interior", "Plating", "None" };
        public static string[] CATEGORY_TYPES = { "S_Bulkheads", "S_Plating", "S_Windows", "S_Transit", "S_Doors", "S_Natural", "H_Interiors", "H_Lights", "H_Terminals", "H_Furniture", "H_LifeSupport", "H_Hydroponics", "E_Maneuvering", "E_Mains", "E_Interstellar", "E_Docking", "D_Armor", "D_Shields", "D_Sensors", "D_Emitters", "D_Security", "W_Mounts", "W_Weapons", "W_Equipment", "W_Ammunition", "U_Power", "U_Cables", "U_Radiators", "U_Industrial", "None" };
        public bool selectBoxReady = false;
        ModList modList;
        AddonList addonList;
        DataTable data;
        DataTable dataTex;
        DataTable dataPref;
        Boolean first = true;
        String activeMod;
        ArrayList activeAddons;
        List<String> loadedTextures;
        bool isModActive;
        String gameLocation;
        public Main()
        {
            isModActive = true;
            InitializeComponent();
        }
        private void refreshSelect()
        {
            selectModBox.DataSource = modList.getModnames();
            selectAddonBox.DataSource = addonList.getAddonNames();
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
            if (brExe.Exists) { folderBrowserDialog1.SelectedPath = dr.FullName; gameLocation = dr.FullName; }
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

        void DrawAddonItemHandler(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            List<String> addonNames = addonList.getAddonNames();
            int index = 0;
            ArrayList actual = new ArrayList();
            foreach (String name in addonNames)
            {
                if(isActiveAddon(name))
                {
                    actual.Add(index);
                }
                index++;
            }
            if (actual.BinarySearch(e.Index)>-1)
            {
                e.Graphics.DrawString(selectAddonBox.Items[e.Index].ToString(), new Font("Arial", 9, FontStyle.Bold | FontStyle.Italic), Brushes.Black, e.Bounds);
            }
            else
            {
                e.Graphics.DrawString(selectAddonBox.Items[e.Index].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, e.Bounds);
            }
        }

        internal bool isActiveAddon(string name)
        {
            if (addonList.getActiveAddonNames() != null)
            {
                foreach (String addon in addonList.getActiveAddonNames())
                {
                    if (addon == name)
                        return true;
                }
            }
            /*if (addonList.getActiveAddons() != null)
            {
                foreach (String addon in addonList.getActiveAddons())
                {
                    if (addon == name)
                        return true;
                }
            }*/
            return false;
        }


        private void modFolderSelect(String location)
        {
            ModInfo modInfo;
            DirectoryInfo directory = new DirectoryInfo(location+@"\Mods");
            FileInfo[] InfoFile = directory.GetFiles("ModInfo.xml");
            if (InfoFile.Length == 0)
            {
                modInfo = new ModInfo();
            }
            else
            {
                modInfo = ModParser.readInfo(location+@"\Mods\ModInfo.xml");
            }

            modList = new ModList(location,modInfo);
            modList.init(); //init first so it can fail
            addonList = new AddonList(location, modInfo);
            addonList.init();
            selectAddonBox.DrawMode = DrawMode.OwnerDrawFixed;
            selectAddonBox.DataSource = addonList.getAddonNames();
            
            selectModBox.DataSource = modList.getModnames();
            selectModBox.DrawMode = DrawMode.OwnerDrawFixed;
            ArrayList modTextures = modList.getTextures((string)selectModBox.SelectedItem);

            if (!addonList.isEmpty)
            {
                if (addonList.getAddon((string)selectAddonBox.SelectedItem).hasTextures)
                {
                    ArrayList addonTextures = addonList.getTextures((string)selectAddonBox.SelectedItem);
                }
            }
            dataGridView2.DataSource = modList.getTextureTable((string)selectModBox.SelectedItem);
            dataGridView3.DataSource = modList.getPrefabTable((string)selectModBox.SelectedItem);
            refreshSelect();
        }

        private void selectModBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectBoxReady)
            {
                updateTable();
            }
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
            
            if (tabControl2.SelectedIndex == 0)
            {
                String selected = (String)selectModBox.SelectedValue;
                if (selected != null)
                {
                    data = modList.getTable(selected);
                    dataTex = modList.getTextureTable(selected);
                    dataPref = modList.getPrefabTable(selected);
                    dataGridView1.DataSource = data;
                    dataGridView2.DataSource = dataTex;
                    dataGridView3.DataSource = dataPref;
                }
                else
                {
                    dataGridView3.DataSource = dataPref = null;
                    dataGridView2.DataSource = data = null;
                    dataGridView1.DataSource = dataTex = null;
                }
            }
            else
            {
                String selected = (String)selectAddonBox.SelectedValue;
                if (selected != null)
                {
                    data = addonList.getTable(selected);
                    dataTex = addonList.getTextureTable(selected);
                    dataPref = addonList.getPrefabTable(selected);
                    dataGridView1.DataSource = data;
                    dataGridView2.DataSource = dataTex;
                    dataGridView3.DataSource = dataPref;
                }
                else
                {
                    dataGridView3.DataSource = dataPref = null;
                    dataGridView2.DataSource = data = null;
                    dataGridView1.DataSource = dataTex = null;
                }
            }
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
            selectBoxReady = true;
        }

        
        

        private void button2_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            modList.Save(selected);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 0)
            {
                if (((string)selectModBox.SelectedItem) == "Vanilla")
                {
                    vanillaReadOnlyError();
                }
                else if ((String)selectModBox.SelectedValue == activeMod)
                {
                    activeReadOnlyError();
                }
                else
                {
                    data.DefaultView.Sort = String.Empty;
                    DataRow row = data.Rows[0];
                    Object[] items = row.ItemArray;
                    items[0] = "CHANGEME";
                    data.Rows.Add(items);
                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                    dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Selected = true;
                }
            }
            else
            {
                if (dataGridView1.DataSource != null)
                {
                    data.DefaultView.Sort = String.Empty;
                    DataRow row = modList.getTable("Vanilla").Rows[0];
                    Object[] items = row.ItemArray;
                    items[0] = "CHANGEME";
                    data.Rows.Add(items);
                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                    dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Selected = true;
                }
                else
                {
                    data=addonList.getNewTable((string)selectAddonBox.SelectedItem);
                    updateTable();
                    dataGridView1.Columns[0].Frozen = true;
                    dataGridView1.ReadOnly = true; //Read only tag!

                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
                
            }
        }

        void vanillaReadOnlyError()
        {
            MessageBox.Show("You can't modify the Vanilla files, make a clone!", "Invalid Mod");
        }
        void activeReadOnlyError()
        {
            MessageBox.Show("You can't modify an active mod or addon for now", "Invalid Operation");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 0)
            {
                if (((string)selectModBox.SelectedItem) == "Vanilla")
                {
                    vanillaReadOnlyError();
                }
                else if ((String)selectModBox.SelectedValue == activeMod)
                {
                    activeReadOnlyError();
                }
                else
                {
                    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                }
            }
            else
            {
                if (dataGridView1.RowCount > 0) { dataGridView1.Rows.Remove(dataGridView1.CurrentRow); }
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
            if (tabControl2.SelectedIndex == 0)
            {
                if (((string)selectModBox.SelectedItem) == "Vanilla")
                {
                    vanillaReadOnlyError();
                }
                else if ((String)selectModBox.SelectedValue == activeMod)
                {
                    activeReadOnlyError();
                }
                else
                {
                    materialDialog(sender, e);
                }
            }
            else
            {
                materialDialog(sender, e);
            }
        }

        private void materialDialog(object sender, EventArgs e)
        {
            BindingManagerBase bm = dataGridView1.BindingContext[dataGridView1.DataSource, dataGridView1.DataMember];
            try
            {
                DataRow dr = ((DataRowView)bm.Current).Row;
                //DataRow dataRow = data.Rows[row];
                //String name = (String)dataRow.ItemArray[0];
                //TODO REPLACE THIS METHOD WITH REAL DATA BINDING
                String name = (string)dr.ItemArray[0];
                BlockInfo info = new BlockInfo((DataTable)dataGridView1.DataSource, name, modList, modList.getMod((string)selectModBox.SelectedItem));
                info.ShowDialog();
                dr.EndEdit();
            }
            catch (IndexOutOfRangeException)
            {

            }
        }

        private void setActiveMod_Click(object sender, EventArgs e)
        {
            String selected = (String)selectModBox.SelectedValue;
            if (selected != activeMod)
            {
                modList.Activate(selected);
                activeMod = selected;
                addonList.setActiveAddons(null);
                //Add all the texture names to
                /*ArrayList images = modList.getTextures(selected);
                loadedTextures = new List<String>();
                foreach (Texture texture in images)
                {
                    loadedTextures.Add(texture.Name);
                }*/

            }
            writeModInfo();
            refreshSelect();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectBoxReady) { updateTable(); }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool matEnabled;
            if (tabControl2.SelectedIndex == 0)//If first tab selected
            {
                dataGridView1.DataSource =data= modList.getTable((string)selectModBox.SelectedItem);
                dataGridView2.DataSource =dataTex= modList.getTextureTable((string)selectModBox.SelectedItem);
                dataGridView3.DataSource = dataPref = modList.getPrefabTable((string)selectModBox.SelectedItem);
                //Display the selected items
                matEnabled = true;
            }
            else
            {
                if (addonList.isEmpty)
                {
                    dataGridView1.DataSource =data= null;
                    dataGridView2.DataSource =dataTex= null;
                    dataGridView3.DataSource = dataPref = null;
                    //Nothing to display, so disable buttons and stuff
                    matEnabled = false;
                }
                else
                {
                    dataGridView1.DataSource =data= addonList.getTable((string)selectAddonBox.SelectedItem);
                    dataGridView2.DataSource =dataTex= addonList.getTextureTable((string)selectAddonBox.SelectedItem);
                    dataGridView3.DataSource = dataPref = addonList.getPrefabTable((string)selectAddonBox.SelectedItem);
                    //Display the selected item
                    matEnabled = true;
                }
            }
            button3.Enabled = matEnabled;
            button4.Enabled = matEnabled;
            editMaterial.Enabled = matEnabled;

        }

        private void addAddon_Click(object sender, EventArgs e)
        {
             string input = Microsoft.VisualBasic.Interaction.InputBox("Name Addon", "Enter the new addon name");
            if (!input.Equals(""))
            {
                addonList.addAddon(input);
           }
            refreshSelect();
            button3.Enabled = true;
            button4.Enabled=true;
            editMaterial.Enabled=true;
        }

        private void removeAddon_Click(object sender, EventArgs e)
        {
            selectBoxReady = false;
            String selected = (String)selectAddonBox.SelectedValue;
            if (isActiveAddon(selected))
            {
                activeReadOnlyError();
            }
            else
            {
                addonList.Remove(selected);
                refreshSelect();
            }
            selectBoxReady = true;
            
        }

        private void saveAddon_Click(object sender, EventArgs e)
        {
            String selected = (String)selectAddonBox.SelectedValue;
            addonList.Save(selected);
            refreshSelect();
        }

        private void renameAddon_Click(object sender, EventArgs e)
        {
            String selected = (String)selectAddonBox.SelectedValue;
            if (isActiveAddon(selected))
            {
                activeReadOnlyError();
            }
            else
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Rename Addon", "Enter the new addon name");
                if (!input.Equals(""))
                {
                    selectBoxReady = false;
                    addonList.rename((string)selectAddonBox.SelectedItem, input);
                }
                refreshSelect();
                selectBoxReady = true;
            }
        }

        private void activeDeactiveAddon_Click(object sender, EventArgs e)
        {
            String selected = (String)selectAddonBox.SelectedValue;
            if (!isActiveAddon(selected))
            {
                addonList.Activate(selected);
            }
            else
            {
                addonList.SetUnactiveAddons(selected);
                modList.Activate(modList.activeMod.Name);
                ArrayList activeAddons = addonList.getActiveAddonNames();
                addonList.setActiveAddons(new ArrayList());
                foreach (String addon in activeAddons)
                {
                    addonList.Activate(addon);
                }
            }
            writeModInfo();
            refreshSelect();
        }
        private void writeModInfo()
        {
            String modInfo = ModWriter.writeInfo(modList.activeMod.Name, addonList.getActiveAddonNames());
            FileStream fs = File.Create(modList.modLocation + @"\ModInfo.xml");
            StreamWriter file = new StreamWriter(fs);
            file.WriteLine(modInfo);
            file.Close();
        }
    }

    public class AddonList
    {
        AddonComparator myComparer = new AddonComparator();
        ArrayList addons = new ArrayList();
        public String addonLocation { get; set; }
        public String location { get; set; }
        private ArrayList activeAddons;
        public ArrayList activeAddonNames { get; set; }
        public ModInfo info { get; set; }
        public bool isEmpty=true;
        public AddonList(String location,ModInfo info)
        {
            this.info = info;
            this.location = location;
            this.addonLocation = location + @"\Addons";

            DirectoryInfo directory = new DirectoryInfo(addonLocation);

            DirectoryInfo[] addonData = directory.GetDirectories();
            
            

            if (addonData.Length>0)
            {
                isEmpty = false;
                foreach (DirectoryInfo addon in addonData)
                {
                    addons.Add(new Addon(addon.Name, addon.FullName));
                }
            }
            else
            {
                isEmpty = true;
            }
            ArrayList actualActiveAddons = new ArrayList();
            foreach (String addonName in info.activeAddons)
            {
                Addon addon = getAddon(addonName);
                if (addon != null)
                {
                    actualActiveAddons.Add(addon);
                }
            }
            setActiveAddons(actualActiveAddons);
        }
        public DataTable getPrefabTable(String name)
        {
            return getAddon(name).getPrefabTable();
        }
        public void setActiveAddons(ArrayList addons)
        {
            this.activeAddons = addons;
            ArrayList names = new ArrayList();
            if (activeAddons != null)
            {
                foreach (Addon addon in activeAddons)
                {
                    names.Add(addon.getName());
                }
                activeAddonNames = names; ;
            }
            else
            {
                activeAddonNames = new ArrayList();
            }
        }
        public ArrayList getActiveAddons()
        {
            return activeAddons;
        }
        public ArrayList getActiveAddonNames()
        {
            return activeAddonNames;
        }
        /*public ArrayList getActiveAddons()
        {
            ArrayList names = new ArrayList();
            if (activeAddons != null)
            {
                foreach (Addon addon in activeAddons)
                {
                    names.Add(addon.getName());
                }
                return names;
            }
            return null;
        }*/
        public void addAddon(String name)
        {
            
            while (addons.BinarySearch(new Addon(name, null), myComparer) > -1)
            {
                name += "-n";
            }
            Addon mod = new Addon(name,location+@"\Addons\"+name);
            if (isEmpty)
            { addons = new ArrayList(); isEmpty = false; }
            addons.Add(mod);
        }
        public void init()
        {
            try
            {
                foreach (Addon addon in addons)
                {
                    addon.loadBlocks();
                    addon.loadTextures();
                    addon.loadPrefabs();
                }
            }
            catch (FormatException)
            {
                throw new FormatException("ModList threw a format exception");
            }

        }
        public Addon getAddon(String name)
        {
            addons.Sort(myComparer);
            Addon search = new Addon(name, null);
            int index = addons.BinarySearch(search, myComparer);
            if (index > -1)
            {
                return (Addon)addons[addons.BinarySearch(search, myComparer)];
            }
            else
            {
                return null;
            }
        }
        public DataTable getTextureTable(String name)
        {
            return getAddon(name).getTextureTable();
        }
        public ArrayList getTextures(String name)
        {
            return getAddon(name).getTextures().getImages();
        }
        public void Save(String name)
        {
            getAddon(name).Save();
            
        }
        public List<String> getAddonNames()
        {
            List<String> items = new List<String>();
            foreach (Addon mod in addons)
            {
                items.Add(mod.getName());
            }
            return items;
        }
        public void cloneAddon(String sourceString)
        {
            Addon source = getAddon(sourceString);
            source.Save();
            addons.Sort();
            while (addons.BinarySearch(new Addon(sourceString, null), myComparer) > -1)
            {
                sourceString += "-c";
            }
            CopyFolder.CopyAll(new DirectoryInfo(source.getLocation()), new DirectoryInfo(addonLocation + "\\" + sourceString));
            Addon clone = new Addon(sourceString, addonLocation + "\\" + sourceString);
            addons.Add(clone);
            clone.loadBlocks();
            clone.loadTextures();
            clone.loadPrefabs();
        }
        public void rename(String name, String newName)
        {
            Addon mod = getAddon(name);
            while (addons.BinarySearch(new Addon(newName, null), myComparer) > -1)
            {
                newName += "-d";
            }
            mod.setName(newName);
            Directory.Move(addonLocation + "\\" + name, addonLocation + "\\" + newName);
            mod.setLocation(addonLocation + "\\" + newName);
        }
        public void Remove(String name)
        {
            Addon mod = getAddon(name);
            mod.Delete();
            addons.RemoveAt(addons.BinarySearch(mod, myComparer));
        }
        public DataTable getTable(String name)
        {
            Addon mod = getAddon(name);
            return mod.getTable();
        }
        internal void Activate(String name)
        {
            Addon addon = getAddon(name);
            if (addon.hasTextures)
            {
                ArrayList images = addon.getTextures().getImages();
                foreach (Texture image in images)
                {
                    File.Copy(image.Path + @"-diffuse.dds", location + @"\Content\textures\" + addon.getName() + "-" + image.Name + @"-diffuse.dds");
                    File.Copy(image.Path + @"-special.dds", location + @"\Content\textures\" + addon.getName() + "-" + image.Name + @"-special.dds");

                }
            }
            if (addon.hasMaterials)
            {
                StreamReader reader = new System.IO.StreamReader(location+@"\Content\Data\solids.xml");
                String curXml = reader.ReadToEnd();
                reader.Close();
                String firstPart = Regex.Split(curXml, @"</Solids>")[0];
                String output = firstPart + ModWriter.addonTable(addon.getTable(), addon.getName()) + @"</Solids>";
                FileStream fs = File.Create(location + @"\Content\Data\solids.xml");
                StreamWriter file = new StreamWriter(fs);
                file.WriteLine(output);
                file.Close();
            }
            if (addon.hasPrefabs)
            {
                List<Prefab> prefabs = addon.getPrefabs();
                foreach (Prefab prefab in prefabs)
                {
                    CopyFolder.CopyAll(new DirectoryInfo(prefab.Path), new DirectoryInfo(location + @"\Content\prefabs\" + addon.getName() + "-" + prefab.Name));
                }
            }
            if (activeAddons == null)
            {
                setActiveAddons(new ArrayList());
            }
            activeAddons.Add(addon);
            setActiveAddons(getActiveAddons());
            
        }

        internal DataTable getNewTable(string name)
        {
            Addon addon = getAddon(name);
            addon.newTable();
            return addon.getTable();
        }

        internal void SetUnactiveAddons(string selected)
        {
            ArrayList cur = getActiveAddons();
            cur.Remove(getAddon(selected));
            setActiveAddons(cur);
        }
    }
    public class ModInfo
    {
        public String activeMod { get; set; }
        public ArrayList activeAddons { get; set; }
        public ModInfo()
        {
            activeAddons = new ArrayList();
        }
        internal void addAddon(string name)
        {
            if (activeAddons == null)
            {
                activeAddons = new ArrayList();
            }
            activeAddons.Add(name);
        }
    }
    [Serializable()]
    public class ModList
    {
        ModComparator myComparer = new ModComparator();
        ArrayList mods = new ArrayList();
        public String modLocation { get; set; }
        public String location { get; set; }
        public Mod activeMod { get; set; }
        public ModList(String location,ModInfo info)
        {
            this.location = location;
            this.modLocation = location+@"\Mods";
            DirectoryInfo directory = new DirectoryInfo(modLocation);

            DirectoryInfo[] modData = directory.GetDirectories();

            foreach (DirectoryInfo mod in modData)
            {
                mods.Add(new Mod(mod.Name,mod.FullName));
            }
            //textures = new Textures(location+@"\Content\textures");//Right place?
            if (info.activeMod == null)
            {
                firstRun();
            }
            else
            {
                String activeMod = info.activeMod;
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
            String output = ModWriter.writeInfo("Vanilla",new ArrayList());

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
                    mod.loadTextures();
                    mod.loadPrefabs();
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

        public DataTable getPrefabTable(String name)
        {
            return getMod(name).getPrefabTable();
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
            clone.loadTextures();
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

        public  void Activate(string selected)
        {
            Mod newMod = getMod(selected);
            Directory.Delete(location + @"\Content",true);
            CopyFolder.CopyAll(new DirectoryInfo(newMod.location), new DirectoryInfo(location+@"\Content"));
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
    public class AddonComparator : IComparer
    {
        public int Compare(object x, object y)
        {
           Addon x2 = (Addon)x;
            Addon y2 = (Addon)y;
            String paramX = x2.getName();
            String paramY = y2.getName();
            return String.Compare(paramX, paramY);
        }
    }
    public class Addon
    {
        private Mod mod;
        public bool hasMaterials;
        public bool hasTextures;
        public bool hasPrefabs;
        public Addon(String name, String location)
        {
            mod = new Mod(name, location);
        }
        public String getName()
        {
            return mod.Name;
        }
        public void setName(String name)
        {
            mod.Name = name;
        }
        public String getLocation()
        {
            return mod.location;
        }
        public void setLocation(String name)
        {
            mod.location = name;
        }

        public bool loadBlocks()
        {
            try
            {
                mod.loadBlocks();
            }
            catch (DirectoryNotFoundException)
            {
                hasMaterials = false;
                return false;
            }
            hasMaterials = true;
            return true;
        }

        public bool loadTextures()
        {
            try
            {
                mod.loadTextures();
            }
            catch (DirectoryNotFoundException)
            {
                hasTextures = false;
                return false;
            }
            hasTextures = true;
            return true;
        }
        public bool loadPrefabs()
        {
            try
            {
                mod.loadPrefabs();
            }
            catch (DirectoryNotFoundException)
            {
                hasPrefabs = false;
                return false;
            }
            hasPrefabs = true;
            return true;
        }
        public DataTable getTable()
        {
            return mod.getTable();
        }
        public Textures getTextures()
        {
            return mod.textures;
        }
        public DataTable getPrefabTable()
        {
            return mod.getPrefabTable();
        }
        public List<Prefab> getPrefabs()
        {
            return mod.getPrefabs();
        }
        public void Rename(String name)
        {
            mod.Name = name;
        }
        public Image getTexture(String name)
        {
            return mod.getTexture(name);
        }
        public void Save()
        {
            mod.Save();
        }
        public void Delete()
        {
            mod.Delete();
        }
        public DataTable getTextureTable()
        {
            return mod.getTextureTable();
        }

        internal void newTable()
        {
            mod.newTable();
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
        public Prefabs prefabs { get; set; }
        public Mod(String name, String location)
        {
            this.Name=name;
            this.location = location;
        }
        public void loadBlocks()
        {
            String path = location+ @"\Data\solids.xml";
            StreamReader stream;
            try
            {
                 stream = new StreamReader(path);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Mod " + Name + " threw a filenotFoundException");
            }
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
            
        }
        public void loadTextures()
        {
            textures = new Textures(location + @"\Textures");
        }
        public void loadPrefabs()
        {
            prefabs = new Prefabs(location + @"\Prefabs");
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
            if (textures == null) { return null; }
            return textures.getTable();
        }
        public Image getTexture(String name)
        {
            return textures.getTexture(name);
        }
        public DataTable getPrefabTable()
        {
            if (prefabs == null) { return null; }
            return prefabs.getTable();
        }
        public void Save()
        {
            String xml = ModWriter.outputTable(Blocks);
            System.IO.Directory.CreateDirectory(location);
            System.IO.Directory.CreateDirectory(location + @"\Data");
            FileStream fs = File.Create(location + @"\Data\solids.xml");
            StreamWriter file = new StreamWriter(fs);
            //StreamWriter file = new StreamWriter(location + @"\" + Name + @"\solids.xml");
            file.WriteLine(xml);
            file.Close();
         }
        public void Delete()
        {
            try
            {
                Directory.Delete(location, true);
            }
            catch (DirectoryNotFoundException)
            {

            }
        }

        internal void newTable()
        {
            Blocks = ModParser.emptySolidsTable();
        }

        internal List<Prefab> getPrefabs()
        {
            return prefabs.getPrefabs();
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
    public class Prefabs
    {
        List<Prefab> prefabs;
        DataTable dataTable;
        public Prefabs(String path)
        {
            prefabs = new List<Prefab>();

            dataTable = new DataTable();
            dataTable.Columns.Add("Prefab Name");

            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] prefabDirs = dir.GetDirectories();
            foreach(DirectoryInfo prefabDir in prefabDirs)
            {
                Prefab prefab = new Prefab(prefabDir.FullName);
                prefabs.Add(prefab);
                dataTable.Rows.Add(prefab.Name);
            }
        }
        public DataTable getTable()
        {
            return dataTable;
        }

        internal List<Prefab> getPrefabs()
        {
            return prefabs;
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
                texture = new Texture(file.DirectoryName, actualName);
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
        public List<String> getTextureNames()
        {
            List<String> names = new List<String>();
            foreach (Texture texture in images)
            {
                names.Add(texture.Name);
            }
            return names;
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
        public Image Specular {get;set;}
        public String Name { get; set; }
        public String RealName { get; set; }
        public String Path { get; set; }
        public Texture(Image image, String name)
        {
            this.Image = image;
            this.Name = name;
        }
        public Texture(String path, String name)
        {
            Path = path+"\\"+name;
            Name = name;
            try
            {
                Image = ImageTools.DDSDataToBMP(File.ReadAllBytes(Path+@"-diffuse.dds"));
                Specular= ImageTools.DDSDataToBMP(File.ReadAllBytes(Path+"-special.dds"));
            }
            catch (Exception)
            {
                Image = null;
                Specular=null;
            }
        }
    }
    public class Prefab
    {
        public String Name { get; set; }
        public String Path { get; set; }
        public String objectPath { get; set; }
        public String diffusePath { get; set; }
        public String specialPath { get; set; }
        public Prefab(String folderPath)
        {
            Path = folderPath;
            DirectoryInfo di = new DirectoryInfo(folderPath);
            Name = di.Name;
            objectPath = folderPath + @"\model.obj";
            diffusePath = folderPath + @"\texture-diffuse.dds";
            specialPath = folderPath + @"\texture-special.dds";

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
