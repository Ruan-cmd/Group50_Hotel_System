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

        private void btnManageGuests_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Guests manage_guests = new Manage_Guests();
            manage_guests.ShowDialog();
            this.Close();
        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            Manage_Employees manage_Employees = new Manage_Employees();
            manage_Employees.ShowDialog();
        }

        private void btnManageRooms_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Rooms manage_Rooms = new Manage_Rooms();
            manage_Rooms.ShowDialog();
            this.Close();
        }

        private void btnRequestReport_Click(object sender, EventArgs e)
        {

        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login Login = new Login();
            Login.ShowDialog();
            this.Close();
        }
    }
}
