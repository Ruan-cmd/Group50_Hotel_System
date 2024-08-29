namespace Group50_Hotel_System
{
    partial class Review_Hotel
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
            this.starRadioButton1 = new Group50_Hotel_System.StarRadioButton();
            this.starRadioButton2 = new Group50_Hotel_System.StarRadioButton();
            this.starRadioButton3 = new Group50_Hotel_System.StarRadioButton();
            this.SuspendLayout();
            // 
            // starRadioButton1
            // 
            this.starRadioButton1.Appearance = System.Windows.Forms.Appearance.Button;
            this.starRadioButton1.AutoSize = true;
            this.starRadioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.starRadioButton1.Location = new System.Drawing.Point(62, 40);
            this.starRadioButton1.Name = "starRadioButton1";
            this.starRadioButton1.Size = new System.Drawing.Size(122, 28);
            this.starRadioButton1.TabIndex = 0;
            this.starRadioButton1.TabStop = true;
            this.starRadioButton1.Text = "starRadioButton1";
            this.starRadioButton1.UseVisualStyleBackColor = true;
            // 
            // starRadioButton2
            // 
            this.starRadioButton2.Appearance = System.Windows.Forms.Appearance.Button;
            this.starRadioButton2.AutoSize = true;
            this.starRadioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.starRadioButton2.Location = new System.Drawing.Point(62, 75);
            this.starRadioButton2.Name = "starRadioButton2";
            this.starRadioButton2.Size = new System.Drawing.Size(122, 28);
            this.starRadioButton2.TabIndex = 1;
            this.starRadioButton2.TabStop = true;
            this.starRadioButton2.Text = "starRadioButton2";
            this.starRadioButton2.UseVisualStyleBackColor = true;
            // 
            // starRadioButton3
            // 
            this.starRadioButton3.Appearance = System.Windows.Forms.Appearance.Button;
            this.starRadioButton3.AutoSize = true;
            this.starRadioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.starRadioButton3.Location = new System.Drawing.Point(62, 110);
            this.starRadioButton3.Name = "starRadioButton3";
            this.starRadioButton3.Size = new System.Drawing.Size(122, 28);
            this.starRadioButton3.TabIndex = 2;
            this.starRadioButton3.TabStop = true;
            this.starRadioButton3.Text = "starRadioButton3";
            this.starRadioButton3.UseVisualStyleBackColor = true;
            // 
            // Review_Hotel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 270);
            this.Controls.Add(this.starRadioButton3);
            this.Controls.Add(this.starRadioButton2);
            this.Controls.Add(this.starRadioButton1);
            this.Name = "Review_Hotel";
            this.Text = "Review_Hotel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StarRadioButton starRadioButton1;
        private StarRadioButton starRadioButton2;
        private StarRadioButton starRadioButton3;
    }
}