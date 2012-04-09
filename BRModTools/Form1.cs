using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRModTools
{
    public partial class Form1 : Form
    {
        String[] solidParams = { "Category", "Class", "DescLong", "DescShort", "Flags", "Geometry", "HP", "Mass", "Render", "Tags", "Texture" };
        DataTable data;
        public Form1()
        {
            InitializeComponent();
            Solids solids = ModParser.inputSolids();
            solids.TableCreater();
            data = solids.BlockTable;
            this.dataGridView1.DataSource = data;
            
            //solids.BlockTable.WriteXml(@"D:\Games\BlockadeRunner0.54.0\Blockade Runner 0.54.0\Mods\default\Solids3.xml");
        }

       /* public Solids exportTable()
        {
            Solids output = new Solids();
            foreach (DataRow row in data.Rows)
            {
                Block block=null;
                foreach (String column in data.Columns)
                {
                    if (column.Equals("Name"))
                    {
                        block = new Block((String)row["Name"], solidParams);
                    }
                    else
                    {
                        block.setParam(column, (String)row[column]);
                    }
                }
               

            }
            return output;
        }*/

        public Solids exportTable()
        {
            Solids output = new Solids();
            foreach (DataRow row in data.Rows)
            {
                Block block = output.addBlock((String)row["Name"], solidParams);
               
                foreach (DataColumn column in data.Columns)
                {
                    block.setParam(column.Caption, (String)row[column.Caption]);
                }


            }
            return output;
        }


        private void solidsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Solids output = exportTable();
            ModWriter.outputSolids(output);

        }
    }
}
