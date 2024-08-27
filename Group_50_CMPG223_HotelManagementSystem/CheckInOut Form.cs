using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group_50_CMPG223_HotelManagementSystem
{
    public partial class CheckInOut_Form : Form
    {
        public CheckInOut_Form()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manage_Guests manage_Guests = new Manage_Guests();
            manage_Guests.ShowDialog();
            this.Close();
        }

        private void dtvBookingOverview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnQuestsUpdate_Click(object sender, EventArgs e)
        {
            //hello
        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbTypeOfBank_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCheckedClearControls_Click(object sender, EventArgs e)
        {
            txtBookingIDnum.Clear();
            txtBookingName.Clear();
            txtBookingSurname.Clear();
            txtBookingContactNum.Clear();
            txtBookingEmail.Clear();
            txtBookingStreet.Clear();
            txtBookingCity.Clear();
            txtCardHolder.Clear();
            cbBankType.SelectedIndex = -1;
            txtCardNumber.Clear();
            cbCardType.SelectedIndex = -1;
            cbDebitOrCredit.SelectedIndex = -1;
            cbMonth.SelectedIndex = -1;
            cbYear.SelectedIndex = -1;
            dtpCheckInDate.Value = DateTime.Now;
            dtpCheckOutDate.Value = DateTime.Now;
            lblCheckInRSelected.Text = "No Room Selected!";
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel18_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void CheckInOut_Form_Load(object sender, EventArgs e)
        {
           
        }
    }
}
