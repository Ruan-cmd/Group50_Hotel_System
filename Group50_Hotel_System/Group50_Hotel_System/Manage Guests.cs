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
    public partial class Manage_Guests : Form
    {
        public Manage_Guests()
        {
            InitializeComponent();
        }


        private void bookingFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Booking_Form bookingForm = new Booking_Form();
            bookingForm.ShowDialog();
            this.Show();
        }

        private void checkInoutFormToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Hide();
            CheckInOut_Form checkInOutForm = new CheckInOut_Form();
            checkInOutForm.ShowDialog();
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main_Form main_Form = new Main_Form();
            main_Form.ShowDialog();
            this.Close();
        }
    }
}
