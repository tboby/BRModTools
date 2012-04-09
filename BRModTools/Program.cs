using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text;


namespace BRModTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Solids solids = ModParser.inputSolids();
            String output = ModWriter.outputSolids(solids);
            StreamWriter file = new StreamWriter("test.xml");
            file.WriteLine(output);
            file.Close();
        }
    }
    /// <summary>
    /// This class reads in xml files and creates the relevant storage objects
    /// In future it will take whole mod folders, atm just solids.xml
    /// </summary>
    static class ModParser
    {
        /// <summary>
        /// Takes the solids xml file and parses it
        /// </summary>
        /// <returns>The solids object containingall the content</returns>
        public static Solids inputSolids()
        {
            //TODO Magic parameters, need a standardised list from ZanMgt
            String[] solidParams= {"Mass","HP","Class","Render","Category","Tags","DescShort","DescLong","Flags","Geometry","Texture"};
            Solids solids = new Solids();
            //TODO variable input location
            String xmlInput = new System.IO.StreamReader(@"D:\Games\BlockadeRunner0.54.0\Blockade Runner 0.54.0\Mods\default\Solids.xml").ReadToEnd();

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
    class Solids
    {
        ArrayList blocks;
        public Solids()
        {
            blocks = new ArrayList();
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
    class Block
    {
        public String Name { get; set; }
        private ArrayList BlockProperties;
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
            BlockProperty prop = (BlockProperty)BlockProperties[BlockProperties.BinarySearch(new BlockProperty(param), myComparer)];
            prop.Value = value;
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
