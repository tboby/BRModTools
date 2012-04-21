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
            //ImageTools.loadImage();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
    /// <summary>
    /// This class reads in xml files and creates the relevant storage objects
    /// In future it will take whole mod folders, atm just solids.xml
    /// </summary>
    public static class ModParser
    {
       
        public static DataTable solidsTable(String solids, String modInformation)
        {
            DataTable output = new DataTable("Materials");
            String[] solidParams = { "Category", "Type", "DescLong", "DescShort", "Flags", "HP", "Mass", "RenderType", "Tags", "TexturePath", "ModelPath" };
            String xmlInput = new System.IO.StreamReader(solids).ReadToEnd();
            output.Columns.Add("Name");
            foreach (String param in solidParams)
            {
                output.Columns.Add(param);
            }
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlInput)))
                {
                    reader.ReadToFollowing("Solids");//Skips to first element
                    while (reader.Read())//Moves past it
                    {
                        reader.MoveToContent();//Moves to next element
                        if (reader.NodeType != XmlNodeType.EndElement && reader.Name!="")
                        {
                           // output.Rows.Add(
                            Block block = new Block(reader.Name,solidParams);
                            foreach (String parm in solidParams)
                            {
                                block.setParam(parm, reader.Value);
                                reader.MoveToAttribute(parm);
                                block.setParam(reader.Name, reader.Value);
                            }
                            output.Rows.Add(block.getRow());
                        }
                    }


                }
            }
            catch (Exception)
            {
                throw new System.FormatException("Error in XML file, are you sure it's formatted right?");
            }
            return output;
        }
        public static String readInfo(String modInfo)
        {
            String activeMod;
            String xmlInput = new System.IO.StreamReader(modInfo).ReadToEnd();
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlInput)))
                {
                    reader.ReadToFollowing("ActiveMod");//Skips to first element
                    reader.MoveToAttribute("Name");
                    activeMod = reader.Value;
                }
            }
            catch (Exception)
            {
                throw new System.FormatException("Error in XML file, are you sure it's formatted right?");
            }
            return activeMod;
        }
    }
    /// <summary>
    /// Takes the mod files and writes them to file
    /// </summary>
    static class ModWriter
    {
     
        public static String outputTable(DataTable data)
        {
            String[] attributes = new String[data.Columns.Count];
            for(int i=0;i<attributes.Length;i++)
            {
                attributes[i]=data.Columns[i].Caption;
            }
            String output = "<?xml version=\"1.0\"?>\r\n<Solids>\r\n";
            foreach(DataRow row in data.Rows)
            {
                output += "     <" + row["Name"] + " ";
                foreach (String attribute in attributes)
                {
                    if (attribute != "Name")
                    {
                        output += attribute + "=\"" + row[attribute] + "\" ";
                    }
                }
                output += "/>\r\n";
            }
            output += "</Solids>";
            return output;
        }
        //TODO: Make it more xml automatic
        internal static String writeInfo(string activeMod)
        {
            String output = "<?xml version=\"1.0\"?>\r\n<ModInfo>\r\n<ActiveMod Name=\"";
            output += activeMod;
            output+="\"/>\r\n</ModInfo>";
            return output;
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
        String[] parameters;

        public Block(String name, String[] parameters)
        {
            this.parameters = parameters;
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

        public String getParam(String param)
        {
            BlockProperties.Sort(myComparer);
            if (param.Equals("Name")) { return Name; }
            else
            {
                try
                {
                    BlockProperty prop = (BlockProperty)BlockProperties[BlockProperties.BinarySearch(new BlockProperty(param), myComparer)];
                    return prop.Value;
                }
                catch (IndexOutOfRangeException)
                {
                    return "";
                }
                    
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

        public Object[] getRow()
        {
            Object[] returnArray = new Object[parameters.Length+1];
            returnArray[0] = Name;
            for(int i = 0; i<parameters.Length;i++)
            {
                returnArray[i+1] = getParam(parameters[i]);
            }
            return returnArray;
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
        public BlockProperty(String property, String value)
        {
            this.Property = property;
            this.Value = value;
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
