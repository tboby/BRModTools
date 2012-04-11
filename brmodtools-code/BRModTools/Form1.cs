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

namespace BRModTools
{
    public partial class Form1 : Form
    {
        String[] solidParams = { "Category", "Class", "DescLong", "DescShort", "Flags", "Geometry", "HP", "Mass", "Render", "Tags", "Texture" };
        DataTable data;
        public Form1()
        {
            InitializeComponent();
        }

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
            if (data != null)
            {
                Solids output = exportTable();
                String xml = ModWriter.outputSolids(output);
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Xml file|*.xml";
                saveFileDialog1.Title = "Save the Solids File";
                saveFileDialog1.ShowDialog();

                if (saveFileDialog1.FileName != "")
                {
                    FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                    StreamWriter file = new StreamWriter(fs);
                    file.WriteLine(xml);
                    file.Close();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Xml Files|*.xml";
            openFileDialog1.Title = "Select the solids xml file";
            StreamReader stream = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                stream = new StreamReader(openFileDialog1.FileName);
                Solids solids = ModParser.inputSolids(stream);
                solids.TableCreater();
                data = solids.BlockTable;
                this.dataGridView1.DataSource = data;
            }
            
        }
    }
}
