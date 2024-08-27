namespace Group_50_CMPG223_HotelManagementSystem
{
    partial class Main_Form
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnManageEmployees = new System.Windows.Forms.Button();
            this.btnManageGuests = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGoBack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.btnManageEmployees);
            this.groupBox1.Controls.Add(this.btnManageGuests);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(261, 145);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(331, 297);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main System";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(59, 214);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(185, 39);
            this.button4.TabIndex = 5;
            this.button4.Text = "Request report";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(59, 154);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(185, 33);
            this.button3.TabIndex = 4;
            this.button3.Text = "Manage rooms";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnManageEmployees
            // 
            this.btnManageEmployees.Location = new System.Drawing.Point(59, 94);
            this.btnManageEmployees.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnManageEmployees.Name = "btnManageEmployees";
            this.btnManageEmployees.Size = new System.Drawing.Size(185, 34);
            this.btnManageEmployees.TabIndex = 3;
            this.btnManageEmployees.Text = "Manage employees";
            this.btnManageEmployees.UseVisualStyleBackColor = true;
            this.btnManageEmployees.Click += new System.EventHandler(this.btnManageEmployees_Click);
            // 
            // btnManageGuests
            // 
            this.btnManageGuests.Location = new System.Drawing.Point(59, 34);
            this.btnManageGuests.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnManageGuests.Name = "btnManageGuests";
            this.btnManageGuests.Size = new System.Drawing.Size(185, 34);
            this.btnManageGuests.TabIndex = 2;
            this.btnManageGuests.Text = "Manage guests";
            this.btnManageGuests.UseVisualStyleBackColor = true;
            this.btnManageGuests.Click += new System.EventHandler(this.btnManageGuests_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 16);
            this.label3.TabIndex = 1;
            // 
            // btnGoBack
            // 
            this.btnGoBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoBack.Location = new System.Drawing.Point(333, 467);
            this.btnGoBack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.Size = new System.Drawing.Size(143, 52);
            this.btnGoBack.TabIndex = 16;
            this.btnGoBack.Text = "Go back to login";
            this.btnGoBack.UseVisualStyleBackColor = true;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(246, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(419, 29);
            this.label1.TabIndex = 15;
            this.label1.Text = "Welcome to Luxury Hotel System";
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 576);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGoBack);
            this.Controls.Add(this.label1);
            this.Name = "Main_Form";
            this.Text = "Main_Form";
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnManageEmployees;
        private System.Windows.Forms.Button btnManageGuests;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGoBack;
        private System.Windows.Forms.Label label1;
    }
}