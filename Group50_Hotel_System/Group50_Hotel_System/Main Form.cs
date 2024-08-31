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
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            Manage_Employees manage_Employees = new Manage_Employees();
            manage_Employees.ShowDialog();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login Login = new Login();
            Login.ShowDialog();
            this.Close();
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
        }

        private void btnBooking_Click(object sender, EventArgs e)
        {
            this.Hide();
            Booking_Form booking_Form = new Booking_Form();
            booking_Form.ShowDialog();
            this.Close();
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CheckInOut_Form checkInOut_Form = new CheckInOut_Form();
            checkInOut_Form.ShowDialog();
            this.Close();
        }

        private void btnManageRooms_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Rooms manage_Rooms = new Manage_Rooms();
            manage_Rooms.ShowDialog();
            this.Close();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            this.Hide();
            Request_Report request_Report = new Request_Report();
            request_Report.ShowDialog();
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the path to the PDF file relative to the application's startup directory.
                string pdfPath = System.IO.Path.Combine(Application.StartupPath, "UserManual.pdf");

                if (System.IO.File.Exists(pdfPath))
                {
                    // Open the Help form
                    Help helpForm = new Help();
                    helpForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Help file not found. Please ensure that 'UserManual.pdf' is in the correct location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the help file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
