using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text;
using System.Data;


namespace BRModTools
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    /// <summary>
    /// This class reads in xml files and creates the relevant storage objects
    /// In future it will take whole mod folders, atm just solids.xml
    /// </summary>
    public static class ModParser
    {
        /// <summary>
        /// Takes the solids xml file and parses it
        /// </summary>
        /// <returns>The solids object containingall the content</returns>
        public static Solids inputSolids(StreamReader stream)
        {
            //TODO Magic parameters, need a standardised list from ZanMgt
            String[] solidParams = { "Category", "Class", "DescLong", "DescShort", "Flags", "Geometry", "HP", "Mass", "Render", "Tags", "Texture" };
            Solids solids = new Solids();
            //TODO variable input location
            String xmlInput = stream.ReadToEnd();
            //String xmlInput = new System.IO.StreamReader(@"D:\Games\BlockadeRunner0.54.0\Blockade Runner 0.54.0\Mods\default\Solids.xml").ReadToEnd();

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlInput)))
            {
                reader.ReadToFollowing("Solids");//Skips to first element
                while (reader.Read())//Moves past it
                {
                    reader.MoveToContent();//Moves to next element
                    if (reader.NodeType != XmlNodeType.EndElement)
                    {
                        Block block = solids.addBlock(reader.Name, solidParams);//Take element Name and magic params
                        foreach (String parm in solidParams)
                        {
                            reader.MoveToAttribute(parm);
                            block.setParam(parm, reader.Value);
                        }
                    }
                }


            }
            return solids;
        }
    }
    /// <summary>
    /// Takes the mod files and writes them to file
    /// </summary>
    static class ModWriter
    {
        /// <summary>
        /// Takes a Solids object and returns the xml file representing it
        /// </summary>
        /// <param name="solids">A solids object</param>
        /// <returns>The string, formatted to be written to file</returns>
        public static String outputSolids(Solids solids)
        {
            String output = "<?xml version=\"1.0\"?>\r\n<Solids>\r\n";
            ArrayList blocks = solids.getBlocks();//TODO get rid of the need for getBlocks, use length()
            foreach (Block block in blocks)
            {
                output = output + block.getXml() + "\r\n";

            }
            output = output + "</Solids>";
            return output;
        }
    }
    /// <summary>
    /// The solids.xml storage object
    /// Holds a list of all block types along with any other data in the xml
    /// </summary>
    public class Solids
    {
        public DataTable BlockTable = new DataTable("Blocks");
        ArrayList blocks;
        //String[] solidParams = { "Mass", "HP", "Class", "Render", "Category", "Tags", "DescShort", "DescLong", "Flags", "Geometry", "Texture" };
        String[] solidParams = { "Category", "Class", "DescLong", "DescShort", "Flags", "Geometry", "HP", "Mass", "Render", "Tags", "Texture" };
        public Solids()
        {
            blocks = new ArrayList();
            
        }
        public void TableCreater()
        {
            BlockTable.Columns.Add("Name");
            foreach (String param in solidParams)
            {
                BlockTable.Columns.Add(param);
            }
            ArrayList temp;
            foreach (Block block in blocks)
            {
                temp =block.BlockProperties;
                String name = block.Name; 
                BlockTable.Rows.Add(name,((BlockProperty)temp[0]).Value, ((BlockProperty)temp[1]).Value, ((BlockProperty)temp[2]).Value, ((BlockProperty)temp[3]).Value, ((BlockProperty)temp[4]).Value, ((BlockProperty)temp[5]).Value, ((BlockProperty)temp[6]).Value, ((BlockProperty)temp[7]).Value, ((BlockProperty)temp[8]).Value, ((BlockProperty)temp[9]).Value, ((BlockProperty)temp[10]).Value);
            }
        }
        /// <summary>
        /// Add new blocks to the list of blocks in Solids.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Block addBlock(String name,String[] parameters)
        {
            Block newBlock = new Block(name, parameters);
            blocks.Add(newBlock);
            return newBlock;
        }
        /// <summary>
        /// Removes the block at this index in the arrayList (Not necessarily in display)
        /// </summary>
        /// <param name="index"></param>
        public void removeBlock(int index)
        {
            blocks.RemoveAt(index);
        }
        public ArrayList getBlocks()
        {
            return blocks;
        }
    }
    /// <summary>
    /// Stores a single block and all its properties
    /// </summary>
    public class Block
    {
        public String Name { get; set; }
        public ArrayList BlockProperties;
        private IComparer myComparer = new PropertyComparator();

        public Block(String name, String[] parameters)
        {
            BlockProperties = new ArrayList();
            this.Name = name;
            foreach (String parm in parameters)
            {
                BlockProperties.Add(new BlockProperty(parm));
            }
        }

        public void setParam(String param, String value)
        {
            BlockProperties.Sort(myComparer);
            if(param.Equals("Name")){Name=value;} else
            {
            BlockProperty prop = (BlockProperty)BlockProperties[BlockProperties.BinarySearch(new BlockProperty(param), myComparer)];
            prop.Value = value;
            }
            
        }

        public String getXml()
        {
            String output = "     <"+Name+" ";//4 spaces,<, name then space.
            foreach (BlockProperty prop in BlockProperties)
            {
                output = output + prop.Property + "=\"" + prop.Value + "\" ";
            }
            output = output + "/>";
            return output;
        }


    }

    class BlockProperty
    {
        //TODO: Protection on Property
        public String Property { get; set; }
        public String Value { get; set; }

        public BlockProperty(String property)
        {
            this.Property=property;
        }


    }
    
    public class PropertyComparator : IComparer
    {

        public int Compare(object x, object y)
        {
            BlockProperty x2 = (BlockProperty)x;
            BlockProperty y2 = (BlockProperty)y;
            String paramX = x2.Property;
            String paramY = y2.Property;
            return String.Compare(paramX, paramY);
            //return String.Compare((String)x, (String)y);
        }
    }
}
