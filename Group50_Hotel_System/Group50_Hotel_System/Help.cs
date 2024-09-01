using System;
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
            // The file is now part of the project, so it will be copied to the output directory (e.g., bin/Debug).
            // You can directly reference it without specifying a path.
            string pdfPath = System.IO.Path.Combine(Application.StartupPath, "UserManual.pdf");

            try
            {
                // Check if the PDF file exists
                if (System.IO.File.Exists(pdfPath))
                {
                    // Load the PDF file into the Adobe PDF Reader control
                    axAcroPDF1.LoadFile(pdfPath);
                    axAcroPDF1.setView("Fit");  // Set the view to fit the PDF to the viewer
                    axAcroPDF1.setShowToolbar(false);  // Hide the toolbar for a cleaner look
                }
                else
                {
                    MessageBox.Show("Help file not found. Please ensure that 'UserManual.pdf' is in the correct location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to load the help file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
