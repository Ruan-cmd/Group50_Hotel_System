using AxAcroPDFLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            try
            {
                // Extract the embedded PDF resource
                string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "UserManual.pdf");

                // Write the PDF to a temporary location
                System.IO.File.WriteAllBytes(tempPath, Properties.Resources.UserManual);

                // Check if the PDF file exists at the temporary location
                if (System.IO.File.Exists(tempPath))
                {
                    // Load the PDF file into the Adobe PDF Reader control
                    axAcroPDF1.LoadFile(tempPath);
                    axAcroPDF1.setView("Fit");  // Set the view to fit the PDF to the viewer
                    axAcroPDF1.setShowToolbar(false);  // Hide the toolbar for a cleaner look
                }
                else
                {
                    MessageBox.Show("Help file not found.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to load the help file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
}
