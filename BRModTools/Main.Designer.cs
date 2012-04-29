using System.Windows.Forms;
namespace BRModTools
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.setActiveMod = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.selectMod = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.create = new System.Windows.Forms.Button();
            this.selectModBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.editMaterial = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.serviceController1 = new System.ServiceProcess.ServiceController();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.activeDeactiveAddon = new System.Windows.Forms.Button();
            this.renameAddon = new System.Windows.Forms.Button();
            this.saveAddon = new System.Windows.Forms.Button();
            this.removeAddon = new System.Windows.Forms.Button();
            this.addAddon = new System.Windows.Forms.Button();
            this.selectAddonBox = new System.Windows.Forms.ListBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // setActiveMod
            // 
            this.setActiveMod.Enabled = false;
            this.setActiveMod.Location = new System.Drawing.Point(9, 299);
            this.setActiveMod.Name = "setActiveMod";
            this.setActiveMod.Size = new System.Drawing.Size(94, 23);
            this.setActiveMod.TabIndex = 6;
            this.setActiveMod.Text = "Set Active Mod";
            this.setActiveMod.UseVisualStyleBackColor = true;
            this.setActiveMod.Click += new System.EventHandler(this.setActiveMod_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(9, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Save changes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(123, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Rename";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // selectMod
            // 
            this.selectMod.Location = new System.Drawing.Point(87, 391);
            this.selectMod.Name = "selectMod";
            this.selectMod.Size = new System.Drawing.Size(111, 23);
            this.selectMod.TabIndex = 3;
            this.selectMod.Text = "Select game folder";
            this.selectMod.UseVisualStyleBackColor = true;
            this.selectMod.Click += new System.EventHandler(this.selectMod_Click);
            // 
            // remove
            // 
            this.remove.Enabled = false;
            this.remove.Location = new System.Drawing.Point(106, 198);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(92, 23);
            this.remove.TabIndex = 2;
            this.remove.Text = "Remove mod";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // create
            // 
            this.create.Enabled = false;
            this.create.Location = new System.Drawing.Point(6, 198);
            this.create.Name = "create";
            this.create.Size = new System.Drawing.Size(94, 23);
            this.create.TabIndex = 1;
            this.create.Text = "Clone mod";
            this.create.UseVisualStyleBackColor = true;
            this.create.Click += new System.EventHandler(this.button1_Click);
            // 
            // selectModBox
            // 
            this.selectModBox.FormattingEnabled = true;
            this.selectModBox.Location = new System.Drawing.Point(6, 6);
            this.selectModBox.Name = "selectModBox";
            this.selectModBox.Size = new System.Drawing.Size(192, 186);
            this.selectModBox.TabIndex = 0;
            this.selectModBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawItemHandler);
            this.selectModBox.SelectedIndexChanged += new System.EventHandler(this.selectModBox_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(224, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 448);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.editMaterial);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 422);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Materials";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // editMaterial
            // 
            this.editMaterial.Enabled = false;
            this.editMaterial.Location = new System.Drawing.Point(470, 391);
            this.editMaterial.Name = "editMaterial";
            this.editMaterial.Size = new System.Drawing.Size(75, 23);
            this.editMaterial.TabIndex = 3;
            this.editMaterial.Text = "Edit Material";
            this.editMaterial.UseVisualStyleBackColor = true;
            this.editMaterial.Click += new System.EventHandler(this.editMaterial_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(89, 392);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "Remove Material";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(7, 392);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Add Material";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(7, 7);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(539, 379);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_DoubleClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(552, 422);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Textures";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(4, 4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridView2.Size = new System.Drawing.Size(545, 415);
            this.dataGridView2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView3);
            this.tabPage2.Controls.Add(this.listView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(552, 422);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Prefabs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(545, 415);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(6, 13);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(212, 448);
            this.tabControl2.TabIndex = 7;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.setActiveMod);
            this.tabPage4.Controls.Add(this.selectModBox);
            this.tabPage4.Controls.Add(this.create);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.remove);
            this.tabPage4.Controls.Add(this.selectMod);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(204, 422);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Mod Control";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.activeDeactiveAddon);
            this.tabPage5.Controls.Add(this.renameAddon);
            this.tabPage5.Controls.Add(this.saveAddon);
            this.tabPage5.Controls.Add(this.removeAddon);
            this.tabPage5.Controls.Add(this.addAddon);
            this.tabPage5.Controls.Add(this.selectAddonBox);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(204, 422);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Addon Control";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // activeDeactiveAddon
            // 
            this.activeDeactiveAddon.Location = new System.Drawing.Point(25, 286);
            this.activeDeactiveAddon.Name = "activeDeactiveAddon";
            this.activeDeactiveAddon.Size = new System.Drawing.Size(138, 23);
            this.activeDeactiveAddon.TabIndex = 6;
            this.activeDeactiveAddon.Text = "Activate/Deactive Addon";
            this.activeDeactiveAddon.UseVisualStyleBackColor = true;
            this.activeDeactiveAddon.Click += new System.EventHandler(this.activeDeactiveAddon_Click);
            // 
            // renameAddon
            // 
            this.renameAddon.Location = new System.Drawing.Point(99, 229);
            this.renameAddon.Name = "renameAddon";
            this.renameAddon.Size = new System.Drawing.Size(75, 23);
            this.renameAddon.TabIndex = 5;
            this.renameAddon.Text = "Rename";
            this.renameAddon.UseVisualStyleBackColor = true;
            this.renameAddon.Click += new System.EventHandler(this.renameAddon_Click);
            // 
            // saveAddon
            // 
            this.saveAddon.Location = new System.Drawing.Point(7, 229);
            this.saveAddon.Name = "saveAddon";
            this.saveAddon.Size = new System.Drawing.Size(85, 23);
            this.saveAddon.TabIndex = 4;
            this.saveAddon.Text = "Save Changes";
            this.saveAddon.UseVisualStyleBackColor = true;
            this.saveAddon.Click += new System.EventHandler(this.saveAddon_Click);
            // 
            // removeAddon
            // 
            this.removeAddon.Location = new System.Drawing.Point(89, 199);
            this.removeAddon.Name = "removeAddon";
            this.removeAddon.Size = new System.Drawing.Size(99, 23);
            this.removeAddon.TabIndex = 3;
            this.removeAddon.Text = "Remove Addon";
            this.removeAddon.UseVisualStyleBackColor = true;
            this.removeAddon.Click += new System.EventHandler(this.removeAddon_Click);
            // 
            // addAddon
            // 
            this.addAddon.Location = new System.Drawing.Point(7, 199);
            this.addAddon.Name = "addAddon";
            this.addAddon.Size = new System.Drawing.Size(75, 23);
            this.addAddon.TabIndex = 2;
            this.addAddon.Text = "Add Addon";
            this.addAddon.UseVisualStyleBackColor = true;
            this.addAddon.Click += new System.EventHandler(this.addAddon_Click);
            // 
            // selectAddonBox
            // 
            this.selectAddonBox.FormattingEnabled = true;
            this.selectAddonBox.Location = new System.Drawing.Point(6, 6);
            this.selectAddonBox.Name = "selectAddonBox";
            this.selectAddonBox.Size = new System.Drawing.Size(192, 186);
            this.selectAddonBox.TabIndex = 1;
            this.selectAddonBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawAddonItemHandler);
            this.selectAddonBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(4, 4);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(545, 415);
            this.dataGridView3.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 473);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Main";
            this.Text = "BlockadeRunner Mod Tools";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button selectMod;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button create;
        private System.Windows.Forms.ListBox selectModBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.ServiceProcess.ServiceController serviceController1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button editMaterial;
        private Button setActiveMod;
        private TabPage tabPage2;
        private ListView listView1;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private ListBox selectAddonBox;
        private Button activeDeactiveAddon;
        private Button renameAddon;
        private Button saveAddon;
        private Button removeAddon;
        private Button addAddon;
        private DataGridView dataGridView3;
    }
}