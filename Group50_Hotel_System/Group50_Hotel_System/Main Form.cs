using Group_50_CMPG223_HotelManagementSystem;
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
            string managerRole = "Manager";

            if (SessionManager.LoggedInEmployeeRole != managerRole)
            {
                btnRequest.Visible = false;
                btnManageEmployees.Visible = false;
            }
            else
            {
                btnRequest.Visible = true;
                btnManageEmployees.Visible = true;
            }


            toolTip1.SetToolTip(btnBooking, "Add, Update and remove Guest Bookings");
            toolTip1.SetToolTip(btnCheckIn, "Check in, update and Check out Guests");
            toolTip1.SetToolTip(btnManageRooms, "Add, Update and Remove Rooms");
            toolTip1.SetToolTip(btnManageEmployees, "Add, Update and Remove Employees (Reset Employee Passwords)");
            toolTip1.SetToolTip(btnRequest, "Request a report for a specific time period");
            toolTip1.SetToolTip(btnHelp, "See User Manueal");
            toolTip1.SetToolTip(btnGoBack, "Go back to Login");
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
            Help helpForm = new Help();
            helpForm.ShowDialog();
        }
    }
}
