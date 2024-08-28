namespace Group_50_CMPG223_HotelManagementSystem
{
    partial class Manage_Rooms
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
            this.tcRooms = new System.Windows.Forms.TabControl();
            this.tpOverview = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblRoomSelected = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvRoom = new System.Windows.Forms.DataGridView();
            this.tpAddRooms = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnAddRoomUpdate = new System.Windows.Forms.Button();
            this.btnAddRooms = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRoomType = new System.Windows.Forms.TextBox();
            this.txtRoomNum = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcRooms.SuspendLayout();
            this.tpOverview.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoom)).BeginInit();
            this.tpAddRooms.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcRooms
            // 
            this.tcRooms.Controls.Add(this.tpOverview);
            this.tcRooms.Controls.Add(this.tpAddRooms);
            this.tcRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcRooms.Location = new System.Drawing.Point(0, 28);
            this.tcRooms.Margin = new System.Windows.Forms.Padding(4);
            this.tcRooms.Name = "tcRooms";
            this.tcRooms.SelectedIndex = 0;
            this.tcRooms.Size = new System.Drawing.Size(985, 526);
            this.tcRooms.TabIndex = 3;
            // 
            // tpOverview
            // 
            this.tpOverview.Controls.Add(this.groupBox4);
            this.tpOverview.Controls.Add(this.groupBox5);
            this.tpOverview.Controls.Add(this.groupBox1);
            this.tpOverview.Location = new System.Drawing.Point(4, 25);
            this.tpOverview.Margin = new System.Windows.Forms.Padding(4);
            this.tpOverview.Name = "tpOverview";
            this.tpOverview.Padding = new System.Windows.Forms.Padding(4);
            this.tpOverview.Size = new System.Drawing.Size(977, 497);
            this.tpOverview.TabIndex = 0;
            this.tpOverview.Text = "Overview";
            this.tpOverview.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblRoomSelected);
            this.groupBox4.Controls.Add(this.btnUpdate);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Location = new System.Drawing.Point(524, 220);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(365, 228);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Actions";
            // 
            // lblRoomSelected
            // 
            this.lblRoomSelected.AutoSize = true;
            this.lblRoomSelected.Location = new System.Drawing.Point(76, 32);
            this.lblRoomSelected.Name = "lblRoomSelected";
            this.lblRoomSelected.Size = new System.Drawing.Size(101, 16);
            this.lblRoomSelected.TabIndex = 2;
            this.lblRoomSelected.Text = "Room Selected";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(76, 126);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(185, 52);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update Room";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(76, 55);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(185, 49);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Delete Room";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Location = new System.Drawing.Point(524, 7);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(365, 206);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Filter";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 37);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(349, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(117, 33);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(135, 22);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvRoom);
            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(508, 481);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Room Information";
            // 
            // dgvRoom
            // 
            this.dgvRoom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoom.Location = new System.Drawing.Point(0, 23);
            this.dgvRoom.Margin = new System.Windows.Forms.Padding(4);
            this.dgvRoom.Name = "dgvRoom";
            this.dgvRoom.ReadOnly = true;
            this.dgvRoom.RowHeadersWidth = 51;
            this.dgvRoom.Size = new System.Drawing.Size(473, 448);
            this.dgvRoom.TabIndex = 0;
            this.dgvRoom.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoom_CellClick);
            // 
            // tpAddRooms
            // 
            this.tpAddRooms.Controls.Add(this.groupBox7);
            this.tpAddRooms.Location = new System.Drawing.Point(4, 25);
            this.tpAddRooms.Margin = new System.Windows.Forms.Padding(4);
            this.tpAddRooms.Name = "tpAddRooms";
            this.tpAddRooms.Size = new System.Drawing.Size(977, 497);
            this.tpAddRooms.TabIndex = 3;
            this.tpAddRooms.Text = "Add Room";
            this.tpAddRooms.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnAddRoomUpdate);
            this.groupBox7.Controls.Add(this.btnAddRooms);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.txtRoomType);
            this.groupBox7.Controls.Add(this.txtRoomNum);
            this.groupBox7.Location = new System.Drawing.Point(20, 12);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(377, 293);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Room Information";
            // 
            // btnAddRoomUpdate
            // 
            this.btnAddRoomUpdate.Location = new System.Drawing.Point(111, 232);
            this.btnAddRoomUpdate.Name = "btnAddRoomUpdate";
            this.btnAddRoomUpdate.Size = new System.Drawing.Size(149, 54);
            this.btnAddRoomUpdate.TabIndex = 10;
            this.btnAddRoomUpdate.Text = "Update";
            this.btnAddRoomUpdate.UseVisualStyleBackColor = true;
            this.btnAddRoomUpdate.Click += new System.EventHandler(this.btnAddRoomUpdate_Click);
            // 
            // btnAddRooms
            // 
            this.btnAddRooms.Location = new System.Drawing.Point(111, 156);
            this.btnAddRooms.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddRooms.Name = "btnAddRooms";
            this.btnAddRooms.Size = new System.Drawing.Size(149, 68);
            this.btnAddRooms.TabIndex = 9;
            this.btnAddRooms.Text = "Add Room";
            this.btnAddRooms.UseVisualStyleBackColor = true;
            this.btnAddRooms.Click += new System.EventHandler(this.btnAddRooms_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 97);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Room Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 50);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Room Number:";
            // 
            // txtRoomType
            // 
            this.txtRoomType.Location = new System.Drawing.Point(201, 94);
            this.txtRoomType.Margin = new System.Windows.Forms.Padding(4);
            this.txtRoomType.Name = "txtRoomType";
            this.txtRoomType.Size = new System.Drawing.Size(132, 22);
            this.txtRoomType.TabIndex = 4;
            // 
            // txtRoomNum
            // 
            this.txtRoomNum.Location = new System.Drawing.Point(201, 43);
            this.txtRoomNum.Margin = new System.Windows.Forms.Padding(4);
            this.txtRoomNum.Name = "txtRoomNum";
            this.txtRoomNum.Size = new System.Drawing.Size(132, 22);
            this.txtRoomNum.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(985, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // Manage_Rooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 554);
            this.Controls.Add(this.tcRooms);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Manage_Rooms";
            this.Text = "Manage_Rooms";
            this.Load += new System.EventHandler(this.Manage_Rooms_Load);
            this.tcRooms.ResumeLayout(false);
            this.tpOverview.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoom)).EndInit();
            this.tpAddRooms.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcRooms;
        private System.Windows.Forms.TabPage tpOverview;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvRoom;
        private System.Windows.Forms.TabPage tpAddRooms;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnAddRooms;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRoomType;
        private System.Windows.Forms.TextBox txtRoomNum;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Label lblRoomSelected;
        private System.Windows.Forms.Button btnAddRoomUpdate;
    }
}