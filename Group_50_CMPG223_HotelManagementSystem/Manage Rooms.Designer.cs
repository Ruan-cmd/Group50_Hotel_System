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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOverviewRooms = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_Room = new System.Windows.Forms.DataGridView();
            this.tabAddRooms = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnAddRooms = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRoomType_Insert = new System.Windows.Forms.TextBox();
            this.txtRoomNum_Insert = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabOverviewRooms.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Room)).BeginInit();
            this.tabAddRooms.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabOverviewRooms);
            this.tabControl1.Controls.Add(this.tabAddRooms);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(739, 426);
            this.tabControl1.TabIndex = 3;
            // 
            // tabOverviewRooms
            // 
            this.tabOverviewRooms.Controls.Add(this.groupBox4);
            this.tabOverviewRooms.Controls.Add(this.groupBox5);
            this.tabOverviewRooms.Controls.Add(this.groupBox1);
            this.tabOverviewRooms.Location = new System.Drawing.Point(4, 22);
            this.tabOverviewRooms.Name = "tabOverviewRooms";
            this.tabOverviewRooms.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverviewRooms.Size = new System.Drawing.Size(731, 400);
            this.tabOverviewRooms.TabIndex = 0;
            this.tabOverviewRooms.Text = "Overview";
            this.tabOverviewRooms.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnUpdate);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Location = new System.Drawing.Point(393, 179);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(274, 185);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Actions";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(57, 102);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(139, 42);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update Rooms";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(57, 45);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(139, 40);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Delete Rooms";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Location = new System.Drawing.Point(393, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(274, 167);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Filter";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSearch);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 81);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(88, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(102, 20);
            this.txtSearch.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Looking For:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView_Room);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 391);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Room Information";
            // 
            // dataGridView_Room
            // 
            this.dataGridView_Room.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Room.Location = new System.Drawing.Point(0, 19);
            this.dataGridView_Room.Name = "dataGridView_Room";
            this.dataGridView_Room.Size = new System.Drawing.Size(355, 364);
            this.dataGridView_Room.TabIndex = 0;
            // 
            // tabAddRooms
            // 
            this.tabAddRooms.Controls.Add(this.groupBox7);
            this.tabAddRooms.Location = new System.Drawing.Point(4, 22);
            this.tabAddRooms.Name = "tabAddRooms";
            this.tabAddRooms.Size = new System.Drawing.Size(677, 403);
            this.tabAddRooms.TabIndex = 3;
            this.tabAddRooms.Text = "Add rooms";
            this.tabAddRooms.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnAddRooms);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.txtRoomType_Insert);
            this.groupBox7.Controls.Add(this.txtRoomNum_Insert);
            this.groupBox7.Location = new System.Drawing.Point(15, 10);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(283, 238);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Room Information";
            // 
            // btnAddRooms
            // 
            this.btnAddRooms.Location = new System.Drawing.Point(83, 127);
            this.btnAddRooms.Name = "btnAddRooms";
            this.btnAddRooms.Size = new System.Drawing.Size(112, 55);
            this.btnAddRooms.TabIndex = 9;
            this.btnAddRooms.Text = "Add Room";
            this.btnAddRooms.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Room Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Room Number:";
            // 
            // txtRoomType_Insert
            // 
            this.txtRoomType_Insert.Location = new System.Drawing.Point(151, 76);
            this.txtRoomType_Insert.Name = "txtRoomType_Insert";
            this.txtRoomType_Insert.Size = new System.Drawing.Size(100, 20);
            this.txtRoomType_Insert.TabIndex = 4;
            // 
            // txtRoomNum_Insert
            // 
            this.txtRoomNum_Insert.Location = new System.Drawing.Point(151, 35);
            this.txtRoomNum_Insert.Name = "txtRoomNum_Insert";
            this.txtRoomNum_Insert.Size = new System.Drawing.Size(100, 20);
            this.txtRoomNum_Insert.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(739, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // Manage_Rooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Manage_Rooms";
            this.Text = "Manage_Rooms";
            this.tabControl1.ResumeLayout(false);
            this.tabOverviewRooms.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Room)).EndInit();
            this.tabAddRooms.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabOverviewRooms;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView_Room;
        private System.Windows.Forms.TabPage tabAddRooms;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnAddRooms;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRoomType_Insert;
        private System.Windows.Forms.TextBox txtRoomNum_Insert;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    }
}