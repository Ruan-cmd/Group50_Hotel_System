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
    public partial class Review_Hotel : Form
    {
        public int SelectedRating { get; private set; }

        public Review_Hotel()
        {
            InitializeComponent();

        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Assume you have star radio buttons named starRadioButton1, starRadioButton2, etc.
            if (starRadioButton1.Checked)
                SelectedRating = 1;
            else if (starRadioButton2.Checked)
                SelectedRating = 2;
            else if (starRadioButton3.Checked)
                SelectedRating = 3;
            else if (starRadioButton4.Checked)
                SelectedRating = 4;
            else if (starRadioButton5.Checked)
                SelectedRating = 5;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
